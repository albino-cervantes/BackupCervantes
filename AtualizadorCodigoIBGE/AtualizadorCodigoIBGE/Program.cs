using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Npgsql;
using Newtonsoft.Json;
using System.Threading;

namespace AtualizadorCodigoIBGE
{
    public class ViaCepResponse
    {
        public string cep { get; set; }
        public string logradouro { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string localidade { get; set; }
        public string uf { get; set; }
        public string ibge { get; set; }
        public string erro { get; set; }
    }

    public class CidadeInfo
    {
        public string Cidade { get; set; }
        public string UF { get; set; }
        public string CepExemplo { get; set; }
        public int TotalRegistros { get; set; }
    }

    public class Program
    {
        private const string ConnectionString = "Host=naviclouddb.cervantes.dev.br;Database=WebServiceCEP;Username=postgres;Password=1234567";
        private static readonly HttpClient httpClient = new HttpClient();

        public static async Task Main(string[] args)
        {
            Console.WriteLine("=== Atualizador de Código IBGE (Otimizado por Cidade) ===");
            Console.WriteLine($"Iniciado em: {DateTime.Now}");
            Console.WriteLine();

            try
            {
                var cidadesParaAtualizar = await ObterCidadesSemCodigoIBGE();

                if (cidadesParaAtualizar.Count == 0)
                {
                    Console.WriteLine("Nenhuma cidade encontrada sem código IBGE para atualizar.");
                    return;
                }

                Console.WriteLine($"Encontradas {cidadesParaAtualizar.Count} cidades únicas para atualizar.");
                Console.WriteLine();

                int sucessos = 0;
                int erros = 0;
                int totalCidades = cidadesParaAtualizar.Count;
                int totalRegistrosAtualizados = 0;

                for (int i = 0; i < cidadesParaAtualizar.Count; i++)
                {
                    var cidadeInfo = cidadesParaAtualizar[i];

                    try
                    {
                        Console.WriteLine($"[{i + 1}/{totalCidades}] Processando {cidadeInfo.Cidade}/{cidadeInfo.UF} ({cidadeInfo.TotalRegistros} registros)...");

                        var codigoIbge = await ConsultarCodigoIBGEViaCep(cidadeInfo.CepExemplo);

                        if (!string.IsNullOrEmpty(codigoIbge))
                        {
                            var registrosAtualizados = await AtualizarCodigoIBGEPorCidade(cidadeInfo.Cidade, cidadeInfo.UF, codigoIbge);
                            sucessos++;
                            totalRegistrosAtualizados += registrosAtualizados;
                            Console.WriteLine($"✓ {cidadeInfo.Cidade}/{cidadeInfo.UF} atualizada com código IBGE: {codigoIbge} ({registrosAtualizados} registros)");
                        }
                        else
                        {
                            erros++;
                            Console.WriteLine($"✗ {cidadeInfo.Cidade}/{cidadeInfo.UF}: Código IBGE não encontrado na consulta (CEP: {cidadeInfo.CepExemplo})");
                        }
                    }
                    catch (Exception ex)
                    {
                        erros++;
                        Console.WriteLine($"✗ Erro ao processar {cidadeInfo.Cidade}/{cidadeInfo.UF}: {ex.Message}");
                    }

                    // Aguarda 1 minuto entre requisições para não sobrecarregar a API
                    if (i < cidadesParaAtualizar.Count - 1) // Não aguarda na última iteração
                    {
                        Console.WriteLine("Aguardando 1 minuto antes da próxima consulta...");
                        await Task.Delay(60000); // 60 segundos
                    }
                }

                Console.WriteLine();
                Console.WriteLine("=== Relatório Final ===");
                Console.WriteLine($"Total de cidades processadas: {totalCidades}");
                Console.WriteLine($"Cidades atualizadas com sucesso: {sucessos}");
                Console.WriteLine($"Cidades com erro: {erros}");
                Console.WriteLine($"Total de registros atualizados: {totalRegistrosAtualizados}");
                Console.WriteLine($"Finalizado em: {DateTime.Now}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro geral na aplicação: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
            }

            Console.WriteLine();
            Console.WriteLine("Pressione qualquer tecla para sair...");
            Console.ReadKey();
        }

        private static async Task<List<CidadeInfo>> ObterCidadesSemCodigoIBGE()
        {
            var cidades = new List<CidadeInfo>();

            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                await connection.OpenAsync();

                var query = @"
                    SELECT 
                        cidade,
                        uf,
                        MIN(cep) as cep_exemplo,
                        COUNT(*) as total_registros
                    FROM endereco 
                    WHERE (codigo_ibge IS NULL OR codigo_ibge = '' OR TRIM(codigo_ibge) = '')
                      AND cidade IS NOT NULL 
                      AND uf IS NOT NULL
                      AND cep IS NOT NULL
                      AND TRIM(cidade) != ''
                      AND TRIM(uf) != ''
                      AND TRIM(cep) != ''
                    GROUP BY cidade, uf
                    ORDER BY cidade, uf";

                using (var command = new NpgsqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var cidadeOrdinal = reader.GetOrdinal("cidade");
                        var ufOrdinal = reader.GetOrdinal("uf");
                        var cepExemploOrdinal = reader.GetOrdinal("cep_exemplo");
                        var totalRegistrosOrdinal = reader.GetOrdinal("total_registros");

                        // Verifica se os campos obrigatórios não são nulos
                        if (!reader.IsDBNull(cidadeOrdinal) &&
                            !reader.IsDBNull(ufOrdinal) &&
                            !reader.IsDBNull(cepExemploOrdinal) &&
                            !reader.IsDBNull(totalRegistrosOrdinal))
                        {
                            cidades.Add(new CidadeInfo
                            {
                                Cidade = reader.GetString(cidadeOrdinal),
                                UF = reader.GetString(ufOrdinal),
                                CepExemplo = reader.GetString(cepExemploOrdinal),
                                TotalRegistros = reader.GetInt32(totalRegistrosOrdinal)
                            });
                        }
                    }
                }
            }

            return cidades;
        }

        private static async Task<string> ConsultarCodigoIBGEViaCep(string cep)
        {
            try
            {
                // Remove caracteres especiais do CEP
                var cepLimpo = cep.Replace("-", "").Replace(".", "").Replace(" ", "");

                var url = $"https://viacep.com.br/ws/{cepLimpo}/json/";

                var response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var viaCepResponse = JsonConvert.DeserializeObject<ViaCepResponse>(jsonContent);

                    // Verifica se houve erro na resposta ou se o IBGE está vazio
                    if (viaCepResponse != null &&
                        string.IsNullOrEmpty(viaCepResponse.erro) &&
                        !string.IsNullOrEmpty(viaCepResponse.ibge))
                    {
                        return viaCepResponse.ibge;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao consultar ViaCEP para CEP {cep}: {ex.Message}");
                return null;
            }
        }

        private static async Task<int> AtualizarCodigoIBGEPorCidade(string cidade, string uf, string codigoIbge)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                await connection.OpenAsync();

                var query = @"
                    UPDATE endereco 
                    SET codigo_ibge = @codigoIbge 
                    WHERE cidade = @cidade 
                      AND uf = @uf
                      AND (codigo_ibge IS NULL OR codigo_ibge = '' OR TRIM(codigo_ibge) = '')";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@codigoIbge", codigoIbge);
                    command.Parameters.AddWithValue("@cidade", cidade);
                    command.Parameters.AddWithValue("@uf", uf);

                    return await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
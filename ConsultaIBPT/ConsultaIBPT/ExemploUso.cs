using System;
using System.Threading.Tasks;

namespace IBPT.Api.Client
{
    #region Exemplo de Uso

    /// <summary>
    /// Classe de exemplo demonstrando como utilizar o IbptClient
    /// </summary>
    public class ExemploUso
    {
        /// <summary>
        /// Exemplo de consulta de tributos para produtos
        /// </summary>
        public static async Task ExemploConsultaProduto()
        {
            // Configuração do cliente IBPT
            const string cnpj = "08833101000155"; // Seu CNPJ autorizado
            const string token = "59gA7HsRIqS3MwpC7UgyRaDfCI277DPxjzb6m7ls2Sdt_PpeMTo53UcnF1ehrzn6"; // Seu token

            using (var ibptClient = new IbptClient(cnpj, token))
            {
                try
                {
                    // Consulta tributos de um produto
                    //var resultado = await ibptClient.ConsultarTributosProdutoAsync(
                    //    ncm: "60063210",
                    //    uf: "RS",
                    //    exTarif: 0,
                    //    descricao: "Tecido",
                    //    unidadeMedida: "un",
                    //    valor: 60m,
                    //    gtin: "SEM GTIN"
                    //);

                    var resultado = await ibptClient.ConsultarTributosProdutoAsync(
                        ncm: "60063210"
                        //,descricao: "Tecido"
                    );

                    // Exibe os resultados
                    Console.WriteLine($"Produto: {resultado.Descricao}");
                    Console.WriteLine($"NCM: {resultado.Codigo}");
                    Console.WriteLine($"Tributo Nacional: {resultado.Nacional:F2}% (R$ {resultado.ValorTributoNacional:F2})");
                    Console.WriteLine($"Tributo Estadual: {resultado.Estadual:F2}% (R$ {resultado.ValorTributoEstadual:F2})");
                    Console.WriteLine($"Tributo Municipal: {resultado.Municipal:F2}% (R$ {resultado.ValorTributoMunicipal:F2})");
                    Console.WriteLine($"Vigência: {resultado.VigenciaInicio} a {resultado.VigenciaFim}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro na consulta: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Exemplo de consulta de tributos para serviços
        /// </summary>
        public static async Task ExemploConsultaServico()
        {
            const string cnpj = "08833101000155";
            const string token = "seu_token_aqui";

            using (var ibptClient = new IbptClient(cnpj, token))
            {
                try
                {
                    var resultado = await ibptClient.ConsultarTributosServicoAsync(
                        uf: "RS",
                        codigo: "0101",
                        descricao: "Serviço de consultoria",
                        unidadeMedida: "h",
                        valor: 100m
                    );

                    Console.WriteLine($"Serviço: {resultado.Descricao}");
                    Console.WriteLine($"Código: {resultado.Codigo}");
                    Console.WriteLine($"Tributo Federal: {resultado.Federal:F2}% (R$ {resultado.ValorTributoFederal:F2})");
                    Console.WriteLine($"Tributo Municipal: {resultado.Municipal:F2}% (R$ {resultado.ValorTributoMunicipal:F2})");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro na consulta: {ex.Message}");
                }
            }
        }
    }

    #endregion
}
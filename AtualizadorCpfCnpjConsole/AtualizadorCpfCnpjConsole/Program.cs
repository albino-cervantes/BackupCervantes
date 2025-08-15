using System;
using System.Data.SQLite;
using System.Text.RegularExpressions;

namespace AtualizadorCpfCnpjConsole
{
    class Program
    {
        private static string connectionString = "Data Source=\\\\10.0.0.167\\temp\\TrocaCnpj\\alterado.db;Version=3;";

        static void Main(string[] args)
        {
            Console.WriteLine("=== Atualizador de CPF/CNPJ ===");
            Console.WriteLine();

            try
            {
                // Solicitar caminho do banco
                Console.Write("Digite o caminho do banco SQLite: ");
                string caminhoDatabase = Console.ReadLine();

                if (string.IsNullOrEmpty(caminhoDatabase))
                {
                    Console.WriteLine("Caminho do banco não pode ser vazio!");
                    return;
                }

                connectionString = $"Data Source={caminhoDatabase};Version=3;";

                // Listar empresas existentes
                var idEmpresa = ListarEmpresas();

                //// Solicitar ID da empresa
                //Console.Write("Digite o ID da empresa que deseja alterar: ");
                //string idEmpresa = Console.ReadLine();

                //if (string.IsNullOrEmpty(idEmpresa))
                //{
                //    Console.WriteLine("ID da empresa não pode ser vazio!");
                //    return;
                //}

                //// Verificar se empresa existe
                //if (!VerificarEmpresaExiste(idEmpresa))
                //{
                //    Console.WriteLine("Empresa não encontrada!");
                //    return;
                //}

                // Solicitar novo CPF/CNPJ
                Console.Write("Digite o novo CPF/CNPJ (apenas números): ");
                string novoCpfCnpj = Console.ReadLine();

                if (string.IsNullOrEmpty(novoCpfCnpj))
                {
                    Console.WriteLine("CPF/CNPJ não pode ser vazio!");
                    return;
                }

                // Remover caracteres especiais
                novoCpfCnpj = Regex.Replace(novoCpfCnpj, @"[^\d]", "");

                // Validar CPF/CNPJ
                bool ehCpf = ValidarCpf(novoCpfCnpj);
                bool ehCnpj = ValidarCnpj(novoCpfCnpj);

                if (!ehCpf && !ehCnpj)
                {
                    Console.WriteLine("CPF/CNPJ inválido!");
                    return;
                }

                // Confirmar alteração
                Console.WriteLine();
                Console.WriteLine($"Documento: {novoCpfCnpj}");
                Console.WriteLine($"Tipo: {(ehCpf ? "CPF" : "CNPJ")}");
                Console.WriteLine($"Razão Social será: {(ehCpf ? "NULL (removida)" : "mantida/copiada do nome")}");
                Console.WriteLine();
                Console.Write("Confirma a alteração? (S/N): ");

                string confirmacao = Console.ReadLine();
                if (confirmacao?.ToUpper() != "S")
                {
                    Console.WriteLine("Operação cancelada!");
                    return;
                }

                // Executar alteração
                AtualizarEmpresa(idEmpresa, novoCpfCnpj, ehCpf);

                // Executar script da base centralizada
                AtualizarBaseCentralizada();

                Console.WriteLine();
                Console.WriteLine("Alteração realizada com sucesso!");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }

            Console.WriteLine();
            Console.WriteLine("Pressione qualquer tecla para sair...");
            Console.ReadKey();
        }

        static string ListarEmpresas()
        {
            Console.WriteLine("=== Empresas Cadastradas ===");

            string id = null;

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string sql = @"
                    SELECT id_dados_empresa, nome, razao_social, cpf_cnpj 
                    FROM dados_empresa 
                    WHERE data_hora_deletado IS NULL
                    ORDER BY nome";

                using (var command = new SQLiteCommand(sql, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        id = reader["id_dados_empresa"].ToString();
                        string nome = reader["nome"].ToString();
                        string razaoSocial = reader["razao_social"].ToString();
                        string cpfCnpj = reader["cpf_cnpj"].ToString();

                        Console.WriteLine($"ID: {id}");
                        Console.WriteLine($"Nome: {nome}");
                        Console.WriteLine($"Razão Social: {razaoSocial ?? "N/A"}");
                        Console.WriteLine($"CPF/CNPJ: {cpfCnpj}");
                        Console.WriteLine("---");
                    }
                }
            }

            return id;
        }

        static bool VerificarEmpresaExiste(string idEmpresa)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string sql = "SELECT COUNT(*) FROM dados_empresa WHERE id_dados_empresa = @id AND data_hora_deletado IS NULL";

                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", idEmpresa);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        static void AtualizarEmpresa(string idEmpresa, string novoCpfCnpj, bool ehCpf)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string sql;

                if (ehCpf)
                {
                    // CPF: razao_social = NULL
                    sql = @"
                        UPDATE dados_empresa 
                        SET cpf_cnpj_antigo = cpf_cnpj,
                            cpf_cnpj = @novoCpfCnpj,
                            razao_social = NULL,
                            data_hora_ultima_alteracao = @dataHora
                        WHERE id_dados_empresa = @id";
                }
                else
                {
                    // CNPJ: razao_social = nome (se razao_social estiver NULL)
                    sql = @"
                        UPDATE dados_empresa 
                        SET cpf_cnpj_antigo = cpf_cnpj,
                            cpf_cnpj = @novoCpfCnpj,
                            razao_social = CASE 
                                WHEN razao_social IS NULL THEN nome 
                                ELSE razao_social 
                            END,
                            data_hora_ultima_alteracao = @dataHora
                        WHERE id_dados_empresa = @id";
                }

                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", idEmpresa);
                    command.Parameters.AddWithValue("@novoCpfCnpj", novoCpfCnpj);
                    command.Parameters.AddWithValue("@dataHora", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        throw new Exception("Nenhuma linha foi alterada!");
                    }
                }
            }
        }

        static void AtualizarBaseCentralizada()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string sql = @"
                    UPDATE base_centralizada
                    SET id_dispositivo = '00000000-0000-0000-0000-000000000000',
                        id_empresa = NULL,
                        id_usuario = NULL,
                        codigo_dispositivo = 1,
                        ultimo_id_sincronizado = 0,
                        data_ultima_sincronizacao = NULL,
                        ativada = 0,
                        carga_inicial = 0,
                        sincronizando = 0,
                        tabelas_sincronizadas = NULL,
                        mensagem_erro = NULL";

                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        static bool ValidarCpf(string cpf)
        {
            if (string.IsNullOrEmpty(cpf) || cpf.Length != 11)
                return false;

            // Verifica se todos os dígitos são iguais
            bool todosIguais = true;
            for (int i = 1; i < cpf.Length; i++)
            {
                if (cpf[i] != cpf[0])
                {
                    todosIguais = false;
                    break;
                }
            }

            if (todosIguais)
                return false;

            // Calcula o primeiro dígito verificador
            int soma = 0;
            for (int i = 0; i < 9; i++)
            {
                soma += int.Parse(cpf[i].ToString()) * (10 - i);
            }

            int resto = soma % 11;
            int dv1 = resto < 2 ? 0 : 11 - resto;

            if (int.Parse(cpf[9].ToString()) != dv1)
                return false;

            // Calcula o segundo dígito verificador
            soma = 0;
            for (int i = 0; i < 10; i++)
            {
                soma += int.Parse(cpf[i].ToString()) * (11 - i);
            }

            resto = soma % 11;
            int dv2 = resto < 2 ? 0 : 11 - resto;

            return int.Parse(cpf[10].ToString()) == dv2;
        }

        static bool ValidarCnpj(string cnpj)
        {
            if (string.IsNullOrEmpty(cnpj) || cnpj.Length != 14)
                return false;

            // Verifica se todos os dígitos são iguais
            bool todosIguais = true;
            for (int i = 1; i < cnpj.Length; i++)
            {
                if (cnpj[i] != cnpj[0])
                {
                    todosIguais = false;
                    break;
                }
            }

            if (todosIguais)
                return false;

            // Calcula o primeiro dígito verificador
            int[] multiplicador1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma = 0;

            for (int i = 0; i < 12; i++)
            {
                soma += int.Parse(cnpj[i].ToString()) * multiplicador1[i];
            }

            int resto = soma % 11;
            int dv1 = resto < 2 ? 0 : 11 - resto;

            if (int.Parse(cnpj[12].ToString()) != dv1)
                return false;

            // Calcula o segundo dígito verificador
            int[] multiplicador2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            soma = 0;

            for (int i = 0; i < 13; i++)
            {
                soma += int.Parse(cnpj[i].ToString()) * multiplicador2[i];
            }

            resto = soma % 11;
            int dv2 = resto < 2 ? 0 : 11 - resto;

            return int.Parse(cnpj[13].ToString()) == dv2;
        }
    }
}
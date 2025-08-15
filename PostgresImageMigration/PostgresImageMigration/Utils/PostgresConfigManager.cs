// Utils/PostgresConfigManager.cs
using System;
using System.Configuration;
using Npgsql;
using PostgresImageMigration.Utils;

namespace PostgresImageMigration.Utils
{
    /// <summary>
    /// Gerencia a configuração de performance do PostgreSQL durante a migração.
    /// As alterações são aplicadas apenas à sessão atual.
    /// </summary>
    public static class PostgresConfigManager
    {
        private static readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;

        /// <summary>
        /// Prepara o banco para inserção massiva.
        /// As configurações afetam apenas a sessão da conexão.
        /// </summary>
        public static void PrepararParaCarga()
        {
            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    conn.Open();

                    // Modo turbo — sessão atual
                    ExecuteNonQuery(conn, "SET synchronous_commit = OFF;");
                    ExecuteNonQuery(conn, "SET fsync = OFF;");
                    ExecuteNonQuery(conn, "SET full_page_writes = OFF;");

                    Logger.Log("Configurações de alta performance aplicadas para a sessão.");
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Erro ao preparar configuração: {ex.Message}");
            }
        }

        /// <summary>
        /// Não é necessário restaurar configurações — sessão se encerra com a conexão.
        /// Este método existe apenas para manter a assinatura e permitir logs.
        /// </summary>
        public static void RestaurarConfiguracoes()
        {
            Logger.Log("Nenhuma restauração necessária — configurações eram apenas da sessão.");
        }

        private static void ExecuteNonQuery(NpgsqlConnection conn, string sql)
        {
            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}

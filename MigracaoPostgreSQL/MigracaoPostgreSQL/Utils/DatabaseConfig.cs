using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigracaoPostgreSQL.Utils
{
    /// <summary>
    /// Configurações de conexão com os bancos de dados
    /// </summary>
    public class DatabaseConfig
    {
        private const string HOST = "127.0.0.1";
        private const string PORT = "5434";
        private const string USERNAME = "postgres";
        private const string PASSWORD = "cer_2011!";

        private readonly Dictionary<string, string> _databases = new Dictionary<string, string>
        {
            { "tbl_autopecas_postgresql", "tbl_autopecas_postgresql" },
            { "tbl_construcao_2025_postgresql", "tbl_construcao_2025_postgresql" },
            { "tbl_eans_1555_postgresql", "tbl_eans_1555_postgresql" },
            { "tbl_petshop_ecommerce_2025_postgresql", "tbl_petshop_ecommerce_2025_postgresql" }
        };

        /// <summary>
        /// Obtém a string de conexão para o banco de destino
        /// </summary>
        public string GetDestinationConnectionString()
        {
            string HostDestino = "127.0.0.1";
            string portaDestino = "5432";
            string usuarioDestino = "postgres";
            string senhaDestino = "cer_2011!";

            return $"Host={HostDestino};Port={portaDestino};Database=consulta_produtos;Username={usuarioDestino};Password={senhaDestino};Pooling=true;Minimum Pool Size=1;Maximum Pool Size=10;Connection Idle Lifetime=30;";
        }

        /// <summary>
        /// Obtém a string de conexão para um banco de origem específico
        /// </summary>
        public string GetOriginConnectionString(string databaseKey)
        {
            if (_databases.ContainsKey(databaseKey))
            {
                return BuildConnectionString(_databases[databaseKey]);
            }

            throw new System.ArgumentException($"Banco de dados não encontrado: {databaseKey}");
        }

        /// <summary>
        /// Constrói a string de conexão com base no nome do banco
        /// </summary>
        private string BuildConnectionString(string databaseName)
        {
            return $"Host={HOST};Port={PORT};Database={databaseName};Username={USERNAME};Password={PASSWORD};Pooling=true;Minimum Pool Size=1;Maximum Pool Size=10;Connection Idle Lifetime=30;";
        }

        /// <summary>
        /// Obtém lista de todos os bancos de origem configurados
        /// </summary>
        public Dictionary<string, string> GetOriginDatabases()
        {
            return new Dictionary<string, string>(_databases);
        }
    }
}

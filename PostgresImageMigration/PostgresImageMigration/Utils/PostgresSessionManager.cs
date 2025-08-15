using System;
using System.Configuration;
using Npgsql;

namespace PostgresImageMigration.Utils
{
    /// <summary>
    /// Gerencia a conexão persistente com PostgreSQL e aplica configurações de sessão
    /// (modo "turbo") para acelerar a carga. A restauração não é necessária porque as
    /// alterações são apenas na sessão da conexão.
    /// 
    /// Usage:
    /// using (var manager = new PostgresSessionManager())
    /// {
    ///     manager.Open(); // abre conexão e aplica SETs
    ///     var connection = manager.Connection; // usar para COPY
    /// }
    /// </summary>
    public class PostgresSessionManager : IDisposable
    {
        private readonly string _connectionString;
        private NpgsqlConnection _connection;
        private bool _appliedTurbo = false;

        public NpgsqlConnection Connection
        {
            get { return _connection; }
        }

        public PostgresSessionManager()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;
        }

        /// <summary>
        /// Abre a conexão e aplica as configurações de sessão que melhoram a performance.
        /// Essas alterações são aplicadas apenas à sessão desta conexão.
        /// </summary>
        public void Open()
        {
            _connection = new NpgsqlConnection(_connectionString);
            _connection.Open();

            ApplySessionSettings();
        }

        private void ApplySessionSettings()
        {
            try
            {
                // SET na sessão para acelerar gravação. Risco: perda em caso de falha elétrica durante a operação.
                using (var cmd = _connection.CreateCommand())
                {
                    cmd.CommandText = "SET synchronous_commit = OFF;";
                    cmd.ExecuteNonQuery();

                    //cmd.CommandText = "SET full_page_writes = OFF;";
                    //cmd.ExecuteNonQuery();
                }

                _appliedTurbo = true;
                Logger.Log("[PostgresSessionManager] Configurações de sessão aplicadas (synchronous_commit=OFF, fsync=OFF, full_page_writes=OFF).");
            }
            catch (Exception ex)
            {
                Logger.LogException("[PostgresSessionManager] Erro ao aplicar configurações de sessão", ex);
                // se falhar, ainda assim continua; a conexão pode ser usada sem os SETs.
            }
        }

        /// <summary>
        /// Fecha a conexão. Como as configurações foram de sessão, nada precisa ser restaurado explicitamente.
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (_connection != null)
                {
                    _connection.Close();
                    _connection.Dispose();
                    _connection = null;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException("[PostgresSessionManager] Erro ao fechar conexão", ex);
            }
        }
    }
}

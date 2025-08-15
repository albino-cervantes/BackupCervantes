using MigracaoPostgreSQL.Utils;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigracaoPostgreSQL.Repositories
{
    /// <summary>
    /// Classe base para repositórios com funcionalidades comuns
    /// </summary>
    public abstract class BaseRepository
    {
        protected readonly string _connectionString;
        protected readonly Logger _logger;

        protected BaseRepository(string connectionString, Logger logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        /// <summary>
        /// Cria uma nova conexão com o banco de dados
        /// </summary>
        protected NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }

        /// <summary>
        /// Executa uma operação com transação e tratamento de erro
        /// </summary>
        protected async Task<bool> ExecuteWithTransactionAsync(Func<NpgsqlConnection, NpgsqlTransaction, Task<bool>> operation)
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    await connection.OpenAsync();
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            var result = await operation(connection, transaction);
                            if (result)
                            {
                                await transaction.CommitAsync();
                                return true;
                            }
                            else
                            {
                                await transaction.RollbackAsync();
                                return false;
                            }
                        }
                        catch
                        {
                            await transaction.RollbackAsync();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro na operação com transação: {ex.Message}", ex);
                return false;
            }
        }
    }
}

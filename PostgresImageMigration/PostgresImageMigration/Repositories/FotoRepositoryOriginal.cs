// Repositories/FotoRepository.cs
using System.Collections.Generic;
using System.Data;
using Npgsql;
using PostgresImageMigration.Models;
using System.Configuration;
using PostgresImageMigration.Utils;

namespace PostgresImageMigration.Repositories
{
    /// <summary>
    /// Implementa a persistência das fotos no PostgreSQL.
    /// </summary>
    public class FotoRepositoryOriginal : IFotoRepositoryOriginal
    {
        private readonly string _connectionString;

        public FotoRepositoryOriginal()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;
        }

        /// <summary>
        /// Insere as fotos no banco em lotes para maior performance.
        /// </summary>
        public void InserirEmLote(IEnumerable<Foto> fotos)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();

                using (var trans = conn.BeginTransaction())
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.Transaction = trans;

                    cmd.CommandText = "INSERT INTO public.fotos_a_migrar (identificacao, foto) VALUES (@id, @foto) ON CONFLICT (identificacao) DO NOTHING";
                    cmd.Parameters.Add("@id", NpgsqlTypes.NpgsqlDbType.Varchar);
                    cmd.Parameters.Add("@foto", NpgsqlTypes.NpgsqlDbType.Bytea);

                    int count = 0;
                    foreach (var foto in fotos)
                    {
                        cmd.Parameters["@id"].Value = foto.Identificacao;
                        cmd.Parameters["@foto"].Value = foto.Conteudo;
                        cmd.ExecuteNonQuery();
                        count++;
                    }

                    trans.Commit();
                    Logger.Log($"{count} imagens inseridas no banco.");
                }
            }
        }
    }
}

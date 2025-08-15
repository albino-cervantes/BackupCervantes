// Repositories/FotoRepository.cs
using System.Collections.Generic;
using System.Configuration;
using Npgsql;
using PostgresImageMigration.Models;
using PostgresImageMigration.Utils;

namespace PostgresImageMigration.Repositories
{
    /// <summary>
    /// Implementa a persistência das fotos no PostgreSQL usando COPY BINARY para alta performance.
    /// </summary>
    public class FotoRepositoryBinary : IFotoRepositoryOriginal
    {
        private readonly string _connectionString;

        public FotoRepositoryBinary()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;
        }

        /// <summary>
        /// Insere fotos em lote usando COPY BINARY, extremamente rápido.
        /// </summary>
        public void InserirEmLote(IEnumerable<Foto> fotos)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();

                // COPY BINARY direto para a tabela
                using (var writer = conn.BeginBinaryImport("COPY public.fotos_a_migrar (identificacao, foto) FROM STDIN (FORMAT BINARY)"))
                {
                    int count = 0;

                    foreach (var foto in fotos)
                    {
                        writer.StartRow();
                        writer.Write(foto.Identificacao, NpgsqlTypes.NpgsqlDbType.Varchar);
                        writer.Write(foto.Conteudo, NpgsqlTypes.NpgsqlDbType.Bytea);
                        count++;
                    }

                    writer.Complete();
                    Logger.Log($"{count} imagens inseridas no banco via COPY.");
                }
            }
        }
    }
}

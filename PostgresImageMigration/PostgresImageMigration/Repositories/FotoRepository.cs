using System;
using System.Collections.Generic;
using Npgsql;
using NpgsqlTypes;
using PostgresImageMigration.Models;
using PostgresImageMigration.Utils;

namespace PostgresImageMigration.Repositories
{
    /// <summary>
    /// Implementação que realiza a importação via COPY BINARY.
    /// Observação: essa classe recebe uma conexão Npgsql já aberta e se responsabiliza apenas
    /// por executar o BeginBinaryImport nessa conexão (não abre/fecha conexão).
    /// </summary>
    public class FotoRepository : IFotoRepository
    {
        private readonly NpgsqlConnection _connection;

        /// <summary>
        /// Construtor recebe a conexão já aberta (para manter sessão com SET aplicado).
        /// </summary>
        /// <param name="connection">Conexão Npgsql aberta e administrada externamente.</param>
        public FotoRepository(NpgsqlConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException("connection");
        }

        /// <summary>
        /// Insere o lote via COPY BINARY usando a conexão existente.
        /// Implementação robusta — em caso de erro, a exceção sobe para o chamador.
        /// </summary>
        public void InserirEmLote(IEnumerable<Foto> lote)
        {
            if (lote == null)
                throw new ArgumentNullException("lote");

            int count = 0;

            // O BeginBinaryImport precisa de uma conexão aberta.
            using (var writer = _connection.BeginBinaryImport("COPY public.fotos_a_migrar (identificacao, foto) FROM STDIN (FORMAT BINARY)"))
            {
                foreach (var foto in lote)
                {
                    // Cada StartRow + Write escreve uma linha no formato COPY BINARY
                    writer.StartRow();
                    // identificacao como varchar/text
                    writer.Write(foto.Identificacao ?? string.Empty, NpgsqlDbType.Varchar);
                    // foto como bytea
                    writer.Write(foto.Conteudo ?? new byte[0], NpgsqlDbType.Bytea);
                    count++;
                }
                writer.Complete();
            }

            Logger.Log($"[Repository] Inseridos {count} registros via COPY.");
        }
    }
}

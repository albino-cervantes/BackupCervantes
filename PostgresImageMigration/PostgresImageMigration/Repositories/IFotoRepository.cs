using System.Collections.Generic;
using PostgresImageMigration.Models;

namespace PostgresImageMigration.Repositories
{
    /// <summary>
    /// Interface do repositório de fotos.
    /// </summary>
    public interface IFotoRepository
    {
        /// <summary>
        /// Insere um lote de fotos usando COPY BINARY. Expectativa: chamada pelo writer que
        /// detém a conexão aberta.
        /// </summary>
        void InserirEmLote(IEnumerable<Foto> lote);
    }
}

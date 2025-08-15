// Repositories/IFotoRepository.cs
using System.Collections.Generic;
using PostgresImageMigration.Models;

namespace PostgresImageMigration.Repositories
{
    /// <summary>
    /// Interface para repositório de fotos.
    /// Define operações para inserção no banco PostgreSQL.
    /// </summary>
    public interface IFotoRepositoryOriginal
    {
        void InserirEmLote(IEnumerable<Foto> fotos);
    }
}

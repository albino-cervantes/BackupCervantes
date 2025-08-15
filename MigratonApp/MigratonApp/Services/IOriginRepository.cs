using System.Collections.Generic;
using MigrationApp.Models;

namespace MigrationApp.Services
{
    public interface IOriginRepository
    {
        string Name { get; }
        IEnumerable<List<ProdutoImportacao>> GetBatches(int batchSize);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigracaoPostgreSQL.Services
{
    /// <summary>
    /// Classe para armazenar estatísticas da migração
    /// </summary>
    public class MigrationStatistics
    {
        public int ProductsCreated { get; set; }
        public int ProductsUpdated { get; set; }
        public int ProductsSkipped { get; set; }
        public int ProductsWithError { get; set; }
        public int BatchesWithError { get; set; }
        public List<string> TablesWithErrors { get; set; } = new List<string>();
    }
}

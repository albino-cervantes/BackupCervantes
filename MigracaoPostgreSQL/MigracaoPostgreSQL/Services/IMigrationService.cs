using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigracaoPostgreSQL.Services
{
    /// <summary>
    /// Interface para serviço de migração
    /// </summary>
    public interface IMigrationService
    {
        Task ExecuteMigrationAsync();
    }
}

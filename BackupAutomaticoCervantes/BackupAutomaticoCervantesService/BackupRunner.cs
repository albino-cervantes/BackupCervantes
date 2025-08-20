using BackupAutomaticoCervantes;
using BackupAutomaticoCervantes.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupAutomaticoCervantesService
{
    public static class BackupRunner
    {
        public static void Run(ParametrosBackupModel p)
        {
            // Chama seu método que executa o pg_dump + destinos
            var manager = new BackupManager();
            manager.ExecuteBackup(p);
        }
    }
}

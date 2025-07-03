using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupAutomaticoCervantes.DestinoBackup.GoogleDrive
{
    /// <summary>
    /// Configuração específica para backup no Google Drive.
    /// </summary>
    public class GoogleDriveConfig : IDestinoConfig
    {
        public string CredenciaisJsonPath { get; set; }

        public Guid Id { get; set; } = Guid.NewGuid();

        public DestinoTipo Tipo => DestinoTipo.GoogleDrive;
    }
}

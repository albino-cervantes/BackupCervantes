using Amazon.Util;
using BackupAutomaticoCervantes.DestinoBackup.Amazon;
using BackupAutomaticoCervantes.DestinoBackup.Ftp;
using BackupAutomaticoCervantes.DestinoBackup.GoogleDrive;
using BackupAutomaticoCervantes.DestinoBackup.OneDrive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupAutomaticoCervantes.DestinoBackup
{
    public class DestinoConfig
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DestinoTipo Tipo { get; set; }

        // Sub-configs
        public FtpConfig Ftp { get; set; }
        public GoogleDriveConfig GoogleDrive { get; set; }
        public S3Config S3 { get; set; }
        public OneDriveConfig OneDrive { get; set; }
    }
}

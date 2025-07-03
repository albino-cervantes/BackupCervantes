using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupAutomaticoCervantes.DestinoBackup.OneDrive
{
    public class OneDriveConfig: IDestinoConfig
    {
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public Guid Id { get; set; } = Guid.NewGuid();

        public DestinoTipo Tipo => DestinoTipo.OneDrive;
    }
}

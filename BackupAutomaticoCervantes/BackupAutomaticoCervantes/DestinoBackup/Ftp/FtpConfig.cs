using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupAutomaticoCervantes.DestinoBackup.Ftp
{
    public class FtpConfig : IDestinoConfig
    {
        public string Url { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }

        public Guid Id { get; set; } = Guid.NewGuid();

        public DestinoTipo Tipo => DestinoTipo.Ftp;
    }
}

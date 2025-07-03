using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupAutomaticoCervantes.DestinoBackup.Amazon
{
    public class S3Config : IDestinoConfig
    {
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string BucketName { get; set; }
        public string Region { get; set; }

        public Guid Id { get; set; } = Guid.NewGuid();

        public DestinoTipo Tipo => DestinoTipo.S3;
    }
}

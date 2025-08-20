using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupCervantes2.Models
{
    // Models/UnifiedConfig.cs (apenas o StorageConfig mostrado aqui — mantenha o resto do modelo)
    public class StorageConfig
    {
        public string Type { get; set; } = "Local"; // "Local", "S3", "GDrive", "GDriveUser"

        // S3
        public string AwsAccessKey { get; set; }
        public string AwsSecretKey { get; set; }
        public string AwsRegion { get; set; }
        public string AwsBucket { get; set; }
        public string AwsPrefix { get; set; } = "backups";

        // Service Account (legacy)
        public string GDriveCredentialsFile { get; set; } = "secrets/gdrive-sa.json";
        public string GDriveParentFolderId { get; set; }
        public string GDrivePrefix { get; set; } = "backups";

        // USER OAuth2 (novo)
        // client_secrets.json (OAuth client credentials) - usado apenas para o fluxo de login do usuário
        public string GDriveClientSecretsFile { get; set; } = "secrets/gdrive-client.json";
        // pasta que receberá tokens (arquivo com refresh token) - relativo ao diretório do exe
        public string GDriveTokenFolder { get; set; } = "token";
        // parent folder id (onde gravar)
        public string GDriveUserParentFolderId { get; set; }
        public string GDriveUserPrefix { get; set; } = "backups";
    }

}

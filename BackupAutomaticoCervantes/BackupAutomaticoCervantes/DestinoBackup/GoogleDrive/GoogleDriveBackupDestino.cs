using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BackupAutomaticoCervantes.DestinoBackup.GoogleDrive
{
    internal class GoogleDriveBackupDestino : IBackupDestino
    {
        private readonly GoogleDriveConfig _config;

        /// <summary>
        /// Inicializa o destino de backup Google Drive com a configuração específica.
        /// </summary>
        /// <param name="config">Configuração do Google Drive</param>
        public GoogleDriveBackupDestino(GoogleDriveConfig config)
        {
            _config = config;
        }

        public async Task EnviarBackupAsync(string caminhoArquivoLocal)
        {
            
            // 1) Obtém o DriveService já autenticado (token do usuário)  
            DriveService service = await GoogleDriveAuthManager.AutenticarAsync(_config.Id);

            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = Path.GetFileName(caminhoArquivoLocal)
            };

            var stream = new FileStream(caminhoArquivoLocal, FileMode.Open, FileAccess.Read);
            var request = service.Files.Create(fileMetadata, stream, "application/octet-stream");
            request.Fields = "id";
            var file = await request.UploadAsync();

            if (file.Status == Google.Apis.Upload.UploadStatus.Completed)
            {
                Console.WriteLine("Arquivo enviado com sucesso.");
            }
            else
            {
                Console.WriteLine("Erro ao enviar arquivo.");
            }
        }
    }
}

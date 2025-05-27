using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BackupAutomaticoCervantes.DestinoBackup.OneDrive
{
    internal class OneDriveBackupDestino : IBackupDestino
    {
        public async Task EnviarBackupAsync(string caminhoArquivoLocal)
        {
            throw new NotImplementedException();
        }
    }
    
}

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
    public class GoogleDriveAuthManager
    {
        private const string CredentialFileRelative = "ConfigAutenticacaoGoogleDrive.json";
        private const string TokenStoreBase = "GoogleDriveTokenStore";

        /// <summary>
        /// Autentica um usuário do Google Drive, isolando o token em uma pasta específica por destinoId.
        /// </summary>
        public static async Task<DriveService> AutenticarAsync(Guid destinoId)
        {
            // 1) Caminho completo do JSON de credenciais
            string credPath = Path.Combine(
                $"{AppDomain.CurrentDomain.BaseDirectory}Padrao\\",
                CredentialFileRelative);

            if (!File.Exists(credPath))
                throw new FileNotFoundException("Credenciais do Google Drive não encontradas.", credPath);

            // 2) Cria um DataStore único para este destino
            string userId = destinoId.ToString();
            string tokenDir = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                TokenStoreBase,
                userId);

            // 3) Lê o JSON de credenciais e inicia o fluxo OAuth
            var stream = new FileStream(credPath, FileMode.Open, FileAccess.Read);

            var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.FromStream(stream).Secrets,
                new[] { DriveService.Scope.DriveFile },
                userId,                        // o nome do usuário pro token store
                CancellationToken.None,
                new FileDataStore(tokenDir, true)).ConfigureAwait(false);

            // 4) Cria o serviço
            return new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "BackupAutomaticoCervantes"
            });
        }
    }
}

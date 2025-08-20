using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace BackupCervantes2.Storage
{
    /// <summary>
    /// Provider que utiliza OAuth2 (user consent). Na primeira execução abrirá browser para logar.
    /// Armazena tokens em uma pasta (GDriveTokenFolder) para uso futuro (refresh token).
    /// </summary>
    public class GoogleDriveUserStorageProvider : IStorageProvider
    {
        private readonly DriveService _service;
        private readonly string _parentFolderId;
        private readonly string _prefix;
        private readonly string _tokenFolder;

        /// <summary>
        /// clientSecretsJsonPath: caminho para client_secrets.json (OAuth client credentials).
        /// parentFolderId: ID da pasta no Drive onde os arquivos serão enviados.
        /// prefix: prefixo/pasta virtual dentro do parent (opcional).
        /// tokenFolder: pasta onde será salvo o token (relativa ao diretório do exe).
        /// </summary>
        public GoogleDriveUserStorageProvider(string clientSecretsJsonPath, string parentFolderId, string prefix = "", string tokenFolder = "token", string nomeSistema = "")
        {
            if (string.IsNullOrWhiteSpace(clientSecretsJsonPath) || !File.Exists(clientSecretsJsonPath))
                throw new FileNotFoundException("client_secrets.json não encontrado: " + clientSecretsJsonPath);

            _parentFolderId = parentFolderId ?? throw new ArgumentNullException(nameof(parentFolderId));
            _prefix = prefix ?? "";
            _tokenFolder = tokenFolder ?? "token";

            Logger.Info($"Inicializando GoogleDriveUserStorageProvider. clientSecrets={clientSecretsJsonPath}, parentId={_parentFolderId}, tokenFolder={_tokenFolder}", nomeSistema);

            var credential = AuthorizeUserAsync(clientSecretsJsonPath, _tokenFolder, nomeSistema).GetAwaiter().GetResult();

            _service = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "BackupUnified-GDriveUser"
            });
        }

        private static async Task<UserCredential> AuthorizeUserAsync(string clientSecretsJsonPath, string tokenFolder, string nomeSistema)
        {
            using (var stream = new FileStream(clientSecretsJsonPath, FileMode.Open, FileAccess.Read))
            {
                var secrets = GoogleClientSecrets.Load(stream).Secrets;
                var scopes = new[] { DriveService.Scope.Drive }; // escopo amplo (gravação + leitura)

                // tokenFolder será criado dentro do diretório do exe
                var credPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, tokenFolder);
                Logger.Info($"Iniciando fluxo OAuth2 do usuário. Tokens serão salvos em: {credPath}", nomeSistema);

                var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    secrets,
                    scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)
                );

                Logger.Info("Autorização do usuário concluída com sucesso.", nomeSistema);
                return credential;
            }
        }

        public async Task UploadFileAsync(string localPath, string remotePath, string nomeSistema)
        {
            if (!File.Exists(localPath))
                throw new FileNotFoundException("Arquivo fonte não encontrado: " + localPath);

            // remotePath pode ser "subpasta/arquivo.zip" - manter só filename para metadata.Name
            var fileName = Path.GetFileName(remotePath);
            var metadata = new Google.Apis.Drive.v3.Data.File
            {
                Name = fileName,
                Parents = new[] { _parentFolderId }
            };

            Logger.Info($"[GDriveUser] Upload start: {localPath} -> {fileName}", nomeSistema);
            using (var fs = new FileStream(localPath, FileMode.Open, FileAccess.Read))
            {
                var req = _service.Files.Create(metadata, fs, "application/zip");
                req.Fields = "id";
                req.SupportsAllDrives = true;

                var result = await req.UploadAsync();
                if (result.Status != Google.Apis.Upload.UploadStatus.Completed)
                {
                    var msg = result.Exception?.Message ?? "Upload failed (unknown)";
                    Logger.Error($"[GDriveUser] Upload failed: {msg}", nomeSistema);
                    throw new Exception("Falha no upload GDrive (user): " + msg);
                }

                Logger.Info($"[GDriveUser] Upload completed. fileId={req.Body?.Name}", nomeSistema);
            }
        }

        public void Dispose()
        {
            _service?.Dispose();
        }
    }
}

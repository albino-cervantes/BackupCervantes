using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupCervantes2.Storage
{
    public class LocalStorageProvider : IStorageProvider
    {
        private readonly string _baseFolder;

        public LocalStorageProvider(string baseFolder)
        {
            _baseFolder = baseFolder ?? throw new ArgumentNullException(nameof(baseFolder));
            if (!Directory.Exists(_baseFolder)) Directory.CreateDirectory(_baseFolder);
        }

        public async Task UploadFileAsync(string localPath, string remotePath, string nomeSistema)
        {
            var destPath = Path.Combine(_baseFolder, remotePath.Replace('/', Path.DirectorySeparatorChar));
            var destDir = Path.GetDirectoryName(destPath);
            if (!Directory.Exists(destDir)) Directory.CreateDirectory(destDir);

            Logger.Info($"[Local] Copiando {localPath} → {destPath}", nomeSistema);
            using (var src = new FileStream(localPath, FileMode.Open, FileAccess.Read, FileShare.Read, 1024 * 128, true))
            using (var dst = new FileStream(destPath, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 128, true))
            {
                await src.CopyToAsync(dst);
            }
        }

        public void Dispose() { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupCervantes2.Storage
{
    public interface IStorageProvider : IDisposable
    {
        Task UploadFileAsync(string localPath, string remotePath, string nomeSistema);
    }
}

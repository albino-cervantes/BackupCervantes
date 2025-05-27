using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BackupAutomaticoCervantes.DestinoBackup.Ftp
{
    internal class FtpBackupDestino : IBackupDestino
    {
        private readonly string _ftpUrl = "ftp://seuservidor.com/backup/";
        private readonly string _usuario = "usuario";
        private readonly string _senha = "senha";

        public async Task EnviarBackupAsync(string caminhoArquivoLocal)
        {
            var request = (FtpWebRequest)WebRequest.Create(_ftpUrl + Path.GetFileName(caminhoArquivoLocal));
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(_usuario, _senha);

            byte[] fileContents;
            using (var fileStream = new FileStream(caminhoArquivoLocal, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                fileContents = new byte[fileStream.Length];
                await fileStream.ReadAsync(fileContents, 0, (int)fileStream.Length);
            }

            request.ContentLength = fileContents.Length;

            using (var requestStream = await request.GetRequestStreamAsync())
            {
                await requestStream.WriteAsync(fileContents, 0, fileContents.Length);
            }
        }
    }
}

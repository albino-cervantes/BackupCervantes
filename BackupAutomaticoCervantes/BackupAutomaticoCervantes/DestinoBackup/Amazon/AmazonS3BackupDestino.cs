using Amazon.S3.Model;
using Amazon.S3;
using Amazon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupAutomaticoCervantes.DestinoBackup.Amazon
{
    internal class AmazonS3BackupDestino : IBackupDestino
    {
        public async Task EnviarBackupAsync(string caminhoArquivoLocal)
        {
            var client = new AmazonS3Client("accessKey", "secretKey", RegionEndpoint.USEast1);
            var request = new PutObjectRequest
            {
                BucketName = "meu-bucket",
                Key = Path.GetFileName(caminhoArquivoLocal),
                FilePath = caminhoArquivoLocal
            };

            await client.PutObjectAsync(request);
        }
    }
  
}

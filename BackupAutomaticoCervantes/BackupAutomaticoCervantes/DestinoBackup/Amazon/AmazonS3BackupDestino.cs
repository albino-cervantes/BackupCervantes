using Amazon.S3.Model;
using Amazon.S3;
using Amazon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace BackupAutomaticoCervantes.DestinoBackup.Amazon
{
    public class AmazonS3BackupDestino : IBackupDestino
    {
        private readonly IAmazonS3 _s3Client;
        private readonly S3Config _config;
        private readonly ILogger<AmazonS3BackupDestino> _logger;

        public async Task EnviarBackupAsync(string caminhoArquivoLocal)
        {
            try
            {
                var fileInfo = new FileInfo(caminhoArquivoLocal);
                var key = $"backups/{DateTime.UtcNow:yyyy/MM/dd}/{fileInfo.Name}";

                var request = new PutObjectRequest
                {
                    BucketName = _config.BucketName,
                    Key = key,
                    FilePath = caminhoArquivoLocal,
                    ServerSideEncryptionMethod = ServerSideEncryptionMethod.AES256,
                    StorageClass = S3StorageClass.StandardInfrequentAccess
                };

                var response = await _s3Client.PutObjectAsync(request);
                _logger.LogInformation("Backup enviado para S3: {Key}", key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao enviar backup para S3");
                throw;
            }
        }
    }

}


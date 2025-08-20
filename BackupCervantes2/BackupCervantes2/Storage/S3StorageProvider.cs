using Amazon.S3;
using Amazon.S3.Transfer;
using System.Threading.Tasks;

namespace BackupCervantes2.Storage
{
    public class S3StorageProvider : IStorageProvider
    {
        private readonly AmazonS3Client _client;
        private readonly string _bucket;
        private readonly string _prefix;

        public S3StorageProvider(string accessKey, string secretKey, string region, string bucket, string prefix)
        {
            _client = new AmazonS3Client(accessKey, secretKey, Amazon.RegionEndpoint.GetBySystemName(region));
            _bucket = bucket;
            _prefix = prefix ?? "";
        }

        public async Task UploadFileAsync(string localPath, string remotePath, string nomeSistema)
        {
            var key = string.IsNullOrWhiteSpace(_prefix) ? remotePath : $"{_prefix}/{remotePath}";

            Logger.Info($"[S3] Upload {localPath} → s3://{_bucket}/{key}", nomeSistema);
            var fileTransferUtility = new TransferUtility(_client);
            await fileTransferUtility.UploadAsync(localPath, _bucket, key);
        }

        public void Dispose() => _client?.Dispose();
    }
}

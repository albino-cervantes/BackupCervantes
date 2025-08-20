using BackupCervantes2.Models;
using BackupCervantes2.Storage;
using System;


namespace BackupCervantes2.Services
{
    public static class StorageFactory
    {
        public static IStorageProvider CreateForSystem(UnifiedConfig config, SystemConfig system)
        {
            var type = (config.Global.Storage?.Type ?? "Local").Trim().ToLowerInvariant();

            switch (type)
            {
                case "s3":
                    var sc = config.Global.Storage;
                    return new S3StorageProvider(
                        sc.AwsAccessKey,
                        sc.AwsSecretKey,
                        sc.AwsRegion,
                        sc.AwsBucket,
                        sc.AwsPrefix);

                case "gdrive":
                    // legacy: service account approach (mantido se ainda usado)
                    var gsc = config.Global.Storage;
                    var creds = ResolvePath(gsc.GDriveCredentialsFile);
                    return new GoogleDriveUserStorageProvider(creds, gsc.GDriveParentFolderId, gsc.GDrivePrefix, nomeSistema: system.Name);

                case "gdriveuser":
                    // NEW: OAuth user flow
                    var ug = config.Global.Storage;
                    var clientSecrets = ResolvePath(ug.GDriveClientSecretsFile);
                    var tokenFolder = string.IsNullOrWhiteSpace(ug.GDriveTokenFolder) ? "token" : ug.GDriveTokenFolder;
                    var parentFolder = ug.GDriveUserParentFolderId ?? ug.GDriveParentFolderId;
                    if (string.IsNullOrWhiteSpace(parentFolder))
                        throw new ArgumentException("GDriveUserParentFolderId (ou GDriveParentFolderId) deve estar configurado em Global.Storage.");

                    return new GoogleDriveUserStorageProvider(clientSecrets, parentFolder, ug.GDriveUserPrefix ?? ug.GDrivePrefix, tokenFolder, system.Name);

                case "local":
                default:
                    // usa path definido no system (cada sistema tem PathBackupDestinoDrive)
                    var dest = system.PathBackupDestinoDrive;
                    if (string.IsNullOrWhiteSpace(dest))
                        throw new ArgumentException($"PathBackupDestinoDrive não configurado para o sistema {system.Name}");
                    return new LocalStorageProvider(dest);
            }
        }

        private static string ResolvePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return path;
            return System.IO.Path.IsPathRooted(path)
                ? path
                : System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
        }
    }
}


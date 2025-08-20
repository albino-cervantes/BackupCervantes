using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace BackupCervantes2.Services
{
    public static class ZipUtils
    {
        public static CompressionLevel ParseLevel(string level)
        {
            if (string.IsNullOrWhiteSpace(level)) return CompressionLevel.Optimal;
            switch (level.Trim().ToLower())
            {
                case "fastest": return CompressionLevel.Fastest;
                case "nocompression": return CompressionLevel.NoCompression;
                default: return CompressionLevel.Optimal;
            }
        }

        /// <summary>
        /// Compacta um diretório completo em um único .zip (Zip64 habilitado automaticamente quando necessário).
        /// </summary>
        public static async Task CreateZipFromDirectoryAsync(string sourceDir, string zipPath, CompressionLevel level)
        {
            if (File.Exists(zipPath)) File.Delete(zipPath);
            var basePathLen = sourceDir.EndsWith(Path.DirectorySeparatorChar.ToString()) ? sourceDir.Length : sourceDir.Length + 1;

            using (var zipFs = new FileStream(zipPath, FileMode.CreateNew, FileAccess.Write, FileShare.None, 1024 * 128, useAsync: true))
            using (var archive = new ZipArchive(zipFs, ZipArchiveMode.Create, leaveOpen: false))
            {
                foreach (var file in Directory.EnumerateFiles(sourceDir, "*", SearchOption.AllDirectories))
                {
                    var rel = file.Substring(basePathLen);
                    var entry = archive.CreateEntry(rel, level);
                    using (var entryStream = entry.Open())
                    using (var inFs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read, 1024 * 128, useAsync: true))
                    {
                        await inFs.CopyToAsync(entryStream);
                    }
                }
            }
        }
    }
}

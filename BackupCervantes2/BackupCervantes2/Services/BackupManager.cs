using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using BackupCervantes2.Models;
using System.Collections.Concurrent;

namespace BackupCervantes2.Services
{
    public class BackupManager
    {
        private readonly UnifiedConfig _config;

        // Conjunto thread-safe que mantém quais bancos (server::database) estão em execução
        private readonly ConcurrentDictionary<string, byte> _activeDatabases = new ConcurrentDictionary<string, byte>(StringComparer.OrdinalIgnoreCase);

        public BackupManager(UnifiedConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        /// <summary>
        /// Executa todos os sistemas configurados respeitando ParallelBackups.
        /// Se um backup para o mesmo (server::database) já estiver em execução,
        /// o backup concorrente é pulado (não enfileirado).
        /// </summary>
        public async Task RunAllAsync()
        {
            Logger.Info($"Executando backups com paralelo = {_config.Global.ParallelBackups}", "");

            var sem = new SemaphoreSlim(Math.Max(1, _config.Global.ParallelBackups));

            var tasks = new List<Task>();

            foreach (var s in _config.Systems)
            {
                await sem.WaitAsync();

                tasks.Add(Task.Run(async () =>
                {
                    string dbKey = null;

                    try
                    {
                        // Calcula chave única do banco (server + database)
                        dbKey = GetDatabaseKey(s);


                        if (string.IsNullOrWhiteSpace(dbKey))
                        {
                            Logger.Error($"Chave do banco inválida para o sistema {s.Name}. Pulando.", s.Name);
                            return;
                        }

                        // Tenta marcar o banco como ativo
                        if (!_activeDatabases.TryAdd(dbKey, 0))
                        {
                            // Já existe execução em andamento para esse banco
                            Logger.Error($"Já existe backup em execução para '{dbKey}'. Pulando este job.", s.Name);
                            return;
                        }

                        Logger.Info($"[{s.Name}] Iniciando backup. dbKey={dbKey}", s.Name);

                        var msg = await ValidateAndResolveSystemAsync(s, _config);
                        if (!string.IsNullOrWhiteSpace(msg))
                        {
                            Logger.Error($"[{s.Name}] Validação falhou: {msg}", s.Name);
                            return;
                        }
                        await RunSingleAsync(s);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"[{s.Name}] Erro: {ex}", s.Name);
                    }
                    finally
                    {
                        sem.Release();
                    }
                }));
            }

            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Retorna uma chave única para identificar o banco.
        /// Prioriza campos BDServer/BDDatabase, mas tenta System.Database.* se disponível.
        /// Formato: server::database
        /// </summary>
        private string GetDatabaseKey(SystemConfig system)
        {
            if (system == null) return null;

            string server = null;
            string database = null;

            // tenta propriedades antigas (BDServer / BDDatabase)
            try
            {
                // se você tem propriedade Database (objeto), tenta também
                var dbObjProp = system.GetType().GetProperty("Database");
                if (dbObjProp != null)
                {
                    var dbObj = dbObjProp.GetValue(system);
                    if (dbObj != null)
                    {
                        var serverProp = dbObj.GetType().GetProperty("Server");
                        var databaseProp = dbObj.GetType().GetProperty("Database");
                        if (serverProp != null) server = serverProp.GetValue(dbObj)?.ToString();
                        if (databaseProp != null) database = databaseProp.GetValue(dbObj)?.ToString();
                    }
                }
            }
            catch
            {
                // ignore reflection failures
            }

            // fallback para propriedades antigas nomeadas BDServer / BDDatabase
            if (string.IsNullOrWhiteSpace(server))
                server = (system.BDServer ?? system.DatabaseServer()).SafeTrim();

            if (string.IsNullOrWhiteSpace(database))
                database = (system.BDDatabase ?? system.DatabaseName()).SafeTrim();

            if (string.IsNullOrWhiteSpace(database))
            {
                // se não há DB informado, podemos usar NomeArquivoDeBackup ou Name como fallback
                database = system.NomeArquivoDeBackup ?? system.Name;
            }

            server = string.IsNullOrWhiteSpace(server) ? "local" : server;

            return $"{server.ToLowerInvariant()}::{database.ToLowerInvariant()}";
        }

        // Métodos auxiliares para compatibilidade com diferentes modelos:
        // Caso seu SystemConfig já tenha DatabaseServer() / DatabaseName() esses métodos serão ignorados.



        private async Task<string> ValidateAndResolveSystemAsync(SystemConfig s, UnifiedConfig config)
        {
            var baseFolder = Path.Combine(AppContext.BaseDirectory, config.Global.PgDumpVersionsFolder ?? "pg_dump_versions");

            if (string.IsNullOrWhiteSpace(s.PgDumpVersion))
                throw new InvalidOperationException($"Sistema {s.Name} não possui PgDumpVersion configurado.");

            var exePath = Path.Combine(baseFolder, s.PgDumpVersion, "pg_dump.exe");

            if (!File.Exists(exePath))
                throw new FileNotFoundException($"pg_dump não encontrado em {exePath}");

            if (string.IsNullOrWhiteSpace(s.PathBackupLocal))
                return "PathBackupLocal vazio.";
            if (string.IsNullOrWhiteSpace(s.NomeArquivoDeBackup))
                return "NomeArquivoDeBackup vazio.";
            if (string.IsNullOrWhiteSpace(s.BDServer) || string.IsNullOrWhiteSpace(s.BDSuperUsuarioLogin) || string.IsNullOrWhiteSpace(s.BDDatabase))
                return "Config DB incompleta (BDServer/Login/Database).";

            // Espaço livre
            try
            {
                var root = Path.GetPathRoot(s.PathBackupLocal);
                var drive = new DriveInfo(root);
                var freeMb = drive.AvailableFreeSpace / (1024 * 1024);
                if (freeMb < _config.Global.MinimumFreeSpaceMB)
                    return $"Espaço livre insuficiente em '{s.PathBackupLocal}': {freeMb}MB.";
            }
            catch (Exception ex)
            {
                Logger.Error($"Falha ao checar espaço: {ex.Message}", "");
            }
            return null;
        }

        //private string FindPgDumpInFolder(string root)
        //{
        //    try
        //    {
        //        if (!Directory.Exists(root)) return null;
        //        var files = Directory.GetFiles(root, "pg_dump.exe", SearchOption.AllDirectories);
        //        return files.Length > 0 ? files.OrderByDescending(f => f).First() : null;
        //    }
        //    catch { return null; }
        //}

        private async Task RunSingleAsync(SystemConfig s)
        {
            var ts = DateTime.Now;
            var timestamp = ts.ToString("yyyyMMdd_HHmmss");

            // 1) Dump
            var tmpDumpDir = Path.Combine(s.PathBackupLocal, $"{s.NomeArquivoDeBackup}_{timestamp}_dir");


            int jobs = s.Jobs.HasValue && s.Jobs.Value > 0 ? s.Jobs.Value : _config.Global.DefaultJobs;
            var useDir = s.UseDirectoryFormat.HasValue ? s.UseDirectoryFormat.Value : _config.Global.PreferDirectoryFormat;

            if (useDir)
            {
                Directory.CreateDirectory(tmpDumpDir);

                Logger.Info($"[{s.Name}] pg_dump -Fd com {jobs} jobs → {tmpDumpDir}", s.Name);

                PgDumpRunner.RunDirectoryDump(s, tmpDumpDir, jobs, _config);

                // 2) Zip (um único .zip)
                var zipName = BuildZipName(s, ts);
                var zipPath = Path.Combine(s.PathBackupLocal, zipName);
                var lvl = ZipUtils.ParseLevel(s.ZipCompressionLevel);
                Logger.Info($"[{s.Name}] Compactando diretório em ZIP: {zipPath} (level={s.ZipCompressionLevel})", s.Name);
                await ZipUtils.CreateZipFromDirectoryAsync(tmpDumpDir, zipPath, lvl);

                // 3) Upload/Storage
                await UploadFinalFileAsync(zipPath, s);

                // 4) Limpeza
                TryDeleteDirectory(tmpDumpDir);
                //TryDeleteFile(zipPath);

                Logger.Info($"[{s.Name}] OK (ZIP enviado).", s.Name);
            }
            else
            {
                // Fallback -Fc
                var dumpFile = Path.Combine(s.PathBackupLocal, $"{s.NomeArquivoDeBackup}_{timestamp}.dump");
                Logger.Info($"[{s.Name}] pg_dump -Fc -> {dumpFile}", s.Name);
                PgDumpRunner.RunCustomFileDump(s, dumpFile, _config);

                var zipName = BuildZipName(s, ts);
                var zipPath = Path.Combine(s.PathBackupLocal, zipName);

                Logger.Info($"[{s.Name}] Compactando arquivo em ZIP: {zipPath}", s.Name);

                await ZipSingleFileAsync(dumpFile, zipPath, ZipUtils.ParseLevel(s.ZipCompressionLevel));

                await UploadFinalFileAsync(zipPath, s);

                TryDeleteFile(dumpFile);
                //TryDeleteFile(zipPath);

                Logger.Info($"[{s.Name}] OK (ZIP enviado - custom).", s.Name);
            }
        }

        private string BuildZipName(SystemConfig s, DateTime ts)
        {
            var pattern = s.ZipNamePattern ?? "{Nome}_{yyyyMMdd_HHmmss}.zip";
            return pattern
                .Replace("{Nome}", s.NomeArquivoDeBackup ?? s.Name)
                .Replace("{yyyyMMdd_HHmmss}", ts.ToString("yyyyMMdd_HHmmss"))
                .Replace("{yyyy}", ts.ToString("yyyy"))
                .Replace("{MM}", ts.ToString("MM"))
                .Replace("{dd}", ts.ToString("dd"))
                .Replace("{HH}", ts.ToString("HH"))
                .Replace("{mm}", ts.ToString("mm"))
                .Replace("{ss}", ts.ToString("ss"));
        }

        private async Task ZipSingleFileAsync(string inputFile, string zipPath, System.IO.Compression.CompressionLevel level)
        {
            if (File.Exists(zipPath)) File.Delete(zipPath);
            using (var zipFs = new FileStream(zipPath, FileMode.CreateNew, FileAccess.Write, FileShare.None, 1024 * 128, useAsync: true))
            using (var archive = new System.IO.Compression.ZipArchive(zipFs, System.IO.Compression.ZipArchiveMode.Create, leaveOpen: false))
            {
                var entryName = Path.GetFileName(inputFile);
                var entry = archive.CreateEntry(entryName, level);
                using (var entryStream = entry.Open())
                using (var inFs = new FileStream(inputFile, FileMode.Open, FileAccess.Read, FileShare.Read, 1024 * 128, useAsync: true))
                {
                    await inFs.CopyToAsync(entryStream);
                }
            }
        }

        private async Task UploadFinalFileAsync(string filePath, SystemConfig s)
        {
            var provider = StorageFactory.CreateForSystem(_config, s);

            var remotePath = $"{s.NomeArquivoDeBackup}/{Path.GetFileName(filePath)}";

            await provider.UploadFileAsync(filePath, remotePath, s.Name);

            (provider as IDisposable)?.Dispose();
        }

        private static void TryDeleteDirectory(string dir)
        {
            try { if (Directory.Exists(dir)) Directory.Delete(dir, true); }
            catch (Exception ex) { Logger.Error($"Não foi possível apagar '{dir}': {ex.Message}", ""); }
        }

        private static void TryDeleteFile(string file)
        {
            try { if (File.Exists(file)) File.Delete(file); }
            catch (Exception ex) { Logger.Error($"Não foi possível apagar '{file}': {ex.Message}", ""); }
        }

    }

    // ==== Extensões auxiliares (pode colocar em arquivo Utils/Extensions.cs) ====
    internal static class BackupExtensions
    {
        // tenta pegar BDServer via Reflection (campo alternativo) - se não existir retorna null
        public static string DatabaseServer(this SystemConfig s)
        {
            try
            {
                var prop = s.GetType().GetProperty("BDServer");
                return prop?.GetValue(s)?.ToString();
            }
            catch { return null; }
        }

        public static string DatabaseName(this SystemConfig s)
        {
            try
            {
                var prop = s.GetType().GetProperty("BDDatabase");
                return prop?.GetValue(s)?.ToString();
            }
            catch { return null; }
        }

        public static string SafeTrim(this string s) => s?.Trim();
    }
}

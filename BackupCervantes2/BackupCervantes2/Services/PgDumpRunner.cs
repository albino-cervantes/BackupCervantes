using BackupCervantes2.Models;
using BackupCervantes2;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace BackupCervantes2.Services
{
    public static class PgDumpRunner
    {
        public static void RunDirectoryDump(SystemConfig s, string dirDestination, int jobs, UnifiedConfig config)
        {
            //if (string.IsNullOrWhiteSpace(s.PgDumpVersion) || !File.Exists(s.PgDumpVersion))
            //    throw new FileNotFoundException("pg_dump não encontrado: " + (s.PgDumpVersion ?? "(vazio)"));

            if (!Directory.Exists(dirDestination))
                Directory.CreateDirectory(dirDestination);

            var jobsCount = Math.Max(1, jobs);
            var ignoreArgs = BuildIgnoreArgs(s.TabelasIgnoradasNoBackup);

            var args = $"{ignoreArgs} -F d -j {jobsCount} -f \"{dirDestination}\" --host \"{s.BDServer}\" --username \"{s.BDSuperUsuarioLogin}\" --port {s.BDPort} \"{s.BDDatabase}\"";

            var psi = new ProcessStartInfo
            {
                FileName = ResolvePgDumpPath(s, config),
                Arguments = args,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = false,
                CreateNoWindow = true
            };

            if (!string.IsNullOrWhiteSpace(s.BDSuperUsuarioPassword))
                psi.Environment["PGPASSWORD"] = s.BDSuperUsuarioPassword;

            Logger.Info($"pg_dump (dir) {s.Name}: {Path.GetFileName(ResolvePgDumpPath(s, config))} {args}","");
            using (var p = Process.Start(psi))
            {
                var stderr = p.StandardError.ReadToEnd();
                p.WaitForExit();

                if (!string.IsNullOrWhiteSpace(stderr))
                    Logger.Error($"pg_dump stderr ({s.Name}): {stderr}","");

                if (p.ExitCode != 0)
                    throw new Exception($"pg_dump retornou {p.ExitCode} para {s.Name}.");
            }
        }

        public static void RunCustomFileDump(SystemConfig s, string outFile, UnifiedConfig config)
        {
            var ignoreArgs = BuildIgnoreArgs(s.TabelasIgnoradasNoBackup);
            var args = $"{ignoreArgs} --format custom --blobs --file \"{outFile}\" --host \"{s.BDServer}\" --username \"{s.BDSuperUsuarioLogin}\" --port {s.BDPort} \"{s.BDDatabase}\"";

            var psi = new ProcessStartInfo
            {
                FileName = ResolvePgDumpPath(s, config),
                Arguments = args,
                UseShellExecute = false,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            if (!string.IsNullOrWhiteSpace(s.BDSuperUsuarioPassword))
                psi.Environment["PGPASSWORD"] = s.BDSuperUsuarioPassword;

            Logger.Info($"pg_dump (custom) {s.Name}: {Path.GetFileName(s.PgDumpVersion)} {args}",s.Name);
            using (var p = Process.Start(psi))
            {
                var stderr = p.StandardError.ReadToEnd();
                p.WaitForExit();

                if (!string.IsNullOrWhiteSpace(stderr))
                    Logger.Error($"pg_dump stderr ({s.Name}): {stderr}",s.Name);

                if (p.ExitCode != 0)
                    throw new Exception($"pg_dump (custom) retornou {p.ExitCode} para {s.Name}.");
            }
        }

        public static string BuildIgnoreArgs(string csv)
        {
            if (string.IsNullOrWhiteSpace(csv)) return string.Empty;
            var parts = csv.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            var sb = new StringBuilder();
            foreach (var p in parts)
            {
                var t = p.Trim();
                if (string.IsNullOrWhiteSpace(t)) continue;
                sb.Append($" --exclude-table-data={t}");
            }
            return sb.ToString();
        }

        private static string ResolvePgDumpPath(SystemConfig system, UnifiedConfig config)
        {
            var baseFolder = Path.Combine(AppContext.BaseDirectory, config.Global.PgDumpVersionsFolder ?? "pg_dump_versions");

            if (string.IsNullOrWhiteSpace(system.PgDumpVersion))
                throw new InvalidOperationException($"Sistema {system.Name} não possui PgDumpVersion configurado.");

            var exePath = Path.Combine(baseFolder, system.PgDumpVersion, "pg_dump.exe");

            if (!File.Exists(exePath))
                throw new FileNotFoundException($"pg_dump não encontrado em {exePath}");

            return exePath;
        }
    }
}

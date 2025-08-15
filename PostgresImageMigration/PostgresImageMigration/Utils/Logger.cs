using System;
using System.IO;
using System.Configuration;

namespace PostgresImageMigration.Utils
{
    /// <summary>
    /// Logger simples que grava em console e arquivo.
    /// </summary>
    public static class Logger
    {
        private static string _logFile;

        public static void Initialize()
        {
            try
            {
                var path = AppDomain.CurrentDomain.BaseDirectory;
                var cfg = ConfigurationManager.AppSettings["Log_File"];
                _logFile = string.IsNullOrWhiteSpace(cfg) ? Path.Combine(path, "log_migracao.txt") : Path.Combine(path, cfg);
                // Garante diretório
                Directory.CreateDirectory(Path.GetDirectoryName(_logFile));
            }
            catch
            {
                _logFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log_migracao.txt");
            }
        }

        public static void Log(string message)
        {
            var text = string.Format("{0:yyyy-MM-dd HH:mm:ss} - {1}", DateTime.Now, message);
            try
            {
                Console.WriteLine(text);
                File.AppendAllText(_logFile, text + Environment.NewLine);
            }
            catch
            {
                // não interrompe por erro de log
            }
        }

        public static void LogException(string context, Exception ex)
        {
            Log(string.Format("{0} - EXCEPTION: {1} \n{2}", context, ex.Message, ex.ToString()));
        }
    }
}

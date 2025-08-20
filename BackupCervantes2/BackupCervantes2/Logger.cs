using System;
using System.IO;

namespace BackupCervantes2
{
    public static class Logger
    {
        private static readonly object _lock = new object();
        private static string _globalLogDir = Path.Combine(AppContext.BaseDirectory, "logs");

        public static void Info(string message, string systemName) =>
            Write("INFO", message, systemName);

        public static void Error(string message, string systemName) =>
            Write("ERROR", message, systemName);

        private static void Write(string level, string message, string systemName)
        {
            lock (_lock)
            {
                var logDir = Path.Combine(_globalLogDir, systemName);
                if (!Directory.Exists(logDir)) Directory.CreateDirectory(logDir);

                var logFile = Path.Combine(logDir, $"{DateTime.Now:yyyyMMdd}.log");

                var formatted = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}";
                File.AppendAllText(logFile, formatted + Environment.NewLine);

                Console.WriteLine($"[{systemName}] {formatted}");
            }
        }
    }
}

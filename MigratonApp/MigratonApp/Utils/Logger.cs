using System;
using System.IO;

namespace MigrationApp.Utils
{
    public class Logger : IDisposable
    {
        private StreamWriter _writer;

        public Logger(string path)
        {
            _writer = new StreamWriter(path, true);
            _writer.AutoFlush = true;
        }

        public void Info(string message) => Log("INFO", message);
        public void Error(string message, Exception ex = null) => Log("ERROR", $"{message} - {ex?.Message}\n{ex?.StackTrace}");

        private void Log(string level, string message)
        {
            string logLine = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";
            _writer.WriteLine(logLine);
            Console.WriteLine(logLine);
        }

        public void Dispose() => _writer?.Dispose();
    }
}
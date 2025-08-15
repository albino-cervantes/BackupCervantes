using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigracaoPostgreSQL.Utils
{
    /// <summary>
    /// Classe responsável pelo sistema de logging da aplicação
    /// </summary>
    public class Logger : IDisposable
    {
        private readonly string _logFilePath;
        private readonly object _lockObject = new object();
        private StreamWriter _writer;
        private bool _disposed = false;

        public Logger(string logFileName)
        {
            _logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, logFileName);
            InitializeLogFile();
        }

        private void InitializeLogFile()
        {
            try
            {
                _writer = new StreamWriter(_logFilePath, append: true, Encoding.UTF8)
                {
                    AutoFlush = true
                };

                LogInfo("=== SISTEMA DE MIGRAÇÃO INICIADO ===");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao inicializar arquivo de log: {ex.Message}");
            }
        }

        /// <summary>
        /// Registra uma mensagem informativa
        /// </summary>
        public void LogInfo(string message)
        {
            WriteLog("INFO", message, null);
        }

        /// <summary>
        /// Registra uma mensagem de aviso
        /// </summary>
        public void LogWarning(string message)
        {
            WriteLog("WARNING", message, null);
        }

        /// <summary>
        /// Registra uma mensagem de erro
        /// </summary>
        public void LogError(string message, Exception exception = null)
        {
            WriteLog("ERROR", message, exception);
        }

        /// <summary>
        /// Registra uma mensagem de debug
        /// </summary>
        public void LogDebug(string message)
        {
            WriteLog("DEBUG", message, null);
        }

        private void WriteLog(string level, string message, Exception exception)
        {
            if (_disposed) return;

            lock (_lockObject)
            {
                try
                {
                    var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    var logEntry = new StringBuilder();

                    logEntry.AppendLine($"[{timestamp}] [{level}] {message}");

                    if (exception != null)
                    {
                        logEntry.AppendLine($"Exception: {exception.GetType().Name}");
                        logEntry.AppendLine($"Message: {exception.Message}");
                        logEntry.AppendLine($"StackTrace: {exception.StackTrace}");

                        if (exception.InnerException != null)
                        {
                            logEntry.AppendLine($"Inner Exception: {exception.InnerException.Message}");
                        }
                    }

                    _writer?.Write(logEntry.ToString());

                    // Também exibir no console para acompanhamento em tempo real
                    Console.WriteLine($"[{level}] {message}");
                    if (exception != null)
                    {
                        Console.WriteLine($"Erro: {exception.Message}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao escrever no log: {ex.Message}");
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                lock (_lockObject)
                {
                    LogInfo("=== SISTEMA DE MIGRAÇÃO FINALIZADO ===");
                    _writer?.Dispose();
                    _disposed = true;
                }
            }
        }

        ~Logger()
        {
            Dispose(false);
        }
    }
}

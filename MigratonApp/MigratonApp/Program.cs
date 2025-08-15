using System;
using System.IO;
using MigrationApp.Services;

namespace MigrationApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", $"Log_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
            Directory.CreateDirectory(Path.GetDirectoryName(logPath));

            using (var logger = new Utils.Logger(logPath))
            {
                try
                {
                    logger.Info("Iniciando migração de dados...");

                    var migrationService = new MigrationService(logger);
                    migrationService.ExecuteMigration();

                    logger.Info("Migração finalizada com sucesso.");
                }
                catch (Exception ex)
                {
                    logger.Error("Erro inesperado na aplicação", ex);
                }
            }
        }
    }
}

using System.ServiceProcess;
using BackupCervantes2.Models;
using BackupCervantes2.Services;

namespace BackupCervantes2.Service
{
    public class BackupService : ServiceBase
    {
        private readonly UnifiedConfig _config;
        private SimpleScheduler _scheduler;

        public BackupService(UnifiedConfig config)
        {
            ServiceName = "BackupUnifiedService";
            _config = config;
        }

        protected override void OnStart(string[] args)
        {
            Logger.Info("BackupUnifiedService iniciando...","Servico");
            var manager = new BackupManager(_config);

            if (_config.Global.Schedule.Enabled)
            {
                _scheduler = new SimpleScheduler(_config, async () => await manager.RunAllAsync());
                _scheduler.Start();
                Logger.Info("Agendamento do serviço iniciado.", "Servico");
            }
            else
            {
                // Sem agendamento: roda uma vez
                manager.RunAllAsync().GetAwaiter().GetResult();
            }
        }

        protected override void OnStop()
        {
            Logger.Info("BackupUnifiedService parando...", "Servico");
            _scheduler?.Dispose();
        }
    }
}

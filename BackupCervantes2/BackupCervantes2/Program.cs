using System;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using BackupCervantes2.Models;
using BackupCervantes2;
using Newtonsoft.Json;

namespace BackupCervantes2
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                var runAsService = args.Any(a => a.Equals("--service", StringComparison.OrdinalIgnoreCase));
                var runOnce = args.Any(a => a.Equals("--run-once", StringComparison.OrdinalIgnoreCase));

                var basePath = AppDomain.CurrentDomain.BaseDirectory;
                var cfgPath = Path.Combine(basePath, "config.json");

                if (!File.Exists(cfgPath))
                {
                    Console.WriteLine("Arquivo config.json não encontrado: " + cfgPath);
                    return 1;
                }

                var json = File.ReadAllText(cfgPath);
                var config = JsonConvert.DeserializeObject<UnifiedConfig>(json) ?? new UnifiedConfig();

                //var logPath = string.IsNullOrWhiteSpace(config.Global.LogFile)
                //    ? Path.Combine(basePath, "backup_unified.log")
                //    : Path.IsPathRooted(config.Global.LogFile)
                //        ? config.Global.LogFile
                //        : Path.Combine(basePath, config.Global.LogFile);

                //Logger.Init(logPath);

                if (runAsService)
                {
                    ServiceBase.Run(new Service.BackupService(config));
                    return 0;
                }

                // Console mode
                Logger.Info("=== Iniciando Backup Unified (Console) ===","");
                var manager = new Services.BackupManager(config);

                if (runOnce || !config.Global.Schedule.Enabled)
                {
                    manager.RunAllAsync().GetAwaiter().GetResult();
                }
                else
                {
                    Logger.Info("Agendamento ativo em modo console. Pressione Ctrl+C para encerrar.", "");
                    using (var sched = new Service.SimpleScheduler(config, async () => await manager.RunAllAsync()))
                    {
                        sched.Start();
                        System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
                    }
                }

                Logger.Info("=== Processo de backup concluído (Console) ===", "");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro fatal: " + ex);
                return 2;
            }
        }
    }
}

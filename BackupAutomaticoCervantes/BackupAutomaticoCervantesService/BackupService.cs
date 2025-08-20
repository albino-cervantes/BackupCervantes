using BackupAutomaticoCervantes.Padrao;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using QuartzScheduler = BackupWindowsService.QuartzScheduler;

namespace BackupAutomaticoCervantesService
{
    public partial class BackupService : ServiceBase
    {
        private QuartzScheduler _quartz;

        public BackupService() 
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // 1) monta o caminho absoluto onde o JSON realmente está
            //    por exemplo, se o serviço estiver em C:\Services\BackupService\bin\Debug\
            //    e o JSON foi copiado para C:\Services\BackupService\Config\, faça:
            string pastaConfig = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,   // ex: ...\bin\Debug\
                "..", "..",                             // sobe duas pastas até a raiz do serviço
                "Config"                                // nome da pasta onde o JSON está
            );
            string arquivo = Path.GetFullPath(Path.Combine(pastaConfig, "ConfigBackupAutomaticoCervantes.json"));

            // 2) diz ao ConfigManager para usar esse path
            ConfigManager.SetConfigPath(arquivo);

            // 3) agora carrega sua configuração normalmente
            var config = ConfigManager.Instance.Config;
            var parametros = config.ListaDeParamentos;

            _quartz = new QuartzScheduler();
            _quartz.StartAsync(parametros).GetAwaiter().GetResult();
        }

        protected override void OnStop()
        {
            _quartz.StopAsync().GetAwaiter().GetResult();
        }
    }
}

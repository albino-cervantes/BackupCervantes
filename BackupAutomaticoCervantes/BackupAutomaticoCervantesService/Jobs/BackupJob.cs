using BackupAutomaticoCervantesService;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using System;
using System.Threading.Tasks;

namespace BackupWindowsService.Jobs
{
    /// <summary>
    /// Job responsável por executar o backup de um banco específico.
    /// O atributo [DisallowConcurrentExecution] impede que duas instâncias
    /// deste mesmo JobDetail (mesmo banco) rodem em paralelo.
    /// </summary>
    [DisallowConcurrentExecution]
    public class BackupJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            // Recupera o ParametrosBackupModel passado via JobDataMap
            var parametros = (BackupAutomaticoCervantes.ParametrosBackupModel)
                context.MergedJobDataMap.Get("Parametros");

            // Roda a rotina de backup (pg_dump + destinos)
            BackupRunner.Run(parametros);

            return Task.CompletedTask;
        }
    }
}

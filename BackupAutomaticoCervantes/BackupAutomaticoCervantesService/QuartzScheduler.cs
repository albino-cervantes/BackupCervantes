using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Quartz.Logging.OperationName;

namespace BackupWindowsService
{
    public class QuartzScheduler
    {
        private IScheduler _scheduler;

        public async Task StartAsync(IEnumerable<BackupAutomaticoCervantes.ParametrosBackupModel> parametrosList)
        {
            // Cria Factory e Scheduler
            var factory = new StdSchedulerFactory();
            _scheduler = await factory.GetScheduler();
            await _scheduler.Start();

            // Para cada configuração de backup e cada agendamento, crie um Job + Trigger
            foreach (var p in parametrosList)
            {
                foreach (var ag in p.Agendamentos)
                {
                    // Identificadores únicos para Job e Trigger
                    string jobKey = $"BackupJob-{p.Id}";
                    string triggerKey = $"Trigger-{p.Id}-{ag.Id}";

                    // JobDetail: carrega o objeto de parâmetros
                    var job = JobBuilder.Create<Jobs.BackupJob>()
                        .WithIdentity(jobKey, "BackupGroup")
                        .UsingJobData(new JobDataMap { { "Parametros", p } })
                        .Build();

                    // Monta um cron expression:
                    // se ExecutarTodosOsDias = true => todos os dias
                    // senão, apenas nos dias selecionados (enum System.DayOfWeek)
                    string days = ag.ExecutarTodosOsDias
                        ? "*"
                        : string.Join(",", ag.DiasDaSemana.ConvertAll(d => ((int)d + 1) % 7));
                    // Quartz usa 1=domingo … 7=sábado

                    // Cron: segundos minutos horas dia-do-mês mês dia-da-semana ano(omisso)
                    string cronExpr = $"0 {ag.Hora.Minutes} {ag.Hora.Hours} ? * {days}";

                    var trigger = TriggerBuilder.Create()
                        .WithIdentity(triggerKey, "BackupGroup")
                        .WithCronSchedule(cronExpr, x => x.WithMisfireHandlingInstructionDoNothing())
                        .ForJob(job)
                        .Build();

                    // Agendar
                    await _scheduler.ScheduleJob(job, trigger);
                }
            }
        }

        public async Task StopAsync()
        {
            if (_scheduler != null)
                await _scheduler.Shutdown(waitForJobsToComplete: true);
        }
    }
}

using BackupCervantes2.Models;
using BackupCervantes2;
using System;
using System.Threading;

namespace BackupCervantes2.Service
{
    public sealed class SimpleScheduler : IDisposable
    {
        private readonly UnifiedConfig _config;
        private readonly Func<System.Threading.Tasks.Task> _job;
        private Timer _timer;
        private volatile bool _running;

        public SimpleScheduler(UnifiedConfig config, Func<System.Threading.Tasks.Task> job)
        {
            _config = config;
            _job = job;
        }

        public void Start()
        {
            var due = _config.Global.Schedule.RunAtStartup ? TimeSpan.Zero : TimeSpan.FromMinutes(_config.Global.Schedule.IntervalMinutes);
            var period = TimeSpan.FromMinutes(Math.Max(1, _config.Global.Schedule.IntervalMinutes));
            _timer = new Timer(async _ => await Tick(), null, due, period);
        }

        private async System.Threading.Tasks.Task Tick()
        {
            if (_running) return; // evita sobreposição
            _running = true;
            try { await _job(); }
            catch (Exception ex) { Logger.Error("Erro no agendamento: " + ex,""); }
            finally { _running = false; }
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}

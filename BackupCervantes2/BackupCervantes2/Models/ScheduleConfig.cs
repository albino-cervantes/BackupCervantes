using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupCervantes2.Models
{
    public class ScheduleConfig
    {
        public bool Enabled { get; set; } = false;          // se true: executa conforme IntervalMinutes
        public int IntervalMinutes { get; set; } = 1440;    // padrão: 1x por dia
        public bool RunAtStartup { get; set; } = true;
    }
}

using System.Collections.Generic;

namespace BackupCervantes2.Models
{
    public class UnifiedConfig
    {
        public GlobalConfig Global { get; set; } = new GlobalConfig();
        public List<SystemConfig> Systems { get; set; } = new List<SystemConfig>();
    }
}

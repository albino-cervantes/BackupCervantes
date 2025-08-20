namespace BackupCervantes2.Models
{
    public class GlobalConfig
    {
        public int ParallelBackups { get; set; } = 2;       // sistemas em paralelo
        public int DefaultJobs { get; set; } = 6;           // -j do pg_dump
        public bool PreferDirectoryFormat { get; set; } = true;

        public long MinimumFreeSpaceMB { get; set; } = 10240;

        // Pastas incluídas no build/output (ver .csproj)
        public string PgDumpVersionsFolder { get; set; } = "pg_dump_versions";
        public string SecretsFolder { get; set; } = "secrets";

        // Armazenamento: Local | S3 | GDrive
        public StorageConfig Storage { get; set; } = new StorageConfig();

        // Agendamento simples
        public ScheduleConfig Schedule { get; set; } = new ScheduleConfig();
    }
}

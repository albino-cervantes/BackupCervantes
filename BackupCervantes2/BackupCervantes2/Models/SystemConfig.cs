namespace BackupCervantes2.Models
{
    public class SystemConfig
    {
        public string Name { get; set; }

        public string PgDumpVersion { get; set; }

        public string TabelasIgnoradasNoBackup { get; set; } // csv: schema.tabela

        // Se não informado, tentaremos localizar automaticamente em Global.PgDumpVersionsFolder
        //public string PathExeDumpPostgres { get; set; }

        public string PathBackupLocal { get; set; }
        public string PathBackupDestinoDrive { get; set; }
        public string NomeArquivoDeBackup { get; set; }

        public bool BDUsarServidorLocal { get; set; } = false;
        public string BDServer { get; set; }
        public string BDPort { get; set; } = "5432";
        public string BDSuperUsuarioPassword { get; set; }
        public string BDSuperUsuarioLogin { get; set; }
        public string BDDatabase { get; set; }

        // overrides
        public bool? UseDirectoryFormat { get; set; }
        public int? Jobs { get; set; }
        public string ZipCompressionLevel { get; set; } = "Optimal"; // "Optimal" | "Fastest" | "NoCompression"
        public string ZipNamePattern { get; set; } = "{Nome}_{yyyyMMdd_HHmmss}.zip";
    }
}

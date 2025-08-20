using System;
using System.IO;
using System.Reflection;

public static class ServiceConfigLocator
{
    public static string GetConfigPath()
    {
        // 1) Pega o Assembly onde está a sua classe de modelo
        //    (pode ser AppConfigModel, ConfigManager ou qualquer outra).
        Assembly domainAssembly = typeof(BackupAutomaticoCervantes.AppConfigModel)
                                  .Assembly;

        // 2) Descobre a pasta onde esse DLL está carregado
        string domainDir = Path.GetDirectoryName(domainAssembly.Location);

        // 3) Monta o nome do JSON esperado naquela mesma pasta
        string configFileName = "ConfigBackupAutomaticoCervantes.json";

        return Path.Combine(domainDir, configFileName);
    }
}

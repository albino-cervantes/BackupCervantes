
using System.Collections.Generic;
using System.IO;
using BackupAutomaticoCervantes.Padrao;
using Newtonsoft.Json;

namespace BackupAutomaticoCervantes
{
    public class AppConfigModel
    {
        public List<ParametrosBackupModel> ListaDeParamentos { get; set; } = new List<ParametrosBackupModel>();
    }
}

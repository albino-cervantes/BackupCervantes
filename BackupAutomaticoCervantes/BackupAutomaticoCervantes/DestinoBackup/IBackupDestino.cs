using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupAutomaticoCervantes.DestinoBackup
{
    public interface IBackupDestino
    {
        Task EnviarBackupAsync(string caminhoArquivoLocal);

    }
}

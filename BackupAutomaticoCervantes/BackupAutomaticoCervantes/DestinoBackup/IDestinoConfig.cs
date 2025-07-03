using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupAutomaticoCervantes.DestinoBackup
{
    /// <summary>
    /// Interface base para configurações de destino de backup.
    /// </summary>
    public interface IDestinoConfig
    {
        Guid Id { get; }
        DestinoTipo Tipo { get; }
    }
}

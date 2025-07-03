using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupAutomaticoCervantes.DestinoBackup.Factory
{
    /// <summary>
    /// Interface para o Factory Pattern
    /// Define o contrato para criação de destinos de backup
    /// </summary>
    public interface IBackupDestinoFactory
    {
        /// <summary>
        /// Cria uma instância de IBackupDestino baseada na configuração
        /// </summary>
        /// <param name="configuracao">Configuração do destino</param>
        /// <returns>Instância do destino de backup</returns>
        IBackupDestino CriarDestino(IDestinoConfig config);
    }
}

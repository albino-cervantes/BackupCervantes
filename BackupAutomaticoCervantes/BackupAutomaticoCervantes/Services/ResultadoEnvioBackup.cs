using BackupAutomaticoCervantes.DestinoBackup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupAutomaticoCervantes.Services
{
    /// <summary>
    /// Classe para armazenar o resultado do envio de backup para um destino
    /// </summary>
    public class ResultadoEnvioBackup
    {
        public Guid DestinoId { get; set; }
        public DestinoTipo TipoDestino { get; set; }
        public bool Sucesso { get; set; }
        public string MensagemErro { get; set; }
        public DateTime IniciadoEm { get; set; }
        public DateTime? FinalizadoEm { get; set; }

        public TimeSpan? TempoExecucao
        {
            get
            {
                if (FinalizadoEm.HasValue)
                {
                    return FinalizadoEm.Value - IniciadoEm;
                }
                return null;
            }
        }

        public override string ToString()
        {
            var status = Sucesso ? "✅ SUCESSO" : "❌ ERRO";
            var tempo = TempoExecucao?.TotalSeconds.ToString("F2") ?? "N/A";
            return $"{status} - {TipoDestino} ({tempo}s) {(string.IsNullOrEmpty(MensagemErro) ? "" : $"- {MensagemErro}")}";
        }
    }
}

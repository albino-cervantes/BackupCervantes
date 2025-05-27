using BackupAutomaticoCervantes.Padrao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupAutomaticoCervantes.Models
{
    public class HorarioAgendamentoModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>Hora do agendamento (formato HH:mm)</summary>
        public TimeSpan Hora { get; set; }

        /// <summary>Lista de dias da semana em que o agendamento ocorre (ex: Sábado)</summary>
        public List<DiasDaSemana> DiasDaSemana { get; set; } = new List<DiasDaSemana>();

        /// <summary>Se verdadeiro, ignora os dias da semana e executa todos os dias</summary>
        public bool ExecutarTodosOsDias { get; set; } = false;

        public override string ToString()
        {
            string dias = ExecutarTodosOsDias ? "Todos os dias" : string.Join(", ", DiasDaSemana);
            return $"{dias} às {Hora:hh\\:mm}";
        }
    }
}

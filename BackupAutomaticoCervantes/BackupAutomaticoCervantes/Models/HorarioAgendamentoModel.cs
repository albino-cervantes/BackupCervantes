using BackupAutomaticoCervantes.Padrao;
using Newtonsoft.Json;
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

        public string DiasDaSemanaDisplay
        {
            get
            {
               return ToString();
            }
        }

        public override string ToString()
        {
            string dias = ExecutarTodosOsDias ? "Todos os dias" : string.Join(", ", DiasDaSemana);
            return $"{dias} às {Hora:hh\\:mm}";
        }

        /// <summary>Marca interna se já executou hoje (não é serializado)</summary>
        [JsonIgnore]
        public bool ExecutouHoje { get; set; }

        // <summary>Última data em que executou (uso interno, pode ajudar no reset)</summary>
        [JsonIgnore]
        public DateTime? DataUltimaExecucao { get; set; }

        /// <summary>
        /// Calcula o próximo DateTime agendado para hoje, com base em Hora e DiasDaSemana.
        /// </summary>
        public DateTime? DataHoraAgendadaHoje()
        {
            var hoje = DateTime.Today;
            var dow = hoje.DayOfWeek; // System.DayOfWeek
            if (!ExecutarTodosOsDias && !DiasDaSemana.Contains((DiasDaSemana)dow))
                return null;

            return hoje.Add(Hora);
        }

        /// <summary>
        /// Diz se deve executar agora: 
        /// – Estamos no minuto certo (hora e minuto batem),
        /// – E ainda não executou hoje ou já mudou o dia.
        /// </summary>
        public bool ShouldRun(DateTime now)
        {
            var dtAgendada = DataHoraAgendadaHoje();
            if (dtAgendada == null)
                return false;

            // Se o dia mudou, resetar
            if (DataUltimaExecucao.HasValue && DataUltimaExecucao.Value.Date < now.Date)
                ExecutouHoje = false;

            // Executa se bateu hora/minuto e ainda não executou hoje
            if (!ExecutouHoje
                && now.Hour == dtAgendada.Value.Hour
                && now.Minute == dtAgendada.Value.Minute)
            {
                ExecutouHoje = true;
                DataUltimaExecucao = now;
                return true;
            }

            return false;
        }
    }
}

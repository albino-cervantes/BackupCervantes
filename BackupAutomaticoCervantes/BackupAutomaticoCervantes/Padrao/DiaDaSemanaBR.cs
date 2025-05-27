using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupAutomaticoCervantes.Padrao
{
    public static class DiaDaSemanaBR
    {
        public static readonly Dictionary<DayOfWeek, string> Nomes = new Dictionary<DayOfWeek, string>
    {
        { DayOfWeek.Sunday,    "Domingo" },
        { DayOfWeek.Monday,    "Segunda-feira" },
        { DayOfWeek.Tuesday,   "Terça-feira" },
        { DayOfWeek.Wednesday, "Quarta-feira" },
        { DayOfWeek.Thursday,  "Quinta-feira" },
        { DayOfWeek.Friday,    "Sexta-feira" },
        { DayOfWeek.Saturday,  "Sábado" }
    };

        public static string GetNome(DayOfWeek dia) => Nomes[dia];
    }
}

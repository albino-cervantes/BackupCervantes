using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultaIBPT
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IBPT.Api.Client.ExemploUso.ExemploConsultaProduto().GetAwaiter().GetResult();
            Console.ReadLine();
        }
    }
}

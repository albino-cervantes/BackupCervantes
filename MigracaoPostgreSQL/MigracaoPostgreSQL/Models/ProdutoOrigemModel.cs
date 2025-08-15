using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigracaoPostgreSQL.Models
{
    /// <summary>
    /// Modelo para dados brutos vindos das tabelas de origem
    /// </summary>
    public class ProdutoOrigemModel
    {
        public string CodigoBarra { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Marca { get; set; }
        public string Departamento { get; set; }
        public string Categoria { get; set; }
        public string SubCategoria { get; set; }
        public string SubCategoria2 { get; set; }
        public string Ncm { get; set; }
        public string Cest { get; set; }
        public string FotoJpg { get; set; }
        public string FotoJpg580 { get; set; }
        public string FotoWebp { get; set; }
        public string TabelaOrigem { get; set; }
        public string Peso { get; set; }
        public string Dimensoes { get; set; }
        public string Caracteristicas { get; set; }
        public string DescricaoCurta { get; set; }
        public string FichaTecnica { get; set; }
        public decimal? PrecoMedio { get; set; }
    }
}

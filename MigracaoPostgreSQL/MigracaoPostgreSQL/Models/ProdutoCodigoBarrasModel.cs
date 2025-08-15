using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigracaoPostgreSQL.Models
{
    /// <summary>
    /// Modelo para código de barras do produto
    /// </summary>
    public class ProdutoCodigoBarrasModel
    {
        public long IdProdutoCodigoBarras { get; set; }
        public string CodigoBarra { get; set; }
        public long IdProduto { get; set; }
    }
}

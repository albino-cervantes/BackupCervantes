using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigracaoPostgreSQL.Models
{
    /// <summary>
    /// Modelo para fotos do produto
    /// </summary>
    public class ProdutoFotoModel
    {
        public long IdProdutoFoto { get; set; }
        public byte[] Foto { get; set; }
        public long IdProduto { get; set; }
    }
}

// ================================
// Models/ProdutoImportacao.cs
// ================================
namespace MigrationApp.Models
{
    public class ProdutoImportacao
    {
        public string CodigoBarra { get; set; }
        public string Descricao { get; set; }
        public string Grupo { get; set; }
        public string Subgrupo { get; set; }
        public string Marca { get; set; }
        public string NCM { get; set; }
        public string CEST { get; set; }
        public string Foto { get; set; } // caminho do arquivo da foto
        public string Origem { get; set; } // nome da tabela de origem
    }
}
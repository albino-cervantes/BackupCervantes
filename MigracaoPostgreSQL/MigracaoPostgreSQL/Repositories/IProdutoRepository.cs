using MigracaoPostgreSQL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigracaoPostgreSQL.Repositories
{
    /// <summary>
    /// Interface específica para repositório de produtos
    /// </summary>
    public interface IProdutoRepository : IRepository<ProdutoModel>
    {
        Task<ProdutoModel> GetByCodigoBarraAsync(string codigoBarra);
        Task<bool> HasPendingIcmsAsync(long idProduto);
        Task<List<ProdutoOrigemModel>> GetProdutosFromOrigemAsync(string database, string tabela);
        Task<bool> InsertWithRelatedDataAsync(ProdutoModel produto);
        Task<bool> UpdateWithValidationAsync(ProdutoModel produto);
    }
}

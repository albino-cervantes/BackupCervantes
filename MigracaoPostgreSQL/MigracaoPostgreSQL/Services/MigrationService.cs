using MigracaoPostgreSQL.Models;
using MigracaoPostgreSQL.Repositories;
using MigracaoPostgreSQL.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigracaoPostgreSQL.Services
{
    /// <summary>
    /// Serviço principal responsável pela migração de dados
    /// </summary>
    public class MigrationService : IMigrationService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly Logger _logger;
        private readonly DatabaseConfig _dbConfig;
        private const int BATCH_SIZE = 500;

        // Configuração das tabelas de origem
        private readonly Dictionary<string, string> _tabelasOrigem = new Dictionary<string, string>
        {
            { "tbl_autopecas_postgresql", "ean_202501.tbl_autopecas" },
            { "tbl_construcao_2025_postgresql", "ean_202501.tbl_construcao_2025" },
            { "tbl_eans_1555_postgresql", "tbl_eans_1555_cadastro" },
            { "tbl_petshop_ecommerce_2025_postgresql", "tbl_petshop_ecommerce_2025" }
        };

        public MigrationService(Logger logger)
        {
            _logger = logger;
            _dbConfig = new DatabaseConfig();
            _produtoRepository = new ProdutoRepository(_dbConfig, _logger);
        }

        public async Task ExecuteMigrationAsync()
        {
            var estatisticas = new MigrationStatistics();

            try
            {
                _logger.LogInfo("Iniciando processo de migração de dados");

                foreach (var origem in _tabelasOrigem)
                {
                    await ProcessOriginTableAsync(origem.Key, origem.Value, estatisticas);
                }

                LogFinalStatistics(estatisticas);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro crítico durante a migração: {ex.Message}", ex);
                throw;
            }
        }

        private async Task ProcessOriginTableAsync(string database, string tabela, MigrationStatistics stats)
        {
            try
            {
                _logger.LogInfo($"Processando banco: {database} - Tabela: {tabela}");

                // Carregar produtos da origem
                var produtosOrigem = await _produtoRepository.GetProdutosFromOrigemAsync(database, tabela);

                if (!produtosOrigem.Any())
                {
                    _logger.LogWarning($"Nenhum produto encontrado na tabela {tabela}");
                    return;
                }

                // Processar em lotes para melhor performance
                var batches = CreateBatches(produtosOrigem, BATCH_SIZE);

                foreach (var batch in batches)
                {
                    await ProcessBatchAsync(batch, tabela, stats);
                }

                _logger.LogInfo($"Tabela {tabela} processada. Total: {produtosOrigem.Count} produtos");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao processar tabela {database}.{tabela}: {ex.Message}", ex);
                stats.TablesWithErrors.Add($"{database}.{tabela}");
            }
        }

        private async Task ProcessBatchAsync(List<ProdutoOrigemModel> batch, string tabelaOrigem, MigrationStatistics stats)
        {
            try
            {
                _logger.LogInfo($"Processando lote de {batch.Count} produtos da tabela {tabelaOrigem}");

                foreach (var produtoOrigem in batch)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(produtoOrigem.CodigoBarra))
                        {
                            _logger.LogWarning($"Produto sem código de barras ignorado na tabela {tabelaOrigem}");
                            stats.ProductsSkipped++;
                            continue;
                        }

                        await ProcessSingleProductAsync(produtoOrigem, stats);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Erro ao processar produto {produtoOrigem.CodigoBarra} da tabela {tabelaOrigem}: {ex.Message}", ex);
                        stats.ProductsWithError++;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao processar lote da tabela {tabelaOrigem}: {ex.Message}", ex);
                stats.BatchesWithError++;
            }
        }

        private async Task ProcessSingleProductAsync(ProdutoOrigemModel produtoOrigem, MigrationStatistics stats)
        {
            // Verificar se produto já existe
            var produtoExistente = await _produtoRepository.GetByCodigoBarraAsync(produtoOrigem.CodigoBarra);

            if (produtoExistente == null)
            {
                // Produto não existe - criar novo
                var novoProduto = MapToDestinationProduct(produtoOrigem);

                if (await _produtoRepository.InsertWithRelatedDataAsync(novoProduto))
                {
                    _logger.LogInfo($"Produto criado: {produtoOrigem.CodigoBarra} - {produtoOrigem.Nome}");
                    stats.ProductsCreated++;
                }
                else
                {
                    _logger.LogError($"Falha ao criar produto: {produtoOrigem.CodigoBarra}");
                    stats.ProductsWithError++;
                }
            }
            else
            {
                // Produto existe - verificar se deve ser atualizado
                if (await _produtoRepository.HasPendingIcmsAsync(produtoExistente.IdProduto))
                {
                    var produtoAtualizado = MapToDestinationProduct(produtoOrigem);
                    produtoAtualizado.IdProduto = produtoExistente.IdProduto;

                    if (await _produtoRepository.UpdateWithValidationAsync(produtoAtualizado))
                    {
                        _logger.LogInfo($"Produto atualizado: {produtoOrigem.CodigoBarra} - {produtoOrigem.Nome}");
                        stats.ProductsUpdated++;
                    }
                    else
                    {
                        _logger.LogError($"Falha ao atualizar produto: {produtoOrigem.CodigoBarra}");
                        stats.ProductsWithError++;
                    }
                }
                else
                {
                    _logger.LogInfo($"Produto ignorado (sem ICMS pendente): {produtoOrigem.CodigoBarra}");
                    stats.ProductsSkipped++;
                }
            }
        }

        private ProdutoModel MapToDestinationProduct(ProdutoOrigemModel origem)
        {
            var produto = new ProdutoModel
            {
                Descricao = !string.IsNullOrWhiteSpace(origem.Nome) ? origem.Nome :
                           !string.IsNullOrWhiteSpace(origem.Descricao) ? origem.Descricao : "Produto sem descrição",
                Unidade = "UN",
                UnidadeDescricao = "Unidade",
                NumeroCasasDecimais = 0,
                Grupo = !string.IsNullOrWhiteSpace(origem.Departamento) ? origem.Departamento : "Sem Grupo",
                Subgrupo = GetSubgrupo(origem),
                Marca = origem.Marca,
                Ncm = origem.Ncm,
                Cest = origem.Cest,
                Marcador = false,
                UltimaOrigem = origem.TabelaOrigem
            };

            // Adicionar código de barras
            if (!string.IsNullOrWhiteSpace(origem.CodigoBarra))
            {
                produto.CodigosBarras.Add(origem.CodigoBarra);
            }

            // Adicionar fotos
            //AddPhotosPaths(produto, origem);

            return produto;
        }

        private string GetSubgrupo(ProdutoOrigemModel origem)
        {
            var subgrupos = new List<string>();

            if (!string.IsNullOrWhiteSpace(origem.Categoria))
                subgrupos.Add(origem.Categoria);

            if (!string.IsNullOrWhiteSpace(origem.SubCategoria))
                subgrupos.Add(origem.SubCategoria);

            if (!string.IsNullOrWhiteSpace(origem.SubCategoria2))
                subgrupos.Add(origem.SubCategoria2);

            return subgrupos.Any() ? string.Join(" > ", subgrupos) : "Sem Subgrupo";
        }

        private void AddPhotosPaths(ProdutoModel produto, ProdutoOrigemModel origem)
        {
            var fotos = new List<string> { origem.FotoJpg, origem.FotoJpg580, origem.FotoWebp };

            foreach (var foto in fotos)
            {
                if (!string.IsNullOrWhiteSpace(foto))
                {
                    produto.FotosPath.Add(foto);
                }
            }
        }

        private List<List<T>> CreateBatches<T>(List<T> source, int batchSize)
        {
            var batches = new List<List<T>>();

            for (int i = 0; i < source.Count; i += batchSize)
            {
                batches.Add(source.Skip(i).Take(batchSize).ToList());
            }

            return batches;
        }

        private void LogFinalStatistics(MigrationStatistics stats)
        {
            _logger.LogInfo("=== ESTATÍSTICAS FINAIS DA MIGRAÇÃO ===");
            _logger.LogInfo($"Produtos criados: {stats.ProductsCreated}");
            _logger.LogInfo($"Produtos atualizados: {stats.ProductsUpdated}");
            _logger.LogInfo($"Produtos ignorados: {stats.ProductsSkipped}");
            _logger.LogInfo($"Produtos com erro: {stats.ProductsWithError}");
            _logger.LogInfo($"Lotes com erro: {stats.BatchesWithError}");
            _logger.LogInfo($"Total processado: {stats.ProductsCreated + stats.ProductsUpdated + stats.ProductsSkipped + stats.ProductsWithError}");

            if (stats.TablesWithErrors.Any())
            {
                _logger.LogWarning($"Tabelas com erro: {string.Join(", ", stats.TablesWithErrors)}");
            }
        }
    }
}

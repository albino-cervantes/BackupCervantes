using MigracaoPostgreSQL.Models;
using MigracaoPostgreSQL.Services;
using MigracaoPostgreSQL.Utils;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigracaoPostgreSQL.Repositories
{
    /// <summary>
    /// Repositório para operações com produtos
    /// </summary>
    public class ProdutoRepository : BaseRepository, IProdutoRepository
    {
        private const string TBL_AUTOPECAS = "ean_202501.tbl_autopecas";
        private const string TBL_CONSTRUCAO = "ean_202501.tbl_construcao_2025";
        private const string TBL_EANS_1555_CADASTRO = "tbl_eans_1555_cadastro";
        private const string TBL_PETSHOP_ECOMMERCE = "tbl_petshop_ecommerce_2025";
        private readonly DatabaseConfig _dbConfig;

        public ProdutoRepository(DatabaseConfig dbConfig, Logger logger)
            : base(dbConfig.GetDestinationConnectionString(), logger)
        {
            _dbConfig = dbConfig;
        }

        public async Task<List<ProdutoModel>> GetAllAsync()
        {
            var produtos = new List<ProdutoModel>();

            try
            {
                using (var connection = CreateConnection())
                {
                    await connection.OpenAsync();

                    var query = @"
                        SELECT id_produto, descricao, unidade, unidade_descricao, 
                               numero_casas_decimais, grupo, subgrupo, marca, 
                               cst_pis_cofins, ncm, ex_tipi, cest, marcador, ultima_origem
                        FROM produto";

                    using (var command = new NpgsqlCommand(query, connection))
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            produtos.Add(MapToProdutoModel(reader));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao buscar todos os produtos: {ex.Message}", ex);
            }

            return produtos;
        }

        public async Task<ProdutoModel> GetByIdAsync(long id)
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    await connection.OpenAsync();

                    var query = @"
                        SELECT id_produto, descricao, unidade, unidade_descricao, 
                               numero_casas_decimais, grupo, subgrupo, marca, 
                               cst_pis_cofins, ncm, ex_tipi, cest, marcador, ultima_origem
                        FROM produto 
                        WHERE id_produto = @id";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return MapToProdutoModel(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao buscar produto por ID {id}: {ex.Message}", ex);
            }

            return null;
        }

        public async Task<ProdutoModel> GetByCodigoBarraAsync(string codigoBarra)
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    await connection.OpenAsync();

                    var query = @"
                        SELECT p.id_produto, p.descricao, p.unidade, p.unidade_descricao, 
                               p.numero_casas_decimais, p.grupo, p.subgrupo, p.marca, 
                               p.cst_pis_cofins, p.ncm, p.ex_tipi, p.cest, p.marcador, p.ultima_origem
                        FROM produto p
                        INNER JOIN produto_codigo_barras pcb ON p.id_produto = pcb.id_produto
                        WHERE pcb.codigo_barra = @codigoBarra";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@codigoBarra", codigoBarra);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return MapToProdutoModel(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao buscar produto por código de barras {codigoBarra}: {ex.Message}", ex);
            }

            return null;
        }

        public async Task<bool> HasPendingIcmsAsync(long idProduto)
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    await connection.OpenAsync();

                    var query = @"
                        SELECT COUNT(*) 
                        FROM produto_icms_uf 
                        WHERE id_produto = @idProduto AND data_hora_conferencia IS NULL";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@idProduto", idProduto);

                        var count = Convert.ToInt32(await command.ExecuteScalarAsync());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao verificar ICMS pendente para produto {idProduto}: {ex.Message}", ex);
                return false;
            }
        }

        public async Task<List<ProdutoOrigemModel>> GetProdutosFromOrigemAsync(string database, string tabela)
        {
            var produtos = new List<ProdutoOrigemModel>();

            try
            {
                var connectionString = _dbConfig.GetOriginConnectionString(database);

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var query = BuildOriginQuery(tabela);

                    using (var command = new NpgsqlCommand(query, connection))
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var produto = MapFromOrigin(reader, tabela);
                            if (produto != null)
                            {
                                produtos.Add(produto);
                            }
                        }
                    }
                }

                _logger.LogInfo($"Carregados {produtos.Count} produtos da tabela {tabela}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao carregar produtos da origem {database}.{tabela}: {ex.Message}", ex);
            }

            return produtos;
        }

        public async Task<bool> InsertAsync(ProdutoModel entity)
        {
            return await ExecuteWithTransactionAsync(async (connection, transaction) =>
            {
                var query = @"
                    INSERT INTO produto (descricao, unidade, unidade_descricao, numero_casas_decimais, 
                                       grupo, subgrupo, marca, cst_pis_cofins, ncm, ex_tipi, cest, 
                                       marcador, ultima_origem)
                    VALUES (@descricao, @unidade, @unidade_descricao, @numero_casas_decimais, 
                            @grupo, @subgrupo, @marca, @cst_pis_cofins, @ncm, @ex_tipi, @cest, 
                            @marcador, @ultima_origem)
                    RETURNING id_produto";

                using (var command = new NpgsqlCommand(query, connection, transaction))
                {
                    AddProductParameters(command, entity);

                    var result = await command.ExecuteScalarAsync();
                    entity.IdProduto = Convert.ToInt64(result);

                    return entity.IdProduto > 0;
                }
            });
        }

        public async Task<bool> UpdateAsync(ProdutoModel entity)
        {
            return await ExecuteWithTransactionAsync(async (connection, transaction) =>
            {
                var query = @"
                    UPDATE produto 
                    SET descricao = @descricao, unidade = @unidade, unidade_descricao = @unidade_descricao,
                        numero_casas_decimais = @numero_casas_decimais, grupo = @grupo, subgrupo = @subgrupo,
                        marca = @marca, cst_pis_cofins = @cst_pis_cofins, ncm = @ncm, ex_tipi = @ex_tipi,
                        cest = @cest, marcador = @marcador, ultima_origem = @ultima_origem
                    WHERE id_produto = @id_produto";

                using (var command = new NpgsqlCommand(query, connection, transaction))
                {
                    AddProductParameters(command, entity);
                    command.Parameters.AddWithValue("@id_produto", entity.IdProduto);

                    var rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            });
        }

        public async Task<bool> DeleteAsync(long id)
        {
            return await ExecuteWithTransactionAsync(async (connection, transaction) =>
            {
                var query = "DELETE FROM produto WHERE id_produto = @id";

                using (var command = new NpgsqlCommand(query, connection, transaction))
                {
                    command.Parameters.AddWithValue("@id", id);

                    var rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            });
        }

        public async Task<bool> InsertWithRelatedDataAsync(ProdutoModel produto)
        {
            return await ExecuteWithTransactionAsync(async (connection, transaction) =>
            {
                // Inserir produto
                var produtoQuery = @"
                    INSERT INTO produto (descricao, unidade, unidade_descricao, numero_casas_decimais, 
                                       grupo, subgrupo, marca, cst_pis_cofins, ncm, ex_tipi, cest, 
                                       marcador, ultima_origem)
                    VALUES (@descricao, @unidade, @unidade_descricao, @numero_casas_decimais, 
                            @grupo, @subgrupo, @marca, @cst_pis_cofins, @ncm, @ex_tipi, @cest, 
                            @marcador, @ultima_origem)
                    RETURNING id_produto";

                using (var command = new NpgsqlCommand(produtoQuery, connection, transaction))
                {
                    AddProductParameters(command, produto);
                    var result = await command.ExecuteScalarAsync();
                    produto.IdProduto = Convert.ToInt64(result);
                }

                // Inserir códigos de barras
                foreach (var codigoBarra in produto.CodigosBarras)
                {
                    if (!string.IsNullOrWhiteSpace(codigoBarra))
                    {
                        var codigoQuery = @"
                            INSERT INTO produto_codigo_barras (codigo_barra, id_produto)
                            VALUES (@codigo_barra, @id_produto)";

                        using (var command = new NpgsqlCommand(codigoQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@codigo_barra", codigoBarra);
                            command.Parameters.AddWithValue("@id_produto", produto.IdProduto);
                            await command.ExecuteNonQueryAsync();
                        }
                    }
                }

                // Inserir fotos
                var photoService = new PhotoService(_logger);
                foreach (var fotoPath in produto.FotosPath)
                {
                    if (!string.IsNullOrWhiteSpace(fotoPath))
                    {
                        var fotoBytes = await photoService.LoadPhotoAsync(fotoPath);
                        if (fotoBytes != null)
                        {
                            var fotoQuery = @"
                                INSERT INTO produto_foto (foto, id_produto)
                                VALUES (@foto, @id_produto)";

                            using (var command = new NpgsqlCommand(fotoQuery, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@foto", fotoBytes);
                                command.Parameters.AddWithValue("@id_produto", produto.IdProduto);
                                await command.ExecuteNonQueryAsync();
                            }
                        }
                    }
                }

                return true;
            });
        }

        public async Task<bool> UpdateWithValidationAsync(ProdutoModel produto)
        {
            // Verificar se há ICMS pendente antes de atualizar
            if (await HasPendingIcmsAsync(produto.IdProduto))
            {
                return await UpdateAsync(produto);
            }

            _logger.LogInfo($"Produto {produto.IdProduto} não possui ICMS pendente. Atualização ignorada.");
            return true; // Considera sucesso pois não há erro, apenas condição não atendida
        }

        #region Private Methods

        private ProdutoModel MapToProdutoModel(NpgsqlDataReader reader)
        {
            return new ProdutoModel
            {
                IdProduto = reader.Get<Int64>("id_produto"),
                Descricao = reader.Get<string>("descricao"),
                Unidade = reader.Get<string>("unidade"),
                UnidadeDescricao = reader.Get<string>("unidade_descricao"),
                NumeroCasasDecimais = reader.Get<Int32>("numero_casas_decimais"),
                Grupo = reader.Get<string>("grupo"),
                Subgrupo = reader.Get<string>("subgrupo"),
                Marca = reader.Get<string>("marca"),
                CstPisCofins = reader.Get<string>("cst_pis_cofins"),
                Ncm = reader.Get<string>("ncm"),
                ExTipi = reader.Get<string>("ex_tipi"),
                Cest = reader.Get<string>("cest"),
                Marcador = !reader.Get<Boolean>("marcador"),
                UltimaOrigem = reader.Get<string>("ultima_origem")
            };
        }

        private void AddProductParameters(NpgsqlCommand command, ProdutoModel produto)
        {
            command.Parameters.AddWithValue("@descricao", produto.Descricao ?? string.Empty);
            command.Parameters.AddWithValue("@unidade", produto.Unidade ?? "UN");
            command.Parameters.AddWithValue("@unidade_descricao", produto.UnidadeDescricao ?? "Unidade");
            command.Parameters.AddWithValue("@numero_casas_decimais", produto.NumeroCasasDecimais);
            command.Parameters.AddWithValue("@grupo", produto.Grupo ?? string.Empty);
            command.Parameters.AddWithValue("@subgrupo", produto.Subgrupo ?? string.Empty);
            command.Parameters.AddWithValue("@marca", produto.Marca ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@cst_pis_cofins", produto.CstPisCofins ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@ncm", produto.Ncm ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@ex_tipi", produto.ExTipi ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@cest", produto.Cest ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@marcador", produto.Marcador);
            command.Parameters.AddWithValue("@ultima_origem", produto.UltimaOrigem ?? (object)DBNull.Value);
        }

        private string BuildOriginQuery(string tabela)
        {
            switch (tabela.ToLower())
            {
                case TBL_AUTOPECAS:
                    return $@"
                        SELECT sku, gtin, produto, fabricante, departamento, categoria, 
                               subcategoria, subcategoria_2, preco_medio, ncm, cest, 
                               foto_jpg, foto_jpg580, foto_webp, codigos, descricao
                        FROM {TBL_AUTOPECAS}";

                case TBL_CONSTRUCAO:
                    return $@"
                        SELECT id, codbar, produto, descricao, foto_webp, foto_jpg, 
                               foto_jpg580, marca, preco_medio, departamento, categoria, 
                               sub_categoria, caracteristicas, ncm, peso, dimensoes
                        FROM {TBL_CONSTRUCAO}";

                case TBL_EANS_1555_CADASTRO:
                    return $@"
                        SELECT sku, codbar, produto, produto_upper, produto_acento, peso, 
                               ncm, cest_codigo, embalagem, quantidade_embalagem, foto_webp, 
                               foto_jpg, foto_jpg580, preco_medio, marca, categoria, caracteristicas
                        FROM {TBL_EANS_1555_CADASTRO}";

                case TBL_PETSHOP_ECOMMERCE:
                    return $@"
                        SELECT gtin, nome, marca, departamento, categoria, sub_categoria, 
                               sub_categoria_2, foto_jpg, foto_webp, foto_jpg580, peso, 
                               peso_cubico, altura, largura, comprimento, descricao, 
                               descricao_curta, ficha_tecnica, preco, ncm
                        FROM {TBL_PETSHOP_ECOMMERCE}";

                default:
                    throw new ArgumentException($"Tabela não reconhecida: {tabela}");
            }
        }

        private ProdutoOrigemModel MapFromOrigin(NpgsqlDataReader reader, string tabela)
        {
            try
            {
                var produto = new ProdutoOrigemModel
                {
                    TabelaOrigem = tabela
                };

                switch (tabela.ToLower())
                {
                    case TBL_AUTOPECAS:
                        produto.CodigoBarra = GetSafeString(reader, "gtin");
                        produto.Nome = GetSafeString(reader, "produto");
                        produto.Descricao = GetSafeString(reader, "descricao");
                        produto.Marca = GetSafeString(reader, "fabricante");
                        produto.Departamento = GetSafeString(reader, "departamento");
                        produto.Categoria = GetSafeString(reader, "categoria");
                        produto.SubCategoria = GetSafeString(reader, "subcategoria");
                        produto.SubCategoria2 = GetSafeString(reader, "subcategoria_2");
                        produto.Ncm = GetSafeString(reader, "ncm");
                        produto.Cest = GetSafeString(reader, "cest");
                        produto.FotoJpg = GetSafeString(reader, "foto_jpg");
                        produto.FotoJpg580 = GetSafeString(reader, "foto_jpg580");
                        produto.FotoWebp = GetSafeString(reader, "foto_webp");
                        break;

                    case TBL_CONSTRUCAO:
                        produto.CodigoBarra = GetSafeString(reader, "codbar");
                        produto.Nome = GetSafeString(reader, "produto");
                        produto.Descricao = GetSafeString(reader, "descricao");
                        produto.Marca = GetSafeString(reader, "marca");
                        produto.Departamento = GetSafeString(reader, "departamento");
                        produto.Categoria = GetSafeString(reader, "categoria");
                        produto.SubCategoria = GetSafeString(reader, "sub_categoria");
                        produto.Ncm = GetSafeString(reader, "ncm");
                        produto.FotoJpg = GetSafeString(reader, "foto_jpg");
                        produto.FotoJpg580 = GetSafeString(reader, "foto_jpg580");
                        produto.FotoWebp = GetSafeString(reader, "foto_webp");
                        produto.Caracteristicas = GetSafeString(reader, "caracteristicas");
                        produto.Peso = GetSafeString(reader, "peso");
                        produto.Dimensoes = GetSafeString(reader, "dimensoes");
                        produto.PrecoMedio = GetSafeDecimal(reader, "preco_medio");
                        break;

                    case TBL_EANS_1555_CADASTRO:
                        produto.CodigoBarra = GetSafeString(reader, "codbar");
                        produto.Nome = GetSafeString(reader, "produto");
                        produto.Marca = GetSafeString(reader, "marca");
                        produto.Categoria = GetSafeString(reader, "categoria");
                        produto.Ncm = GetSafeString(reader, "ncm");
                        produto.Cest = GetSafeString(reader, "cest_codigo");
                        produto.FotoJpg = GetSafeString(reader, "foto_jpg");
                        produto.FotoJpg580 = GetSafeString(reader, "foto_jpg580");
                        produto.FotoWebp = GetSafeString(reader, "foto_webp");
                        produto.Caracteristicas = GetSafeString(reader, "caracteristicas");
                        produto.Peso = GetSafeString(reader, "peso");
                        produto.PrecoMedio = GetSafeDecimal(reader, "preco_medio");
                        break;

                    case TBL_PETSHOP_ECOMMERCE:
                        produto.CodigoBarra = GetSafeString(reader, "gtin");
                        produto.Nome = GetSafeString(reader, "nome");
                        produto.Marca = GetSafeString(reader, "marca");
                        produto.Departamento = GetSafeString(reader, "departamento");
                        produto.Categoria = GetSafeString(reader, "categoria");
                        produto.SubCategoria = GetSafeString(reader, "sub_categoria");
                        produto.SubCategoria2 = GetSafeString(reader, "sub_categoria_2");
                        produto.Ncm = GetSafeString(reader, "ncm");
                        produto.FotoJpg = GetSafeString(reader, "foto_jpg");
                        produto.FotoJpg580 = GetSafeString(reader, "foto_jpg580");
                        produto.FotoWebp = GetSafeString(reader, "foto_webp");
                        produto.Descricao = GetSafeString(reader, "descricao");
                        produto.DescricaoCurta = GetSafeString(reader, "descricao_curta");
                        produto.FichaTecnica = GetSafeString(reader, "ficha_tecnica");
                        produto.PrecoMedio = GetSafeDecimal(reader, "preco");
                        break;
                }

                return produto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao mapear produto da origem {tabela}: {ex.Message}", ex);
                return null;
            }
        }

        private string GetSafeString(NpgsqlDataReader reader, string columnName)
        {
            try
            {
                return reader.Get<string>(columnName, string.Empty);
            }
            catch
            {
                return string.Empty;
            }
        }

        private decimal? GetSafeDecimal(NpgsqlDataReader reader, string columnName)
        {
            try
            {
                return reader.Get<decimal?>(columnName, (decimal?)null);
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}

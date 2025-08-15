using MigrationApp.Models;
using MigrationApp.Utils;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MigrationApp.Services
{
    public class DestinationService
    {
        private readonly Logger _logger;
        private readonly string _connString = "Host=127.0.0.1;Port=5434;Username=postgres;Password=123;Database=consulta_produtos;Pooling=true";
        private readonly string _fotoBasePath = @"C:\\FotosProdutos\\";

        public DestinationService(Logger logger)
        {
            _logger = logger;
        }

        public void ProcessBatch(List<ProdutoImportacao> batch)
        {
            using (var conn = new NpgsqlConnection(_connString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (var prod in batch)
                        {
                            long idProduto = ObterOuInserirProduto(conn, prod);
                            InserirOuAtualizarCodigoBarras(conn, prod.CodigoBarra, idProduto);
                            InserirFoto(conn, prod.Foto, idProduto);
                        }
                        tran.Commit();
                    }
                    catch (Exception)
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }

        private long ObterOuInserirProduto(NpgsqlConnection conn, ProdutoImportacao prod)
        {
            // Verifica se já existe produto com esse código de barras e se tem produto_icms_uf com data_hora_conferencia IS NULL
            string sqlVerifica = @"
                SELECT p.id_produto FROM produto p
                JOIN produto_codigo_barras cb ON cb.id_produto = p.id_produto
                LEFT JOIN produto_icms_uf icms ON icms.id_produto = p.id_produto
                WHERE cb.codigo_barra = @codigo_barra
                AND (icms.data_hora_conferencia IS NULL OR icms.id_produto_icms_uf IS NULL)
                LIMIT 1;";

            using (var cmd = new NpgsqlCommand(sqlVerifica, conn))
            {
                cmd.Parameters.AddWithValue("codigo_barra", prod.CodigoBarra);
                var result = cmd.ExecuteScalar();
                if (result != null)
                {
                    long idExistente = (long)result;

                    // Atualiza produto
                    string sqlUpdate = @"
                        UPDATE produto SET descricao=@descricao, unidade='UN', unidade_descricao='Unidade', numero_casas_decimais=0, grupo=@grupo, subgrupo=@subgrupo, marca=@marca, ncm=@ncm, cest=@cest, ultima_origem=@origem
                        WHERE id_produto = @id;";

                    using (var upd = new NpgsqlCommand(sqlUpdate, conn))
                    {
                        upd.Parameters.AddWithValue("descricao", prod.Descricao ?? string.Empty);
                        upd.Parameters.AddWithValue("grupo", prod.Grupo ?? string.Empty);
                        upd.Parameters.AddWithValue("subgrupo", prod.Subgrupo ?? string.Empty);
                        upd.Parameters.AddWithValue("marca", prod.Marca ?? string.Empty);
                        upd.Parameters.AddWithValue("ncm", prod.NCM ?? string.Empty);
                        upd.Parameters.AddWithValue("cest", prod.CEST ?? string.Empty);
                        upd.Parameters.AddWithValue("origem", prod.Origem ?? string.Empty);
                        upd.Parameters.AddWithValue("id", idExistente);
                        upd.ExecuteNonQuery();
                    }

                    return idExistente;
                }
                else
                {
                    // Insere produto novo
                    string sqlInsert = @"
                        INSERT INTO produto (descricao, unidade, unidade_descricao, numero_casas_decimais, grupo, subgrupo, marca, ncm, cest, ultima_origem)
                        VALUES (@descricao, 'UN', 'Unidade', 0, @grupo, @subgrupo, @marca, @ncm, @cest, @origem)
                        RETURNING id_produto;";

                    using (var ins = new NpgsqlCommand(sqlInsert, conn))
                    {
                        ins.Parameters.AddWithValue("descricao", prod.Descricao ?? string.Empty);
                        ins.Parameters.AddWithValue("grupo", prod.Grupo ?? string.Empty);
                        ins.Parameters.AddWithValue("subgrupo", prod.Subgrupo ?? string.Empty);
                        ins.Parameters.AddWithValue("marca", prod.Marca ?? string.Empty);
                        ins.Parameters.AddWithValue("ncm", prod.NCM ?? string.Empty);
                        ins.Parameters.AddWithValue("cest", prod.CEST ?? string.Empty);
                        ins.Parameters.AddWithValue("origem", prod.Origem ?? string.Empty);
                        return (long)ins.ExecuteScalar();
                    }
                }
            }
        }

        private void InserirOuAtualizarCodigoBarras(NpgsqlConnection conn, string codigoBarra, long idProduto)
        {
            string sql = @"
                INSERT INTO produto_codigo_barras (codigo_barra, id_produto)
                VALUES (@codigo, @id)
                ON CONFLICT (codigo_barra) DO NOTHING;";

            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("codigo", codigoBarra);
                cmd.Parameters.AddWithValue("id", idProduto);
                cmd.ExecuteNonQuery();
            }
        }

        private void InserirFoto(NpgsqlConnection conn, string fotoFileName, long idProduto)
        {
            if (string.IsNullOrWhiteSpace(fotoFileName)) return;
            string fullPath = Path.Combine(_fotoBasePath, fotoFileName);
            if (!File.Exists(fullPath)) return;

            byte[] bytes = File.ReadAllBytes(fullPath);
            string sql = @"
                INSERT INTO produto_foto (foto, id_produto)
                VALUES (@foto, @id);";

            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("foto", bytes);
                cmd.Parameters.AddWithValue("id", idProduto);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
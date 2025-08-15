using MigrationApp.Models;
using MigrationApp.Services;
using MigrationApp.Utils;
using Npgsql;
using System.Collections.Generic;

namespace MigrationApp.Repositories
{
    public class AutopecasRepository : IOriginRepository
    {
        private readonly Logger _logger;
        private readonly string _connString = "Host=127.0.0.1;Port=5434;Username=postgres;Password=123;Database=tbl_autopecas_postgresql;Pooling=true";

        public string Name => "tbl_autopecas_postgresql";

        public AutopecasRepository(Logger logger)
        {
            _logger = logger;
        }

        public IEnumerable<List<ProdutoImportacao>> GetBatches(int batchSize)
        {
            var lista = new List<ProdutoImportacao>();

            using (var conn = new NpgsqlConnection(_connString))
            {
                conn.Open();
                string sql = "SELECT gtin, produto, fabricante, departamento, categoria, subcategoria, subcategoria_2, preco_medio, ncm, cest, foto_jpg, foto_jpg580, foto_webp FROM ean_202501.tbl_autopecas";

                using (var cmd = new NpgsqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new ProdutoImportacao
                        {
                            CodigoBarra = reader["gtin"].ToString(),
                            Descricao = reader["produto"].ToString(),
                            Marca = reader["fabricante"].ToString(),
                            Grupo = reader["departamento"].ToString(),
                            Subgrupo = reader["subcategoria"].ToString(),
                            NCM = reader["ncm"].ToString(),
                            CEST = reader["cest"].ToString(),
                            Foto = reader["foto_jpg"].ToString() ?? reader["foto_jpg580"].ToString() ?? reader["foto_webp"].ToString(),
                            Origem = Name
                        });

                        if (lista.Count >= batchSize)
                        {
                            yield return lista;
                            lista = new List<ProdutoImportacao>();
                        }
                    }
                }
            }
            if (lista.Count > 0) yield return lista;
        }
    }
}
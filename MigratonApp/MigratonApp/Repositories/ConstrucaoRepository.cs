using MigrationApp.Models;
using MigrationApp.Services;
using MigrationApp.Utils;
using Npgsql;
using System.Collections.Generic;

namespace MigrationApp.Repositories
{
    public class ConstrucaoRepository : IOriginRepository
    {
        private readonly Logger _logger;
        private readonly string _connString = "Host=127.0.0.1;Port=5434;Username=postgres;Password=123;Database=tbl_construcao_2025_postgresql;Pooling=true";

        public string Name => "tbl_construcao_2025_postgresql";

        public ConstrucaoRepository(Logger logger)
        {
            _logger = logger;
        }

        public IEnumerable<List<ProdutoImportacao>> GetBatches(int batchSize)
        {
            var lista = new List<ProdutoImportacao>();

            using (var conn = new NpgsqlConnection(_connString))
            {
                conn.Open();
                string sql = "SELECT codbar, produto, marca, departamento, categoria, sub_categoria, ncm, foto_jpg, foto_webp, foto_jpg580 FROM ean_202501.tbl_construcao_2025";

                using (var cmd = new NpgsqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new ProdutoImportacao
                        {
                            CodigoBarra = reader["codbar"].ToString(),
                            Descricao = reader["produto"].ToString(),
                            Marca = reader["marca"].ToString(),
                            Grupo = reader["departamento"].ToString(),
                            Subgrupo = reader["sub_categoria"].ToString(),
                            NCM = reader["ncm"].ToString(),
                            Foto = reader["foto_jpg"].ToString() ?? reader["foto_webp"].ToString() ?? reader["foto_jpg580"].ToString(),
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
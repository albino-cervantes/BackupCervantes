using MigrationApp.Models;
using MigrationApp.Services;
using MigrationApp.Utils;
using Npgsql;
using System.Collections.Generic;

namespace MigrationApp.Repositories
{
    public class EansRepository : IOriginRepository
    {
        private readonly Logger _logger;
        private readonly string _connString = "Host=127.0.0.1;Port=5434;Username=postgres;Password=123;Database=tbl_eans_1555_postgresql;Pooling=true";

        public string Name => "tbl_eans_1555_postgresql";

        public EansRepository(Logger logger)
        {
            _logger = logger;
        }

        public IEnumerable<List<ProdutoImportacao>> GetBatches(int batchSize)
        {
            var lista = new List<ProdutoImportacao>();

            using (var conn = new NpgsqlConnection(_connString))
            {
                conn.Open();
                string sql = "SELECT codbar, produto, marca, categoria, ncm, cest_codigo, foto_jpg, foto_webp, foto_jpg580 FROM public.tbl_eans_1555_cadastro";

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
                            Grupo = "", // Não possui campo 'departamento'
                            Subgrupo = reader["categoria"].ToString(),
                            NCM = reader["ncm"].ToString(),
                            CEST = reader["cest_codigo"].ToString(),
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
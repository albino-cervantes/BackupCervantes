using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class ClsDadosLink
    {
        private const string ConnectionString = "Host=127.0.0.1;Database=InkDB_1371453627465470124;Username=postgres;Password=cer_2011!";

        public static List<ClsDadosProduto> Get()
        {
            var dados = new List<ClsDadosProduto>();

            ExecutaScript(CriaFuncaoValidaCodigoBarraNoBanco());

            using (var connection = new Npgsql.NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new Npgsql.NpgsqlCommand(ConsultaSql(), connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id_produto = reader.GetOrdinal("id_produto");
                        var codigo_barra = reader.GetOrdinal("codigo_barra");
                        var descricao_produto = reader.GetOrdinal("descricao");
                        var cod_ncm = reader.GetOrdinal("cod_ncm");
                        var cest = reader.GetOrdinal("cest");

                        var produto = new ClsDadosProduto
                        {
                            IdProduto = reader.IsDBNull(id_produto) ? null : reader.GetInt64(id_produto).ToString(),
                            CodigoBarra = reader.IsDBNull(codigo_barra) ? null : reader.GetString(codigo_barra),
                            DescricaoProduto = reader.IsDBNull(descricao_produto) ? null : reader.GetString(descricao_produto),
                            Ncm = reader.IsDBNull(cod_ncm) ? null : reader.GetString(cod_ncm),
                            Cest = reader.IsDBNull(cest) ? null : reader.GetString(cest)
                        };
                        dados.Add(produto);
                    }
                }
            }

            ExecutaScript(ExcluiFuncaoValidaCodigoBarraNoBanco());

            return dados;
        }

        public static void ExecutaScript(string pConsultaSql)
        {
            using (var connection = new Npgsql.NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new Npgsql.NpgsqlCommand(pConsultaSql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }

        }

        private static string CriaFuncaoValidaCodigoBarraNoBanco()
        {
            return @"
            CREATE OR REPLACE FUNCTION public.fnc_is_codigo_barras_valido(codigo_barras character varying)
                RETURNS boolean AS
                $BODY$DECLARE 
	                somaPares INT := 0;
	                somaImpares INT := 0;
	                valor INT := 0;
	                resultado INT :=0;
	                digitoverificador INT :=0;
                BEGIN 
	                FOR i IN 1..(char_length(codigo_barras)-1)::INTEGER
	                LOOP
		
		                IF ((i % 2) = 1)
		                THEN
			                somaImpares := somaImpares + (substring(codigo_barras from i for 1))::INT;
		                ELSE 
			                somaPares := somaPares + (substring(codigo_barras from i for 1))::INT ;
		                END IF;

	                END LOOP;

	                resultado := somaImpares + (somaPares * 3); 	

	                digitoverificador := 10 - (resultado % 10);

	                RETURN digitoverificador = (substring(codigo_barras from char_length(codigo_barras) for 1))::INT;
	
                END;$BODY$
                  LANGUAGE plpgsql VOLATILE
                  COST 100;";
        }

        private static string ExcluiFuncaoValidaCodigoBarraNoBanco()
        {
            return @"
            DROP FUNCTION IF EXISTS public.fnc_is_codigo_barras_valido();";
        }

        private static string ConsultaSql()
        {
            return @"
                

                SELECT DISTINCT ON (id_produto) 
                    id_produto, 
                    codigo_barra,
                    descricao,
                    cod_ncm,
                    cest
                FROM public.prod_cod_barras
                LEFT JOIN produto USING (id_produto)
                WHERE fnc_is_codigo_barras_valido(codigo_barra) 
                ORDER BY id_produto, id_prod_cod_barras DESC;";
        }
    }

    public class ClsDadosProduto
    {
        public string IdProduto { get; set; }
        public string CodigoBarra { get; set; }
        public string DescricaoProduto { get; set; }
        public string Ncm { get; set; }
        public string Cest { get; set; }
    }
}

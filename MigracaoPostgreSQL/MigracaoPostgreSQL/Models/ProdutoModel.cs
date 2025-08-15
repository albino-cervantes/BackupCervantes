// Models/ProdutoModel.cs
using System;
using System.Collections.Generic;

namespace MigracaoPostgreSQL.Models
{
    /// <summary>
    /// Modelo representando um produto no sistema
    /// </summary>
    public class ProdutoModel
    {
        public Int64 IdProduto { get; set; }
        public string Descricao { get; set; }
        public string Unidade { get; set; } = "UN";
        public string UnidadeDescricao { get; set; } = "Unidade";
        public int NumeroCasasDecimais { get; set; } = 0;
        public string Grupo { get; set; }
        public string Subgrupo { get; set; }
        public string Marca { get; set; }
        public string CstPisCofins { get; set; }
        public string Ncm { get; set; }
        public string ExTipi { get; set; }
        public string Cest { get; set; }
        public bool Marcador { get; set; } = false;
        public string UltimaOrigem { get; set; }

        // Propriedades auxiliares para migração
        public List<string> CodigosBarras { get; set; } = new List<string>();
        public List<string> FotosPath { get; set; } = new List<string>();
    }
}
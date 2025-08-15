using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace IBPT.Api.Client
{
    ///// <summary>
    ///// Modelo de dados para representar a resposta da API IBPT para produtos
    ///// Baseado no XML de retorno da API: https://apidoni.ibpt.org.br/api/v1/produtos
    ///// </summary>
    //[XmlRoot("ProdutoDTO", Namespace = "http://schemas.datacontract.org/2004/07/Aplicacao.DTO")]
    //public class ProdutoDTO
    //{
    //    /// <summary>
    //    /// Chave única do produto no IBPT
    //    /// </summary>
    //    [XmlElement("Chave")]
    //    public string Chave { get; set; }

    //    /// <summary>
    //    /// Código NCM (Nomenclatura Comum do Mercosul) do produto
    //    /// </summary>
    //    [XmlElement("Codigo")]
    //    public string Codigo { get; set; }

    //    /// <summary>
    //    /// Descrição detalhada do produto conforme NCM
    //    /// </summary>
    //    [XmlElement("Descricao")]
    //    public string Descricao { get; set; }

    //    /// <summary>
    //    /// Número EX (Exceção) da tabela NCM
    //    /// </summary>
    //    [XmlElement("EX")]
    //    public int EX { get; set; }

    //    /// <summary>
    //    /// Percentual de tributos estaduais (ICMS)
    //    /// </summary>
    //    [XmlElement("Estadual")]
    //    public decimal Estadual { get; set; }

    //    /// <summary>
    //    /// Fonte dos dados tributários
    //    /// </summary>
    //    [XmlElement("Fonte")]
    //    public string Fonte { get; set; }

    //    /// <summary>
    //    /// Percentual de tributos para produtos importados
    //    /// </summary>
    //    [XmlElement("Importado")]
    //    public decimal Importado { get; set; }

    //    /// <summary>
    //    /// Percentual de tributos municipais (ISS)
    //    /// </summary>
    //    [XmlElement("Municipal")]
    //    public decimal Municipal { get; set; }

    //    /// <summary>
    //    /// Percentual de tributos federais (PIS/COFINS/IPI)
    //    /// </summary>
    //    [XmlElement("Nacional")]
    //    public decimal Nacional { get; set; }

    //    /// <summary>
    //    /// Tipo do produto (0 = Nacional, 1 = Importado)
    //    /// </summary>
    //    [XmlElement("Tipo")]
    //    public string Tipo { get; set; }

    //    /// <summary>
    //    /// Unidade Federativa (Estado) para cálculo dos tributos
    //    /// </summary>
    //    [XmlElement("UF")]
    //    public string UF { get; set; }

    //    /// <summary>
    //    /// Valor do produto informado na consulta
    //    /// </summary>
    //    [XmlElement("Valor")]
    //    public decimal Valor { get; set; }

    //    /// <summary>
    //    /// Valor em reais dos tributos estaduais
    //    /// </summary>
    //    [XmlElement("ValorTributoEstadual")]
    //    public decimal ValorTributoEstadual { get; set; }

    //    /// <summary>
    //    /// Valor em reais dos tributos para produtos importados
    //    /// </summary>
    //    [XmlElement("ValorTributoImportado")]
    //    public decimal ValorTributoImportado { get; set; }

    //    /// <summary>
    //    /// Valor em reais dos tributos municipais
    //    /// </summary>
    //    [XmlElement("ValorTributoMunicipal")]
    //    public decimal ValorTributoMunicipal { get; set; }

    //    /// <summary>
    //    /// Valor em reais dos tributos federais
    //    /// </summary>
    //    [XmlElement("ValorTributoNacional")]
    //    public decimal ValorTributoNacional { get; set; }

    //    /// <summary>
    //    /// Versão da tabela de tributos utilizada
    //    /// </summary>
    //    [XmlElement("Versao")]
    //    public string Versao { get; set; }

    //    /// <summary>
    //    /// Data final de vigência da tabela (formato dd/MM/yyyy)
    //    /// </summary>
    //    [XmlElement("VigenciaFim")]
    //    public string VigenciaFim { get; set; }

    //    /// <summary>
    //    /// Data inicial de vigência da tabela (formato dd/MM/yyyy)
    //    /// </summary>
    //    [XmlElement("VigenciaInicio")]
    //    public string VigenciaInicio { get; set; }
    //}

    public class ProdutoDTO
    {
        public string Chave { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public int EX { get; set; }
        public decimal Estadual { get; set; }
        public string Fonte { get; set; }
        public decimal Importado { get; set; }
        public decimal Municipal { get; set; }
        public decimal Nacional { get; set; }
        public int Tipo { get; set; }
        public string UF { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorTributoEstadual { get; set; }
        public decimal ValorTributoImportado { get; set; }
        public decimal ValorTributoMunicipal { get; set; }
        public decimal ValorTributoNacional { get; set; }
        public string Versao { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime VigenciaInicio { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime VigenciaFim { get; set; }
    }
}
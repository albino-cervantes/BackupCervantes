using System.Xml.Serialization;

namespace IBPT.Api.Client
{
    /// <summary>
    /// Modelo de dados para representar a resposta da API IBPT para serviços
    /// Estrutura similar ao ProdutoDTO, mas específica para serviços
    /// </summary>
    [XmlRoot("ServicoDTO", Namespace = "http://schemas.datacontract.org/2004/07/Aplicacao.DTO")]
    public class ServicoDTO
    {
        /// <summary>
        /// Chave única do serviço no IBPT
        /// </summary>
        [XmlElement("Chave")]
        public string Chave { get; set; }

        /// <summary>
        /// Código do serviço conforme lista de serviços municipal
        /// </summary>
        [XmlElement("Codigo")]
        public string Codigo { get; set; }

        /// <summary>
        /// Descrição detalhada do serviço
        /// </summary>
        [XmlElement("Descricao")]
        public string Descricao { get; set; }

        /// <summary>
        /// Percentual de tributos estaduais (normalmente 0 para serviços)
        /// </summary>
        [XmlElement("Estadual")]
        public decimal Estadual { get; set; }

        /// <summary>
        /// Fonte dos dados tributários
        /// </summary>
        [XmlElement("Fonte")]
        public string Fonte { get; set; }

        /// <summary>
        /// Percentual de tributos federais (PIS/COFINS)
        /// </summary>
        [XmlElement("Federal")]
        public decimal Federal { get; set; }

        /// <summary>
        /// Percentual de tributos municipais (ISS)
        /// </summary>
        [XmlElement("Municipal")]
        public decimal Municipal { get; set; }

        /// <summary>
        /// Unidade Federativa (Estado)
        /// </summary>
        [XmlElement("UF")]
        public string UF { get; set; }

        /// <summary>
        /// Valor do serviço informado na consulta
        /// </summary>
        [XmlElement("Valor")]
        public decimal Valor { get; set; }

        /// <summary>
        /// Valor em reais dos tributos federais
        /// </summary>
        [XmlElement("ValorTributoFederal")]
        public decimal ValorTributoFederal { get; set; }

        /// <summary>
        /// Valor em reais dos tributos municipais
        /// </summary>
        [XmlElement("ValorTributoMunicipal")]
        public decimal ValorTributoMunicipal { get; set; }

        /// <summary>
        /// Versão da tabela de tributos utilizada
        /// </summary>
        [XmlElement("Versao")]
        public string Versao { get; set; }

        /// <summary>
        /// Data final de vigência da tabela (formato dd/MM/yyyy)
        /// </summary>
        [XmlElement("VigenciaFim")]
        public string VigenciaFim { get; set; }

        /// <summary>
        /// Data inicial de vigência da tabela (formato dd/MM/yyyy)
        /// </summary>
        [XmlElement("VigenciaInicio")]
        public string VigenciaInicio { get; set; }
    }
}
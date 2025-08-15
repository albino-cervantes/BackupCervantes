using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using System.IO;
using Newtonsoft.Json;

namespace IBPT.Api.Client
{
    /// <summary>
    /// Cliente principal para consulta da API IBPT (Instituto Brasileiro de Planejamento e Tributação)
    /// 
    /// Esta classe permite consultar informações sobre tributos de produtos e serviços
    /// através da API REST do IBPT. É necessário possuir CNPJ autorizado e token válido.
    /// 
    /// Documentação da API: https://ibpt.com.br/
    /// </summary>
    public class IbptClient : IDisposable
    {
        #region Campos Privados

        /// <summary>
        /// URL base da API IBPT
        /// </summary>
        private readonly string _baseUrl = "https://apidoni.ibpt.org.br/api/v1/";

        /// <summary>
        /// CNPJ autorizado para consulta na API IBPT
        /// </summary>
        private readonly string _cnpj;

        /// <summary>
        /// Token de autenticação fornecido pelo IBPT
        /// </summary>
        private readonly string _token;

        /// <summary>
        /// Serviço HTTP para realizar as requisições
        /// </summary>
        private readonly IHttpClientService _httpClientService;

        /// <summary>
        /// Indica se o objeto foi criado internamente (para controle de dispose)
        /// </summary>
        private readonly bool _httpClientCreatedInternally;

        /// <summary>
        /// Flag para controle de dispose
        /// </summary>
        private bool _disposed = false;

        #endregion

        #region Construtores

        /// <summary>
        /// Construtor principal da classe IbptClient
        /// </summary>
        /// <param name="cnpj">CNPJ autorizado para consulta (apenas números)</param>
        /// <param name="token">Token de autenticação fornecido pelo IBPT</param>
        /// <param name="httpClientService">Serviço HTTP personalizado (opcional)</param>
        /// <exception cref="ArgumentException">Lançada quando CNPJ ou token são inválidos</exception>
        public IbptClient(string cnpj, string token, IHttpClientService httpClientService = null)
        {
            // Validação dos parâmetros obrigatórios
            if (string.IsNullOrWhiteSpace(cnpj))
                throw new ArgumentException("CNPJ não pode ser nulo ou vazio", nameof(cnpj));

            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token não pode ser nulo ou vazio", nameof(token));

            // Remove caracteres não numéricos do CNPJ
            _cnpj = System.Text.RegularExpressions.Regex.Replace(cnpj, @"[^\d]", "");

            // Validação do formato do CNPJ (deve ter 14 dígitos)
            if (_cnpj.Length != 14)
                throw new ArgumentException("CNPJ deve conter exatamente 14 dígitos", nameof(cnpj));

            _token = token;

            // Inicializa o serviço HTTP
            if (httpClientService == null)
            {
                _httpClientService = new HttpClientService();
                _httpClientCreatedInternally = true;
            }
            else
            {
                _httpClientService = httpClientService;
                _httpClientCreatedInternally = false;
            }
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Consulta informações sobre tributos de produtos na API IBPT
        /// </summary>
        /// <param name="ncm">Código NCM do produto (8 dígitos)</param>
        /// <param name="uf">Sigla da Unidade Federativa (estado) - Ex: "RS", "SP"</param>
        /// <param name="exTarif">Número EX (exceção) da tabela NCM (padrão: 0)</param>
        /// <param name="descricao">Descrição do produto</param>
        /// <param name="unidadeMedida">Unidade de medida do produto - Ex: "un", "kg"</param>
        /// <param name="valor">Valor do produto para cálculo dos tributos</param>
        /// <param name="gtin">Código GTIN/EAN do produto (opcional)</param>
        /// <param name="codigoInterno">Código interno da empresa (opcional)</param>
        /// <returns>Objeto ProdutoDTO com informações tributárias</returns>
        /// <exception cref="ArgumentException">Lançada quando parâmetros obrigatórios são inválidos</exception>
        /// <exception cref="HttpRequestException">Lançada quando ocorre erro na requisição HTTP</exception>
        /// <exception cref="InvalidOperationException">Lançada quando a resposta não pode ser deserializada</exception>
        public async Task<ProdutoDTO> ConsultarTributosProdutoAsync(
            string ncm,
            string uf = "RS",
            int exTarif = 0,
            string descricao = "",
            string unidadeMedida = "un",
            decimal valor = 0,
            string gtin = "SEM GTIM",
            string codigoInterno = "")
        {
            // Validação dos parâmetros obrigatórios
            ValidarParametrosComuns(uf, valor);

            if (string.IsNullOrWhiteSpace(ncm))
                throw new ArgumentException("NCM não pode ser nulo ou vazio", nameof(ncm));

            // Remove caracteres não numéricos do NCM
            ncm = System.Text.RegularExpressions.Regex.Replace(ncm, @"[^\d]", "");

            if (ncm.Length != 8)
                throw new ArgumentException("NCM deve conter exatamente 8 dígitos", nameof(ncm));

            // Construção da URI da requisição
            var uriBuilder = new UriBuilder($"{_baseUrl}produtos");
            var query = HttpUtility.ParseQueryString(string.Empty);

            // Parâmetros obrigatórios
            query["token"] = _token;
            query["cnpj"] = _cnpj;
            query["codigo"] = ncm;
            query["uf"] = uf.ToUpper();
            query["ex"] = exTarif.ToString();
            query["valor"] = valor.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            query["descricao"] = descricao;
            query["gtin"] = gtin;

            // Parâmetros opcionais
            if (!string.IsNullOrWhiteSpace(codigoInterno))
                query["codigoInterno"] = codigoInterno;

            if (!string.IsNullOrWhiteSpace(unidadeMedida))
                query["unidadeMedida"] = unidadeMedida;
                             
            uriBuilder.Query = query.ToString();

            try
            {
                // Executa a requisição HTTP
                var jsonResponse = await _httpClientService.GetAsync(uriBuilder.ToString());

                // Deserializa a resposta XML
                //return DeserializarXml<ProdutoDTO>(xmlResponse);

                return JsonConvert.DeserializeObject<ProdutoDTO>(jsonResponse);
            }
            catch (Exception ex) when (!(ex is ArgumentException))
            {
                throw new InvalidOperationException($"Erro ao consultar tributos do produto NCM {ncm}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Consulta informações sobre tributos de serviços na API IBPT
        /// </summary>
        /// <param name="uf">Sigla da Unidade Federativa (estado) - Ex: "RS", "SP"</param>
        /// <param name="codigo">Código do serviço conforme lista municipal</param>
        /// <param name="descricao">Descrição do serviço</param>
        /// <param name="unidadeMedida">Unidade de medida do serviço - Ex: "un", "h"</param>
        /// <param name="valor">Valor do serviço para cálculo dos tributos</param>
        /// <returns>Objeto ServicoDTO com informações tributárias</returns>
        /// <exception cref="ArgumentException">Lançada quando parâmetros obrigatórios são inválidos</exception>
        /// <exception cref="HttpRequestException">Lançada quando ocorre erro na requisição HTTP</exception>
        /// <exception cref="InvalidOperationException">Lançada quando a resposta não pode ser deserializada</exception>
        public async Task<ServicoDTO> ConsultarTributosServicoAsync(
            string uf,
            string codigo,
            string descricao = "",
            string unidadeMedida = "un",
            decimal valor = 0)
        {
            // Validação dos parâmetros obrigatórios
            ValidarParametrosComuns(uf, valor);

            if (string.IsNullOrWhiteSpace(codigo))
                throw new ArgumentException("Código do serviço não pode ser nulo ou vazio", nameof(codigo));

            // Construção da URI da requisição
            var uriBuilder = new UriBuilder($"{_baseUrl}servicos");
            var query = HttpUtility.ParseQueryString(string.Empty);

            // Parâmetros obrigatórios
            query["token"] = _token;
            query["cnpj"] = _cnpj;
            query["codigo"] = codigo;
            query["uf"] = uf.ToUpper();
            query["valor"] = valor.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);

            // Parâmetros opcionais
            if (!string.IsNullOrWhiteSpace(descricao))
                query["descricao"] = descricao;

            if (!string.IsNullOrWhiteSpace(unidadeMedida))
                query["unidadeMedida"] = unidadeMedida;

            uriBuilder.Query = query.ToString();

            try
            {
                // Executa a requisição HTTP
                var xmlResponse = await _httpClientService.GetAsync(uriBuilder.ToString());

                // Deserializa a resposta XML
                return DeserializarXml<ServicoDTO>(xmlResponse);
            }
            catch (Exception ex) when (!(ex is ArgumentException))
            {
                throw new InvalidOperationException($"Erro ao consultar tributos do serviço código {codigo}: {ex.Message}", ex);
            }
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Valida parâmetros comuns a ambos os métodos de consulta
        /// </summary>
        /// <param name="uf">Unidade Federativa</param>
        /// <param name="valor">Valor do produto/serviço</param>
        /// <exception cref="ArgumentException">Lançada quando parâmetros são inválidos</exception>
        private static void ValidarParametrosComuns(string uf, decimal valor)
        {
            if (string.IsNullOrWhiteSpace(uf))
                throw new ArgumentException("UF não pode ser nula ou vazia", nameof(uf));

            if (uf.Length != 2)
                throw new ArgumentException("UF deve conter exatamente 2 caracteres", nameof(uf));

            if (valor < 0)
                throw new ArgumentException("Valor não pode ser negativo", nameof(valor));
        }

        /// <summary>
        /// Deserializa uma string XML para o tipo especificado
        /// </summary>
        /// <typeparam name="T">Tipo do objeto para deserialização</typeparam>
        /// <param name="xmlContent">Conteúdo XML como string</param>
        /// <returns>Objeto deserializado do tipo T</returns>
        /// <exception cref="InvalidOperationException">Lançada quando não é possível deserializar o XML</exception>
        private static T DeserializarXml<T>(string xmlContent) where T : class
        {
            try
            {
                // Remove o wrapper <retorno> se existir
                if (xmlContent.Contains("<retorno>"))
                {
                    var startTag = xmlContent.IndexOf("<retorno>") + "<retorno>".Length;
                    var endTag = xmlContent.IndexOf("</retorno>");
                    if (endTag > startTag)
                    {
                        xmlContent = xmlContent.Substring(startTag, endTag - startTag).Trim();
                    }
                }

                var serializer = new XmlSerializer(typeof(T));
                using (var reader = new StringReader(xmlContent))
                {
                    return (T)serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erro ao deserializar resposta XML: {ex.Message}", ex);
            }
        }

        #endregion

        #region IDisposable Implementation

        /// <summary>
        /// Libera os recursos utilizados pela classe
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Método protegido para liberação de recursos
        /// </summary>
        /// <param name="disposing">Indica se está sendo chamado pelo Dispose público</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                // Só faz dispose do HttpClient se foi criado internamente
                if (_httpClientCreatedInternally && _httpClientService is IDisposable disposable)
                {
                    disposable.Dispose();
                }
                _disposed = true;
            }
        }

        #endregion
    }
}
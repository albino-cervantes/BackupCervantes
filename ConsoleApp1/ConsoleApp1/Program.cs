using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;

using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ConsoleApp1
{
    // Classe para representar a requisição de consulta GTIN
    [XmlRoot("consGTIN", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class ConsGTIN
    {
        [XmlAttribute("versao")]
        public string Versao { get; set; } = "1.00";

        [XmlElement("GTIN")]
        public string GTIN { get; set; }
    }

    // Classe para representar a resposta da consulta GTIN
    [XmlRoot("retConsGTIN", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class RetConsGTIN
    {
        [XmlAttribute("versao")]
        public string Versao { get; set; }

        [XmlElement("verAplic")]
        public string VersaoAplicacao { get; set; }

        [XmlElement("cStat")]
        public string CodigoStatus { get; set; }

        [XmlElement("xMotivo")]
        public string Motivo { get; set; }

        [XmlElement("dhResp")]
        public DateTime DataHoraResposta { get; set; }

        [XmlElement("GTIN")]
        public string GTIN { get; set; }

        [XmlElement("tpGTIN")]
        public string TipoGTIN { get; set; }

        [XmlElement("xProd")]
        public string DescricaoProduto { get; set; }

        [XmlElement("NCM")]
        public string NCM { get; set; }

        [XmlElement("CEST")]
        public string[] CEST { get; set; }
    }

    // Classe principal para consumir o Web Service
    public class ConsultaGTINService
    {
        private const string ENDPOINT_URL = "https://dfe-servico.svrs.rs.gov.br/ws/ccgConsGTIN/ccgConsGTIN.asmx";
        private const string SOAP_ACTION = "http://www.portalfiscal.inf.br/nfe/wsdl/ccgConsGtin/ccgConsGTIN";
        private const string NAMESPACE = "http://www.portalfiscal.inf.br/nfe";

        private X509Certificate2 _certificado;

        public ConsultaGTINService(X509Certificate2 certificado)
        {
            _certificado = certificado ?? throw new ArgumentNullException(nameof(certificado));
        }

        public RetConsGTIN ConsultarGTIN(string gtin)
        {
            if (string.IsNullOrWhiteSpace(gtin))
                throw new ArgumentException("GTIN não pode ser vazio", nameof(gtin));

            // Validar formato do GTIN
            if (!ValidarGTIN(gtin))
                throw new ArgumentException("GTIN deve ter entre 6 e 14 dígitos", nameof(gtin));

            var requisicao = new ConsGTIN { GTIN = gtin };
            var xmlRequisicao = SerializarXml(requisicao);

            // Validar XML contra schema
            ValidarXMLContraSchema(xmlRequisicao);

            var xmlResposta = EnviarRequisicaoSOAP(xmlRequisicao);
            return DeserializarResposta(xmlResposta);
        }

        private bool ValidarGTIN(string gtin)
        {
            if (string.IsNullOrWhiteSpace(gtin))
                return false;

            // GTIN deve ter entre 6 e 14 dígitos numéricos
            return gtin.Length >= 6 && gtin.Length <= 14 &&
                   long.TryParse(gtin, out _);
        }

        private string SerializarXml(ConsGTIN requisicao)
        {
            var serializer = new XmlSerializer(typeof(ConsGTIN));
            var settings = new XmlWriterSettings
            {
                Indent = false,
                OmitXmlDeclaration = true,
                Encoding = Encoding.UTF8
            };

            using (var stringWriter = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
            {
                var namespaces = new XmlSerializerNamespaces();
                namespaces.Add("", NAMESPACE);

                serializer.Serialize(xmlWriter, requisicao, namespaces);
                return stringWriter.ToString();
            }
        }

        private void ValidarXMLContraSchema(string xml)
        {
            try
            {
                var doc = XDocument.Parse(xml);
                var schemas = new XmlSchemaSet();

                // Aqui você adicionaria os schemas XSD
                // schemas.Add(NAMESPACE, "caminho/para/consGTIN_v1.00.xsd");

                // Para este exemplo, fazemos uma validação básica
                var isValid = !string.IsNullOrEmpty(doc.Root?.Element(XName.Get("GTIN", NAMESPACE))?.Value);

                if (!isValid)
                    throw new XmlSchemaException("XML inválido - GTIN não encontrado");
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Erro na validação do XML: {ex.Message}", ex);
            }
        }

        private string EnviarRequisicaoSOAP(string xmlRequisicao)
        {
            var soapEnvelope = CriarEnvelopeSOAP(xmlRequisicao);

            var request = (HttpWebRequest)WebRequest.Create(ENDPOINT_URL);
            request.Method = "POST";
            request.ContentType = "application/soap+xml; charset=utf-8";
            request.Headers.Add("SOAPAction", SOAP_ACTION);

            // Configurar TLS 1.2 e certificado
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback =
                (sender, certificate, chain, errors) => true;

            if (_certificado != null)
            {
                request.ClientCertificates.Add(_certificado);
            }

            // Configurar compressão GZip
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip");
            request.AutomaticDecompression = DecompressionMethods.GZip;

            // Enviar requisição
            var data = Encoding.UTF8.GetBytes(soapEnvelope);
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            // Receber resposta
            using (var response = (HttpWebResponse)request.GetResponse())
            using (var responseStream = response.GetResponseStream())
            using (var reader = new StreamReader(responseStream, Encoding.UTF8))
            {
                var responseXml = reader.ReadToEnd();
                return ExtrairCorpoSOAP(responseXml);
            }
        }

        private string CriarEnvelopeSOAP(string xmlRequisicao)
        {
            return $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap12:Envelope xmlns:soap12=""http://www.w3.org/2003/05/soap-envelope"">
    <soap12:Body>
        <ccgConsGTIN xmlns=""http://www.portalfiscal.inf.br/nfe/wsdl/ccgConsGtin"">
            <nfeDadosMsg>{xmlRequisicao}</nfeDadosMsg>
        </ccgConsGTIN>
    </soap12:Body>
</soap12:Envelope>";
        }

        private string ExtrairCorpoSOAP(string soapResponse)
        {
            try
            {
                var doc = XDocument.Parse(soapResponse);
                var ns = XNamespace.Get("http://www.portalfiscal.inf.br/nfe/wsdl/ccgConsGtin");
                var soapNs = XNamespace.Get("http://www.w3.org/2003/05/soap-envelope");

                var resultElement = doc.Root?
                    .Element(soapNs + "Body")?
                    .Element(ns + "ccgConsGTINResponse")?
                    .Element(ns + "nfeResultMsg");

                if (resultElement != null)
                {
                    // Extrair o conteúdo interno do nfeResultMsg
                    return resultElement.FirstNode?.ToString() ?? string.Empty;
                }

                throw new InvalidOperationException("Resposta SOAP inválida");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erro ao processar resposta SOAP: {ex.Message}", ex);
            }
        }

        private RetConsGTIN DeserializarResposta(string xmlResposta)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(RetConsGTIN));
                using (var reader = new StringReader(xmlResposta))
                {
                    return (RetConsGTIN)serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erro ao deserializar resposta: {ex.Message}", ex);
            }
        }
    }

    // Classe utilitária para carregar certificados
    public static class CertificadoHelper
    {
        public static X509Certificate2 CarregarCertificado(string caminhoCertificado, string senha)
        {
            try
            {
                return new X509Certificate2(caminhoCertificado, senha, X509KeyStorageFlags.MachineKeySet);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Erro ao carregar certificado: {ex.Message}", ex);
            }
        }

        public static X509Certificate2 CarregarCertificadoDoStore(string thumbprint)
        {
            var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);

            try
            {
                var certificates = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);

                if (certificates.Count == 0)
                    throw new ArgumentException($"Certificado com thumbprint {thumbprint} não encontrado");

                return certificates[0];
            }
            finally
            {
                store.Close();
            }
        }
    }

    // Programa principal de exemplo
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("=== Consulta GTIN - Portal Fiscal ===");

                // Exemplo de uso
                //Console.Write("Digite o GTIN para consulta: ");
                var gtin = "7896022086282"; //Console.ReadLine();

                if (string.IsNullOrWhiteSpace(gtin))
                {
                    Console.WriteLine("GTIN não pode ser vazio.");
                    return;
                }

                // Carregar certificado (exemplo)
                // Substitua pelos dados do seu certificado
                //Console.Write("Caminho do certificado (.pfx): ");
                var caminhoCertificado = "C:\\Cervantes\\CERVANTES_-_TECNOLOGIA_LTDA08833101000155-123456.pfx"; //Console.ReadLine();

                Console.Write("Senha do certificado: ");
                var senha = "123456"; //LerSenhaSegura();

                var certificado = CertificadoHelper.CarregarCertificado(caminhoCertificado, senha);

                // Realizar consulta
                var service = new ConsultaGTINService(certificado);

                File.AppendAllText(
                    "Logs.txt",
                    "idproduto_Link;descricaoproduto_Link;gtin_Link;NCM_link;cest_link;ret_tipoGtin;ret_gtin;ret_descricao;ret_ncm;ret_cest"
                    + Environment.NewLine);

                ClsDadosLink.Get().ForEach(dados =>
                {
                    Console.WriteLine("\nRealizando consulta...");
                    var resultado = service.ConsultarGTIN(dados.CodigoBarra);


                    var a = 
                    $"{dados.IdProduto};{dados.DescricaoProduto};{dados.CodigoBarra};{dados.Ncm};{dados.Cest}";

                    var b = $"{resultado.TipoGTIN};{resultado.GTIN};{resultado.DescricaoProduto};{resultado.NCM};{resultado.CEST?.FirstOrDefault()}";

                    File.AppendAllText("Logs.txt",$"{a};{b}" + Environment.NewLine); // Log do resultado da consulta

                    Task.Delay(60000).Wait();
                });


                //Console.WriteLine("\nRealizando consulta...");
                //var resultado = service.ConsultarGTIN(gtin);

                //// Exibir resultado
                //Console.WriteLine("\n=== RESULTADO DA CONSULTA ===");
                //Console.WriteLine($"Status: {resultado.CodigoStatus} - {resultado.Motivo}");
                //Console.WriteLine($"Data/Hora: {resultado.DataHoraResposta}");

                //if (!string.IsNullOrEmpty(resultado.GTIN))
                //{
                //    Console.WriteLine($"GTIN: {resultado.GTIN}");
                //    Console.WriteLine($"Tipo GTIN: {resultado.TipoGTIN}");
                //    Console.WriteLine($"Produto: {resultado.DescricaoProduto}");
                //    Console.WriteLine($"NCM: {resultado.NCM}");

                //    if (resultado.CEST != null && resultado.CEST.Length > 0)
                //    {
                //        Console.WriteLine($"CEST: {string.Join(", ", resultado.CEST)}");
                //    }
                //}

                //Console.WriteLine($"Versão da aplicação: {resultado.VersaoAplicacao}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Detalhes: {ex.InnerException.Message}");
                }
            }

            Console.WriteLine("\nPressione qualquer tecla para sair...");
            Console.ReadKey();
        }

        private static string LerSenhaSegura()
        {
            var senha = new StringBuilder();
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    senha.Append(key.KeyChar);
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && senha.Length > 0)
                {
                    senha.Remove(senha.Length - 1, 1);
                    Console.Write("\b \b");
                }
            } while (key.Key != ConsoleKey.Enter);

            Console.WriteLine();
            return senha.ToString();
        }
    }
}
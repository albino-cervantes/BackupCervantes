using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace IBPT.Api.Client
{
    /// <summary>
    /// Implementação do serviço HTTP usando HttpClient
    /// </summary>
    public class HttpClientService : IHttpClientService, IDisposable
    {
        private readonly HttpClient _httpClient;
        private bool _disposed = false;

        /// <summary>
        /// Construtor que inicializa o HttpClient
        /// </summary>
        /// <param name="httpClient">Instância do HttpClient (opcional, para injeção de dependência)</param>
        public HttpClientService(HttpClient httpClient = null)
        {
            _httpClient = httpClient ?? new HttpClient();

            // Configurações padrão do HttpClient
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "IBPT-CSharp-Client/1.0");
        }

        /// <summary>
        /// Executa uma requisição HTTP GET assíncrona
        /// </summary>
        /// <param name="uri">URI completa para a requisição</param>
        /// <returns>Conteúdo da resposta como string</returns>
        /// <exception cref="HttpRequestException">Lançada quando ocorre erro na requisição HTTP</exception>
        public async Task<string> GetAsync(string uri)
        {
            try
            {
                var response = await _httpClient.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException($"Erro ao fazer requisição para {uri}: {ex.Message}", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new TimeoutException($"Timeout na requisição para {uri}", ex);
            }
        }

        /// <summary>
        /// Libera os recursos utilizados pelo HttpClient
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
                _httpClient?.Dispose();
                _disposed = true;
            }
        }
    }
}
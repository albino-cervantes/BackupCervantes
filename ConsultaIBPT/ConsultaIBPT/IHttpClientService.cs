using System.Threading.Tasks;

namespace IBPT.Api.Client
{
    /// <summary>
    /// Interface para o cliente HTTP, permitindo injeção de dependência e testes unitários
    /// </summary>
    public interface IHttpClientService
    {
        /// <summary>
        /// Executa uma requisição HTTP GET assíncrona
        /// </summary>
        /// <param name="uri">URI completa para a requisição</param>
        /// <returns>Conteúdo da resposta como string</returns>
        Task<string> GetAsync(string uri);
    }
}
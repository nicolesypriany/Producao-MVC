using Producao_MVC.Responses;

namespace Producao_MVC.Services
{
    public class ProdutoAPI
    {
        private readonly HttpClient _httpClient;

        public ProdutoAPI(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("API");
        }

        public async Task<IEnumerable<ProdutoResponse>> ListarProdutos()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<ProdutoResponse>>("Produto");
        }
    }
}

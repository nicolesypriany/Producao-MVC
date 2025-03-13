using Producao_MVC.Requests;
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

        public async Task<ProdutoResponse> BuscarProdutoPorID(int id)
        {
            return await _httpClient.GetFromJsonAsync<ProdutoResponse>($"Produto/{id}");
        }

        public async Task CriarProduto(ProdutoRequest request)
        {
            await _httpClient.PostAsJsonAsync("Produto", request);
        }

        public async Task AtualizarProduto(int id, ProdutoRequest request)
        {
            await _httpClient.PutAsJsonAsync($"Produto/{id}", request);
        }

        public async Task InativarProduto(int id)
        {
            await _httpClient.DeleteAsync($"Produto/{id}");
        }
    }
}

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
            var responseMessage = await _httpClient.GetAsync("Produto");

            if (responseMessage.IsSuccessStatusCode)
            {
                return await _httpClient.GetFromJsonAsync<IEnumerable<ProdutoResponse>>("Produto");
            }
            else
            {
                await ValidateResponse.Validate(responseMessage);
                return null;
            }
        }

        public async Task<ProdutoResponse> BuscarProdutoPorID(int id)
        {
            var responseMessage = await _httpClient.GetAsync($"Produto/{id}");

            if(responseMessage.IsSuccessStatusCode)
            {
                return await _httpClient.GetFromJsonAsync<ProdutoResponse>($"Produto/{id}");
            }
            else
            {
                await ValidateResponse.Validate(responseMessage);
                return null;
            }
        }

        public async Task CriarProduto(ProdutoRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("Produto", request);
            await ValidateResponse.Validate(response);
        }

        public async Task AtualizarProduto(int id, ProdutoRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"Produto/{id}", request);
            await ValidateResponse.Validate(response);
        }

        public async Task InativarProduto(int id)
        {
            var response = await _httpClient.DeleteAsync($"Produto/{id}");
            await ValidateResponse.Validate(response);
        }
    }
}

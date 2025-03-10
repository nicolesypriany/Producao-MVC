using Producao_MVC.Requests;
using Producao_MVC.Responses;

namespace Producao_MVC.Services
{
    public class ProducaoAPI
    {
        private readonly HttpClient _httpClient;

        public ProducaoAPI(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("API");
        }

        public async Task<IEnumerable<FormaResponse>> ListarFormas()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<FormaResponse>>("Forma");
        }

        public async Task<FormaResponse> BuscarFormaPorID(int id)
        {
            return await _httpClient.GetFromJsonAsync<FormaResponse>($"Forma/{id}");
        }

        public async Task CriarForma(FormaRequest request)
        {
            await _httpClient.PostAsJsonAsync("Forma", request);
        }

        public async Task AtualizarForma(int id, FormaRequest request)
        {
            await _httpClient.PutAsJsonAsync($"Forma/{id}", request);
        }

        public async Task<IEnumerable<ProdutoResponse>> ListarProdutos()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<ProdutoResponse>>("Produto");
        }

        public async Task<IEnumerable<MaquinaResponse>> ListarMaquinas()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<MaquinaResponse>>("Maquina");
        }

        public async Task<MaquinaResponse> BuscarMaquinaPorID(int id)
        {
            return await _httpClient.GetFromJsonAsync<MaquinaResponse>($"Maquina/{id}");
        }
    }
}

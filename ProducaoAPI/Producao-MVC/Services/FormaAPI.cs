using Producao_MVC.Requests;
using Producao_MVC.Responses;

namespace Producao_MVC.Services
{
    public class FormaAPI
    {
        private readonly HttpClient _httpClient;

        public FormaAPI(IHttpClientFactory factory)
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

        public async Task InativarForma(int id)
        {
            await _httpClient.DeleteAsync($"Forma/{id}");
        }
    }
}

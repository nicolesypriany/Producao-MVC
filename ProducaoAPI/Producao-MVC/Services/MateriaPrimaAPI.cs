using Producao_MVC.Requests;
using Producao_MVC.Responses;

namespace Producao_MVC.Services
{
    public class MateriaPrimaAPI
    {
        private readonly HttpClient _httpClient;

        public MateriaPrimaAPI(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("API");
        }

        public async Task<IEnumerable<MateriaPrimaResponse>> ListarMateriasPrimas()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<MateriaPrimaResponse>>("MateriaPrima");
        }

        public async Task<MateriaPrimaResponse> BuscarMateriaPrimaPorID(int id)
        {
            return await _httpClient.GetFromJsonAsync<MateriaPrimaResponse>($"MateriaPrima/{id}");
        }

        public async Task CriarMateriaPrima(MateriaPrimaRequest request)
        {
            await _httpClient.PostAsJsonAsync("MateriaPrima", request);
        }

        public async Task AtualizarMateriaPrima(int id, MateriaPrimaRequest request)
        {
            await _httpClient.PutAsJsonAsync($"MateriaPrima/{id}", request);
        }

        public async Task InativarMateriaPrima(int id)
        {
            await _httpClient.DeleteAsync($"MateriaPrima/{id}");
        }
    }
}

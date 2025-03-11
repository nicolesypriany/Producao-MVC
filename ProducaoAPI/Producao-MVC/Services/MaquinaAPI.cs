using Producao_MVC.Responses;

namespace Producao_MVC.Services
{
    public class MaquinaAPI
    {
        private readonly HttpClient _httpClient;

        public MaquinaAPI(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("API");
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

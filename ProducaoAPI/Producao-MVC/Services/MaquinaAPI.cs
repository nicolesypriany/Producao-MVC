using Producao_MVC.Requests;
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
            var responseMessage = await _httpClient.GetAsync("Maquina");
            if(responseMessage.IsSuccessStatusCode)
            {
                return await _httpClient.GetFromJsonAsync<IEnumerable<MaquinaResponse>>("Maquina");
            }
            else
            {
                await ValidateResponse.Validate(responseMessage);
                return null;
            }
        }

        public async Task<MaquinaResponse> BuscarMaquinaPorID(int id)
        {
            var responseMessage = await _httpClient.GetAsync($"Maquina/{id}");
            
            if(responseMessage.IsSuccessStatusCode)
            {
                return await _httpClient.GetFromJsonAsync<MaquinaResponse>($"Maquina/{id}");
            }
            else
            {
                await ValidateResponse.Validate(responseMessage);
                return null;
            }
        }

        public async Task CriarMaquina(MaquinaRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("Maquina", request);
            await ValidateResponse.Validate(response);
        }

        public async Task AtualizarMaquina(int id, MaquinaRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"Maquina/{id}", request);
            await ValidateResponse.Validate(response);
        }

        public async Task InativarMaquina(int id)
        {
            var response = await _httpClient.DeleteAsync($"Maquina/{id}");
            await ValidateResponse.Validate(response);
        }
    }
}

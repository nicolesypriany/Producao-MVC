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
            var responseMessage = await _httpClient.GetAsync("Forma");
            
            if (responseMessage.IsSuccessStatusCode)
            {
                return await _httpClient.GetFromJsonAsync<IEnumerable<FormaResponse>>("Forma");
            }
            else
            {
                await ValidateResponse.Validate(responseMessage);
                return null;
            }
        }

        public async Task<FormaResponse> BuscarFormaPorID(int id)
        {
            var responseMessage = await _httpClient.GetAsync($"Forma/{id}");

            if (responseMessage.IsSuccessStatusCode)
            {
                return await responseMessage.Content.ReadFromJsonAsync<FormaResponse>();
            }
            else
            {
                await ValidateResponse.Validate(responseMessage);
                return null;
            }
        }

        public async Task CriarForma(FormaRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("Forma", request);
            await ValidateResponse.Validate(response);
        }

        public async Task AtualizarForma(int id, FormaRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"Forma/{id}", request);
            await ValidateResponse.Validate(response);
        }

        public async Task InativarForma(int id)
        {
            var response = await _httpClient.DeleteAsync($"Forma/{id}");
            await ValidateResponse.Validate(response);
        }
    }
}

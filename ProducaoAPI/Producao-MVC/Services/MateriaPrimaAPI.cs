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
            var responseMessage = await _httpClient.GetAsync("MateriaPrima");
            
            if (responseMessage.IsSuccessStatusCode)
            {
                return await _httpClient.GetFromJsonAsync<IEnumerable<MateriaPrimaResponse>>("MateriaPrima");
            }
            else
            {
                await ValidateResponse.Validate(responseMessage);
                return null;
            }
        }

        public async Task<MateriaPrimaResponse> BuscarMateriaPrimaPorID(int id)
        {
            var responseMessage = await _httpClient.GetAsync($"MateriaPrima/{id}");

            if (responseMessage.IsSuccessStatusCode)
            {
                return await _httpClient.GetFromJsonAsync<MateriaPrimaResponse>($"MateriaPrima/{id}");
            }
            else
            {
                await ValidateResponse.Validate(responseMessage);
                return null;
            }
        }

        public async Task CriarMateriaPrima(MateriaPrimaRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("MateriaPrima", request);
            await ValidateResponse.Validate(response);
        }

        public async Task AtualizarMateriaPrima(int id, MateriaPrimaRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"MateriaPrima/{id}", request);
            await ValidateResponse.Validate(response);
        }

        public async Task InativarMateriaPrima(int id)
        {
            var response = await _httpClient.DeleteAsync($"MateriaPrima/{id}");
            await ValidateResponse.Validate(response);
        }
    }
}

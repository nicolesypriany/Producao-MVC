using Producao_MVC.Requests;
using Producao_MVC.Responses;

namespace Producao_MVC.Services
{
    public class ProcessoProducaoAPI
    {
        private readonly HttpClient _httpClient;

        public ProcessoProducaoAPI(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("API");
        }

        public async Task<IEnumerable<ProcessoProducaoResponse>> ListarProcessosProducoes()
        {
            var responseMessage = await _httpClient.GetAsync("ProcessoProducao");
            
            if (responseMessage.IsSuccessStatusCode)
            {
                return await _httpClient.GetFromJsonAsync<IEnumerable<ProcessoProducaoResponse>>("ProcessoProducao");
            }
            else
            {
                await ValidateResponse.Validate(responseMessage);
                return null;
            }
        }

        public async Task<ProcessoProducaoResponse> BuscarProcessoProducaoPorID(int id)
        {
            var responseMessage = await _httpClient.GetAsync($"ProcessoProducao/{id}");

            if (responseMessage.IsSuccessStatusCode)
            {
                return await _httpClient.GetFromJsonAsync<ProcessoProducaoResponse>($"ProcessoProducao/{id}");
            }
            else
            {
                await ValidateResponse.Validate(responseMessage);
                return null;
            }
        }

        public async Task CriarProcessoProducao(ProcessoProducaoRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("ProcessoProducao", request);
            await ValidateResponse.Validate(response);
        }

        public async Task AtualizarProcessoProducao(int id, ProcessoProducaoRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"ProcessoProducao/{id}", request);
            await ValidateResponse.Validate(response);
        }

        public async Task InativarProcessoProducao(int id)
        {
            var response = await _httpClient.DeleteAsync($"ProcessoProducao/{id}");
            await ValidateResponse.Validate(response);
        }

        public async Task CalcularProducao(int id)
        {
            var response = await _httpClient.PostAsync($"ProcessoProducao/CalcularProducao/{id}", null);
            await ValidateResponse.Validate(response);
        }
    }
}

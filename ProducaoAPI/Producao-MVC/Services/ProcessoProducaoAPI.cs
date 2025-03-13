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
            return await _httpClient.GetFromJsonAsync<IEnumerable<ProcessoProducaoResponse>>("ProcessoProducao");
        }

        public async Task<ProcessoProducaoResponse> BuscarProcessoProducaoPorID(int id)
        {
            return await _httpClient.GetFromJsonAsync<ProcessoProducaoResponse>($"ProcessoProducao/{id}");
        }

        public async Task CriarProcessoProducao(ProcessoProducaoRequest request)
        {
            await _httpClient.PostAsJsonAsync("ProcessoProducao", request);
        }

        public async Task AtualizarProcessoProducao(int id, ProcessoProducaoRequest request)
        {
            await _httpClient.PutAsJsonAsync($"ProcessoProducao/{id}", request);
        }

        public async Task InativarProcessoProducao(int id)
        {
            await _httpClient.DeleteAsync($"ProcessoProducao/{id}");
        }
    }
}

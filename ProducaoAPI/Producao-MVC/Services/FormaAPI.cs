using Producao_MVC.Requests;
using Producao_MVC.Responses;
using System.Text;
using System.Text.Json;

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
            var response = await _httpClient.PostAsJsonAsync("Forma", request);

            if (!response.IsSuccessStatusCode)
            {
                var errorBytes = await response.Content.ReadAsByteArrayAsync(); // Lê os bytes sem alterar a codificação
                var errorString = Encoding.UTF8.GetString(errorBytes);

                try
                {
                    var errorObj = JsonSerializer.Deserialize<Dictionary<string, object>>(errorString);
                    var errorMessage = errorObj != null && errorObj.ContainsKey("Message")
                        ? errorObj["Message"].ToString()
                        : "Erro desconhecido ao processar a requisição.";

                    throw new Exception(errorMessage);
                }
                catch (JsonException)
                {
                    throw new Exception("Erro inesperado ao interpretar a resposta da API.");
                }
            }
        }

        //public async Task CriarForma(FormaRequest request)
        //{
        //    await _httpClient.PostAsJsonAsync("Forma", request);
        //}

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

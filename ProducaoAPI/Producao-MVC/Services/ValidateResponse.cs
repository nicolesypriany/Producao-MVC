using System.Text.Json;
using System.Text;

namespace Producao_MVC.Services
{
    public static class ValidateResponse
    {
        public static async Task Validate(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var errorBytes = await response.Content.ReadAsByteArrayAsync();
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
    }
}

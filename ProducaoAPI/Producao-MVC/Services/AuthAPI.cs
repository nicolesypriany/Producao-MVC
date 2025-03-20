using Producao_MVC.Responses;

namespace Producao_MVC.Services
{
    public class AuthAPI
    {
        private readonly HttpClient _httpClient;

        public AuthAPI(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("API");
        }

        public async Task<AuthReponse> Login(string email, string senha)
        {
            var response = await _httpClient.PostAsJsonAsync("auth/login", new
            {
                email,
                password = senha
            });

            await ValidateResponse.Validate(response);

            if (response.IsSuccessStatusCode)
            {
                return new AuthReponse { Sucesso = true };
            }
            return new AuthReponse { Sucesso = false };
        }
    }
}

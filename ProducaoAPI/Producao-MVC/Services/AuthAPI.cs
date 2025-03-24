using Microsoft.AspNetCore.Components.Authorization;
using Producao_MVC.Responses;
using System.Security.Claims;

namespace Producao_MVC.Services
{
    public class AuthAPI : AuthenticationStateProvider
    {
        private bool autenticado = false;
        private readonly HttpClient _httpClient;

        public AuthAPI(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("API");
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            autenticado = false;
            var pessoa = new ClaimsPrincipal();
            var response = await _httpClient.GetAsync("auth/manage/info");

            if(response.IsSuccessStatusCode)
            {
                var info = await response.Content.ReadFromJsonAsync<InfoPessoaResponse>();
                Claim[] dados =
                [
                    new Claim(ClaimTypes.Name, info.Email),
                    new Claim(ClaimTypes.Email, info.Email)
                ];

                var identity = new ClaimsIdentity(dados, "Cookies");
                pessoa = new ClaimsPrincipal(identity);
                autenticado = true;
            }

            return new AuthenticationState(pessoa);
        }

        public async Task<AuthReponse> Login(string email, string senha)
        {
            var response = await _httpClient.PostAsJsonAsync($"auth/login?useCookies=true&useSessionCookies=true", new
            {
                email,
                password = senha
            });

            await ValidateResponse.Validate(response);

            if (response.IsSuccessStatusCode)
            {
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
                return new AuthReponse { Sucesso = true };
            }
            return new AuthReponse { Sucesso = false };
        }

        public async Task Logout()
        {
            await _httpClient.PostAsync("auth/logout", null);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task<bool> VerificarAutenticado()
        {
            await GetAuthenticationStateAsync();
            return autenticado;
        }
    }
}

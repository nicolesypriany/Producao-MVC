using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Producao_MVC.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Producao_MVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthAPI _authApi;

        public AuthController(AuthAPI authApi)
        {
            _authApi = authApi;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                var response = await _authApi.Login(request.Email, request.Password);
                if(response.Sucesso == true)
                {
                    TempData["MensagemSucesso"] = "Login efetuado com sucesso";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["MensagemErro"] = $"Erro ao efetuar o login.";
                    return View();
                }
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Erro ao efetuar o login. Erro: {erro.Message}";
                return View();
            }
        }
    }
}

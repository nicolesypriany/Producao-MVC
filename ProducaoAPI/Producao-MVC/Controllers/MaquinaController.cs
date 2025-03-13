using Microsoft.AspNetCore.Mvc;
using Producao_MVC.Requests;
using Producao_MVC.Services;

namespace Producao_MVC.Controllers
{
    public class MaquinaController : Controller
    {
        private readonly MaquinaAPI _maquinaApi;

        public MaquinaController(MaquinaAPI maquinaApi)
        {
            _maquinaApi = maquinaApi;
        }

        public async Task<IActionResult> Index()
        {
            var maquinas = await _maquinaApi.ListarMaquinas();
            return View(maquinas);
        }
        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CriarMaquina(MaquinaRequest request)
        {
            try
            {
                await _maquinaApi.CriarMaquina(request);
                TempData["MensagemSucesso"] = "Máquina cadastrada com sucesso";
                return RedirectToAction("Index");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Não conseguimos cadastrar a sua máquina, tente novamente, detalhe do erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Editar(int id)
        {
            var maquina = await _maquinaApi.BuscarMaquinaPorID(id);
            return View(maquina);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(int id, MaquinaRequest request)
        {
            try
            {
                await _maquinaApi.AtualizarMaquina(id, request);
                TempData["MensagemSucesso"] = "Máquina alterada com sucesso";
                return RedirectToAction("Index");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Não conseguimos atualizar a máquina, tente novamente, detalhe do erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Inativar(int id)
        {
            var maquina = await _maquinaApi.BuscarMaquinaPorID(id);
            return View(maquina);
        }

        public async Task<IActionResult> InativarMaquina(int id)
        {
            await _maquinaApi.InativarMaquina(id);
            TempData["MensagemSucesso"] = "Máquina inativada com sucesso";
            return RedirectToAction("Index");
        }
    }
}


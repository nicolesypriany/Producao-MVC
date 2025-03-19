using Microsoft.AspNetCore.Mvc;
using Producao_MVC.Requests;
using Producao_MVC.Services;

namespace Producao_MVC.Controllers
{
    public class MateriaPrimaController : Controller
    {
        private readonly MateriaPrimaAPI _materiaPrimaAPI;

        public MateriaPrimaController(MateriaPrimaAPI materiaPrimaAPI)
        {
            _materiaPrimaAPI = materiaPrimaAPI;
        }

        public async Task<IActionResult> Index()
        {
            var materiasPrimas = await _materiaPrimaAPI.ListarMateriasPrimas();
            return View(materiasPrimas);
        }
        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CriarMateriaPrima(MateriaPrimaRequest request)
        {
            try
            {
                await _materiaPrimaAPI.CriarMateriaPrima(request);
                TempData["MensagemSucesso"] = "Matéria-prima cadastrada com sucesso";
                return RedirectToAction("Index");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Não conseguimos cadastrar a sua matéria-prima. Erro: {erro.Message}";
                return View("Criar");
            }
        }

        public async Task<IActionResult> Editar(int id)
        {
            var materiaPrima = await _materiaPrimaAPI.BuscarMateriaPrimaPorID(id);
            return View(materiaPrima);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(int id, MateriaPrimaRequest request)
        {
            try
            {
                await _materiaPrimaAPI.AtualizarMateriaPrima(id, request);
                TempData["MensagemSucesso"] = "Matéria-prima alterada com sucesso";
                return RedirectToAction("Index");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Não conseguimos atualizar a matéria-prima. Erro: {erro.Message}";
                return View();
            }
        }

        public async Task<IActionResult> Inativar(int id)
        {
            try
            {
                var materiaPrima = await _materiaPrimaAPI.BuscarMateriaPrimaPorID(id);
                return View(materiaPrima);
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Não conseguimos atualizar a matéria-prima. Erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> InativarMateriaPrima(int id)
        {
            await _materiaPrimaAPI.InativarMateriaPrima(id);
            TempData["MensagemSucesso"] = "Matéria-prima inativada com sucesso";
            return RedirectToAction("Index");
        }
    }
}

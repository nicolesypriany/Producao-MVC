using Microsoft.AspNetCore.Mvc;
using Producao_MVC.Requests;
using Producao_MVC.Services;

namespace Producao_MVC.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly ProdutoAPI _produtoAPI;

        public ProdutoController(ProdutoAPI produtoAPI)
        {
            _produtoAPI = produtoAPI;
        }

        public async Task<IActionResult> Index()
        {
            var produtos = await _produtoAPI.ListarProdutos();
            return View(produtos);
        }
        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CriarProduto(ProdutoRequest request)
        {
            try
            {
                await _produtoAPI.CriarProduto(request);
                TempData["MensagemSucesso"] = "Produto cadastrado com sucesso";
                return RedirectToAction("Index");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Não conseguimos cadastrar o seu produto, tente novamente, detalhe do erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Editar(int id)
        {
            var produto = await _produtoAPI.BuscarProdutoPorID(id);
            return View(produto);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(int id, ProdutoRequest request)
        {
            try
            {
                await _produtoAPI.AtualizarProduto(id, request);
                TempData["MensagemSucesso"] = "Produto alterado com sucesso";
                return RedirectToAction("Index");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Não conseguimos atualizar o produto, tente novamente, detalhe do erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Inativar(int id)
        {
            var produto = await _produtoAPI.BuscarProdutoPorID(id);
            return View(produto);
        }

        public async Task<IActionResult> InativarProduto(int id)
        {
            await _produtoAPI.InativarProduto(id);
            TempData["MensagemSucesso"] = "Produto inativado com sucesso";
            return RedirectToAction("Index");
        }
    }
}

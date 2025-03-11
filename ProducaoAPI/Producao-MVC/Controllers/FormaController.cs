using Microsoft.AspNetCore.Mvc;
using Producao_MVC.Models;
using Producao_MVC.Requests;
using Producao_MVC.Responses;
using Producao_MVC.Services;

namespace Producao_MVC.Controllers
{
    public class FormaController : Controller
    {
        private readonly FormaAPI _formaApi;
        private readonly MaquinaAPI _maquinaApi;
        private readonly ProdutoAPI _produtoAPI;

        public FormaController(FormaAPI formaApi, MaquinaAPI maquinaApi, ProdutoAPI produtoAPI)
        {
            _formaApi = formaApi;
            _maquinaApi = maquinaApi;
            _produtoAPI = produtoAPI;
        }

        public async Task<IActionResult> Index()
        {
            var formas = await _formaApi.ListarFormas();
            return View(formas);
        }

        public async Task<IActionResult> Criar(FormaRequest request)
        {
            var produtos = await _produtoAPI.ListarProdutos();
            var maquinas = await _maquinaApi.ListarMaquinas();
            var maquinasCheckbox = new List<MaquinaCheckboxViewModel>();

            foreach (var maquina in maquinas)
            {
                var maquinaCheck = new MaquinaCheckboxViewModel
                {
                    Id = maquina.Id,
                    Nome = maquina.Nome,
                    Selecionado = false
                };
                maquinasCheckbox.Add(maquinaCheck);
            }

            var viewModel = new FormaProdutoViewModel
            {
                Produtos = produtos,
                Maquinas = maquinas.ToList(),
                MaquinasCheckbox = maquinasCheckbox,
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Criar(FormaProdutoViewModel formaVm)
        {
            try
            {
                var forma = CriarFormaPorModelo(formaVm);
                TempData["MensagemSucesso"] = "Forma cadastrada com sucesso";
                return RedirectToAction("Index");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Não conseguimos cadastrar a sua forma, tente novamente, detalhe do erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Editar(int id)
        {
            var produtos = await _produtoAPI.ListarProdutos();
            var maquinas = await _maquinaApi.ListarMaquinas();
            var forma = await _formaApi.BuscarFormaPorID(id);
            var maquinasDaForma = forma.Maquinas.ToList();

            var maquinasCheckbox = new List<MaquinaCheckboxViewModel>();
            foreach (var maquina in maquinas)
            {
                maquinasCheckbox.Add(new MaquinaCheckboxViewModel
                {
                    Id = maquina.Id,
                    Nome = maquina.Nome,
                    Selecionado = false
                });
            }

            for (int i = 0; i < maquinasCheckbox.Count; i++)
            {
                var maquina = await _maquinaApi.BuscarMaquinaPorID(maquinasCheckbox[i].Id);
                if (maquinasDaForma.Contains(maquina))
                {
                    maquinasCheckbox[i].Selecionado = true;
                }
            }

            var viewModel = new FormaProdutoViewModel
            {
                Forma = forma,
                Nome = forma.Nome,
                ProdutoId = forma.Produto.Id,
                PecasPorCiclo = forma.PecasPorCiclo,
                Maquinas = maquinas.ToList(),
                MaquinasCheckbox = maquinasCheckbox,
                Produtos = produtos
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(FormaProdutoViewModel formaVm)
        {
            try
            {
                List<FormaMaquinaRequest> maquinasSelecionadas = new();
                foreach (var item in formaVm.MaquinasCheckbox)
                {
                    if (item.Selecionado)
                    {
                        maquinasSelecionadas.Add(new FormaMaquinaRequest(item.Id));
                    }
                }

                var request = new FormaRequest(formaVm.Nome, formaVm.ProdutoId, formaVm.PecasPorCiclo, maquinasSelecionadas);
                await _formaApi.AtualizarForma(formaVm.Forma.Id, request);

                TempData["MensagemSucesso"] = "Forma alterada com sucesso";
                return RedirectToAction("Index");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Não conseguimos atualizar a forma, tente novamente, detalhe do erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> InativarConfirmacao(int id)
        {
            var forma = await _formaApi.BuscarFormaPorID(id);
            return View(forma);
        }

        public async Task<IActionResult> Inativar(int id)
        {
            await _formaApi.InativarForma(id);
            TempData["MensagemSucesso"] = "Forma apagada com sucesso";
            return RedirectToAction("Index");
        }

        public async Task CriarFormaPorModelo(FormaProdutoViewModel formaVm)
        {
            List<MaquinaResponse> maquinasSelecionadas = new();
            foreach (var item in formaVm.MaquinasCheckbox)
            {
                if (item.Selecionado)
                {
                    maquinasSelecionadas.Add(await _maquinaApi.BuscarMaquinaPorID(item.Id));
                }
            }

            List<FormaMaquinaRequest> formaMaquinasRequest = new();
            foreach (var item in maquinasSelecionadas)
            {
                formaMaquinasRequest.Add(new FormaMaquinaRequest(item.Id));
            }

            var formaRequest = new FormaRequest(formaVm.Nome, formaVm.ProdutoId, formaVm.PecasPorCiclo, formaMaquinasRequest);
            await _formaApi.CriarForma(formaRequest);
        }
    }
}

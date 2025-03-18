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

        public async Task<IActionResult> Criar()
        {
            return View(await RetornarViewModel(false));
        }

        [HttpPost]
        public async Task<IActionResult> Criar(FormaProdutoViewModel formaViewModel)
        {
            try
            {
                await VerificarNomeDuplicado(true, formaViewModel.Nome);

                var forma = await CriarFormaPorModelo(formaViewModel);
                await _formaApi.CriarForma(forma);

                TempData["MensagemSucesso"] = "Forma cadastrada com sucesso";
                return RedirectToAction("Index");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Não conseguimos cadastrar a sua forma. Erro: {erro.Message}";
                return View(await RetornarViewModel(false));
            }
        }

        public async Task<IActionResult> Editar(int id)
        {
            return View(await RetornarViewModel(true, id));
        }

        [HttpPost]
        public async Task<IActionResult> Editar(FormaProdutoViewModel formaViewModel)
        {
            try
            {
                await VerificarNomeDuplicado(false, formaViewModel.Nome, formaViewModel.Forma.Id);

                List<FormaMaquinaRequest> maquinasSelecionadas = [];
                foreach (var item in formaViewModel.MaquinasCheckbox)
                {
                    if (item.Selecionado)
                    {
                        maquinasSelecionadas.Add(new FormaMaquinaRequest(item.Id));
                    }
                }

                var request = new FormaRequest(formaViewModel.Nome, formaViewModel.ProdutoId, formaViewModel.PecasPorCiclo, maquinasSelecionadas);
                await _formaApi.AtualizarForma(formaViewModel.Forma.Id, request);

                TempData["MensagemSucesso"] = "Forma alterada com sucesso";
                return RedirectToAction("Index");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Não conseguimos atualizar a forma. Erro: {erro.Message}";
                return View(await RetornarViewModel(true, formaViewModel.Forma.Id));
            }
        }

        public async Task<IActionResult> Inativar(int id)
        {
            try
            {
                var forma = await _formaApi.BuscarFormaPorID(id);
                return View(forma);
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Não conseguimos atualizar a forma. Erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> InativarForma(int id)
        {
            await _formaApi.InativarForma(id);
            TempData["MensagemSucesso"] = "Forma inativada com sucesso";
            return RedirectToAction("Index");
        }

        private async Task<FormaRequest> CriarFormaPorModelo(FormaProdutoViewModel formaViewModel)
        {
            List<MaquinaResponse> maquinasSelecionadas = [];
            foreach (var item in formaViewModel.MaquinasCheckbox)
            {
                if (item.Selecionado)
                {
                    maquinasSelecionadas.Add(await _maquinaApi.BuscarMaquinaPorID(item.Id));
                }
            }

            List<FormaMaquinaRequest> formaMaquinasRequest = [];
            foreach (var item in maquinasSelecionadas)
            {
                formaMaquinasRequest.Add(new FormaMaquinaRequest(item.Id));
            }

            var formaRequest = new FormaRequest(formaViewModel.Nome, formaViewModel.ProdutoId, formaViewModel.PecasPorCiclo, formaMaquinasRequest);
            return formaRequest;
        }

        private async Task<FormaProdutoViewModel> RetornarViewModel(bool Editar, int id = 0)
        {
            var produtos = await _produtoAPI.ListarProdutos();
            var maquinas = await _maquinaApi.ListarMaquinas();

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

            if (Editar)
            {
                var forma = await _formaApi.BuscarFormaPorID(id);
                var maquinasDaForma = forma.Maquinas.ToList();

                for (int i = 0; i < maquinasCheckbox.Count; i++)
                {
                    var maquina = await _maquinaApi.BuscarMaquinaPorID(maquinasCheckbox[i].Id);
                    if (maquinasDaForma.Contains(maquina))
                    {
                        maquinasCheckbox[i].Selecionado = true;
                    }
                }

                return new FormaProdutoViewModel
                {
                    Forma = forma,
                    Nome = forma.Nome,
                    ProdutoId = forma.Produto.Id,
                    PecasPorCiclo = forma.PecasPorCiclo,
                    Maquinas = maquinas.ToList(),
                    MaquinasCheckbox = maquinasCheckbox,
                    Produtos = produtos
                };
            }
            else
            {
                return new FormaProdutoViewModel
                {
                    Produtos = produtos,
                    Maquinas = maquinas.ToList(),
                    MaquinasCheckbox = maquinasCheckbox,
                };
            }
        }

        private async Task VerificarNomeDuplicado(bool cadastrar, string nome, int id = 0)
        {
            var formas = await _formaApi.ListarFormas();
            List<string> nomes = formas.ToList().Select(f => f.Nome).ToList();
            
            if(cadastrar)
            {
                if (nomes.Contains(nome))
                {
                    TempData["MensagemErro"] = "Já existe uma forma com este nome!";
                }
            } 
            else
            {
                var forma = await _formaApi.BuscarFormaPorID(id);
                if (nomes.Contains(nome) && forma.Nome != nome)
                {
                    TempData["MensagemErro"] = "Já existe uma forma com este nome!";
                }
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Producao_MVC.Models;
using Producao_MVC.Requests;
using Producao_MVC.Responses;
using Producao_MVC.Services;

namespace Producao_MVC.Controllers
{
    public class ProcessoProducaoController : Controller
    {
        private readonly ProcessoProducaoAPI _processoProducaoAPI;
        private readonly MaquinaAPI _maquinaAPI;
        private readonly FormaAPI _formaAPI;
        private readonly ProdutoAPI _produtoAPI;
        private readonly MateriaPrimaAPI _materiaPrimaAPI;

        public ProcessoProducaoController(ProcessoProducaoAPI processoProducaoAPI, MaquinaAPI maquinaAPI, FormaAPI formaAPI, ProdutoAPI produtoAPI, MateriaPrimaAPI materiaPrimaAPI)
        {
            _processoProducaoAPI = processoProducaoAPI;
            _maquinaAPI = maquinaAPI;
            _formaAPI = formaAPI;
            _produtoAPI = produtoAPI;
            _materiaPrimaAPI = materiaPrimaAPI;
        }

        public async Task<IActionResult> Index()
        {
            var producoes = await _processoProducaoAPI.ListarProcessosProducoes();
            return View(producoes);
        }
        public async Task<IActionResult> Criar()
        {
            var maquinas = await _maquinaAPI.ListarMaquinas();
            var formas = await _formaAPI.ListarFormas();
            var produtos = await _produtoAPI.ListarProdutos();
            var materiasPrimas = await _materiaPrimaAPI.ListarMateriasPrimas();

            var materiasCheckbox = new List<MateriaPrimaCheckboxViewModel>();

            foreach (var materia in materiasPrimas)
            {
                var materiaCheck = new MateriaPrimaCheckboxViewModel
                {
                    Id = materia.Id,
                    Nome = materia.Nome,
                    Selecionado = false,
                    Quantidade = 0
                };
                materiasCheckbox.Add(materiaCheck);
            }

            var viewModel = new ProcessoProducaoViewModel
            {
                Maquinas = maquinas.ToList(),
                Formas = formas.ToList(),
                Produtos = produtos.ToList(),
                MateriasPrimas = materiasPrimas.ToList(),
                MateriasPrimasCheckbox = materiasCheckbox
            };

            return View(viewModel);
        }
        
        public async Task CriarProducaoPorModelo(ProcessoProducaoViewModel request)
        {
            List<MateriaPrimaResponse> materiasSelecionadas = new();
            List<ProcessoProducaoMateriaPrimaRequest> producaoMateriaPrimas = new();

            foreach (var item in request.MateriasPrimasCheckbox)
            {
                if (item.Selecionado)
                {
                    materiasSelecionadas.Add(await _materiaPrimaAPI.BuscarMateriaPrimaPorID(item.Id));
                }
            }

            for (int i = 0; i < materiasSelecionadas.Count; i++)
            {
                producaoMateriaPrimas.Add(new ProcessoProducaoMateriaPrimaRequest(materiasSelecionadas[i].Id, request.MateriasPrimasCheckbox[i].Quantidade));
            };

            var producao = new ProcessoProducaoRequest(request.Data, request.MaquinaId, request.FormaId, request.Ciclos, producaoMateriaPrimas);
            await _processoProducaoAPI.CriarProcessoProducao(producao);
        }

        [HttpPost]
        public IActionResult Criar(ProcessoProducaoViewModel producaoVm)
        {
            try
            {
                var producao = CriarProducaoPorModelo(producaoVm);
                TempData["MensagemSucesso"] = "Produção cadastrada com sucesso";
                return RedirectToAction("Index");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Não conseguimos cadastrar a produção, tente novamente, detalhe do erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Editar(int id)
        {
            var producao = await _processoProducaoAPI.BuscarProcessoProducaoPorID(id);
            var maquinas = await _maquinaAPI.ListarMaquinas();
            var formas = await _formaAPI.ListarFormas();
            var produtos = await _produtoAPI.ListarProdutos();
            var materiasPrimas = await _materiaPrimaAPI.ListarMateriasPrimas();

            var materiasPrimasDaProducao = producao.ProducaoMateriasPrimas.ToList();
            var materiasCheckbox = new List<MateriaPrimaCheckboxViewModel>();

            foreach (var materia in materiasPrimas)
            {
                var materiaCheck = new MateriaPrimaCheckboxViewModel
                {
                    Id = materia.Id,
                    Nome = materia.Nome,
                    Selecionado = false,
                    Quantidade = 0
                };
                materiasCheckbox.Add(materiaCheck);
            }

            for (int i = 0; i < materiasCheckbox.Count; i++)
            {

                foreach (var item in materiasPrimasDaProducao)
                {
                    if (item.MateriaPrimaId == materiasCheckbox[i].Id)
                    {
                        materiasCheckbox[i].Selecionado = true;
                        materiasCheckbox[i].Quantidade = item.Quantidade;
                    }
                }
            }

            var viewModel = new ProcessoProducaoViewModel
            {
                Producao = producao,
                Data = producao.Data,
                MaquinaId = producao.MaquinaId,
                Maquinas = maquinas.ToList(),
                FormaId = producao.FormaId,
                Formas = formas.ToList(),
                Produtos = produtos.ToList(),
                Ciclos = producao.Ciclos,
                MateriasPrimas = materiasPrimas.ToList(),
                MateriasPrimasCheckbox = materiasCheckbox
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditarProducao(ProcessoProducaoViewModel producaoVM)
        {
            try
            {
                List<ProcessoProducaoMateriaPrimaRequest> producaoMateriaPrimas = new();

                foreach (var item in producaoVM.MateriasPrimasCheckbox)
                {
                    if (item.Selecionado)
                    {
                        producaoMateriaPrimas.Add(new ProcessoProducaoMateriaPrimaRequest(item.Id, item.Quantidade));
                    }
                }

                var request = new ProcessoProducaoRequest(producaoVM.Data, producaoVM.MaquinaId, producaoVM.FormaId, producaoVM.Ciclos, producaoMateriaPrimas);

                await _processoProducaoAPI.AtualizarProcessoProducao(producaoVM.Producao.Id, request);
                TempData["MensagemSucesso"] = "Produção alterada com sucesso";
                return RedirectToAction("Index");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Não conseguimos atualizar a produção, tente novamente, detalhe do erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Inativar(int id)
        {
            var producao = await _processoProducaoAPI.BuscarProcessoProducaoPorID(id);
            return View(producao);
        }

        public async Task<IActionResult> InativarProducao(int id)
        {
            await _processoProducaoAPI.InativarProcessoProducao(id);
            TempData["MensagemSucesso"] = "Produção inativada com sucesso";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> CalcularProducao(int id)
        {
            await _processoProducaoAPI.CalcularProducao(id);
            TempData["MensagemSucesso"] = "Produção calculada com sucesso";
            return RedirectToAction("Index");
        }
    }
}

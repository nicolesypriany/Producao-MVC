using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;
using ProducaoAPI.Services.Interfaces;

namespace ProducaoAPI.Services
{
    public class ProducaoMateriaPrimaServices : IProducaoMateriaPrimaService
    {
        private readonly IProducaoMateriaPrimaRepository _producaoMateriaPrimaRepository;
        public ProducaoMateriaPrimaServices(IProducaoMateriaPrimaRepository producaoMateriaPrimaRepository)
        {
            _producaoMateriaPrimaRepository = producaoMateriaPrimaRepository;
        }

        public ProducaoMateriaPrimaResponse EntityToResponse(ProcessoProducaoMateriaPrima producaoMateriaPrima)
        {
            return new ProducaoMateriaPrimaResponse(
                producaoMateriaPrima.MateriaPrimaId,
                producaoMateriaPrima.MateriaPrima.Nome,
                producaoMateriaPrima.Quantidade
            );
        }

        public ICollection<ProducaoMateriaPrimaResponse> EntityListToResponseList(ICollection<ProcessoProducaoMateriaPrima> producoesMateriasPrimas)
        {
            return producoesMateriasPrimas.Select(m => EntityToResponse(m)).ToList();
        }

        public async Task VerificarProducoesMateriasPrimasExistentes(int producaoId, ICollection<ProcessoProducaoMateriaPrimaRequest> materiasPrimasRequest)
        {
            var producoesMateriasPrimas = await _producaoMateriaPrimaRepository.ListarProducoesMateriasPrimas();

            List<int> idMateriasPrimasAtuais = producoesMateriasPrimas
                .Where(p => p.ProducaoId == producaoId)
                .Select(p => p.MateriaPrimaId)
                .ToList();

            List<int> idMateriasPrimasNovas = materiasPrimasRequest
                .Select(m => m.Id)
                .ToList();

            await CriarOuAtualizarProducaoMateriaPrima(producaoId, idMateriasPrimasNovas, idMateriasPrimasAtuais, materiasPrimasRequest);
            await ExcluirProducaoMateriaPrima(producaoId, idMateriasPrimasNovas, idMateriasPrimasAtuais);
        }

        private async Task CriarOuAtualizarProducaoMateriaPrima(int producaoId, List<int> idMateriasPrimasNovas, List<int> idMateriasPrimasAtuais, ICollection<ProcessoProducaoMateriaPrimaRequest> materiasPrimasRequest)
        {
            foreach (var materiaPrimaId in idMateriasPrimasNovas)
            {
                if (idMateriasPrimasAtuais.Contains(materiaPrimaId))
                {
                    await CompararQuantidadesMateriasPrimas(producaoId, materiaPrimaId, materiasPrimasRequest);
                }
                //else
                //{

                //    var quantidade = await RetornarQuantidadeMateriaPrima(materiaPrimaId, materiasPrimasRequest);
                //    var novoProcesso = new ProcessoProducaoMateriaPrima(producaoId, materiaPrimaId, quantidade);
                //    await _producaoMateriaPrimaRepository.AdicionarAsync(novoProcesso);
                //}

                var quantidade = materiasPrimasRequest.Where(m => m.Id == materiaPrimaId).Select(m => m.Quantidade).First();
                var novoProcesso = new ProcessoProducaoMateriaPrima(producaoId, materiaPrimaId, quantidade);
                await _producaoMateriaPrimaRepository.AdicionarAsync(novoProcesso);
            }
        }

        private async Task ExcluirProducaoMateriaPrima(int producaoId, List<int> listaIdNovasMaterias, List<int> listaIdMateriasAtuais)
        {
            foreach (var materiaPrimaId in listaIdMateriasAtuais)
            {
                if (!listaIdNovasMaterias.Contains(materiaPrimaId))
                {
                    var producaoMateriaPrimaExistente = await _producaoMateriaPrimaRepository.BuscarProducaoMateriaPrimaPorIdDaProducaoEIdDaMateriaPrimaAsync(producaoId, materiaPrimaId);
                    await _producaoMateriaPrimaRepository.RemoverAsync(producaoMateriaPrimaExistente);
                }
            }
        }

        public async Task<double> RetornarQuantidadeMateriaPrima(int idMateriaPrima, ICollection<ProcessoProducaoMateriaPrimaRequest> materiasPrimasRequest)
        {
            double quantidade = 0;
            foreach (var materiaPrima in materiasPrimasRequest)
            {
                if (materiaPrima.Id == idMateriaPrima)
                {
                    quantidade = materiaPrima.Quantidade;
                }
            }
            return quantidade;
        }

        private async Task CompararQuantidadesMateriasPrimas(int producaoId, int materiaPrimaId, ICollection<ProcessoProducaoMateriaPrimaRequest> materiasPrimasRequest)
        {
            var producaoMateriaPrimaExistente = await _producaoMateriaPrimaRepository.BuscarProducaoMateriaPrimaPorIdDaProducaoEIdDaMateriaPrimaAsync(producaoId, materiaPrimaId);

            var quantidadeNova = materiasPrimasRequest.Where(m => m.Id == materiaPrimaId).Select(m => m.Quantidade).First();
            if (producaoMateriaPrimaExistente.Quantidade != quantidadeNova)
            {
                producaoMateriaPrimaExistente.Quantidade = quantidadeNova;
                await _producaoMateriaPrimaRepository.AtualizarAsync(producaoMateriaPrimaExistente);
            }
        }

        public Task AdicionarAsync(ProcessoProducaoMateriaPrima producaoMateriaPrima) => _producaoMateriaPrimaRepository.AdicionarAsync(producaoMateriaPrima);
    }
}

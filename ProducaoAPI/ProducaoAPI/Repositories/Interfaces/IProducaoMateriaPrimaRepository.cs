using ProducaoAPI.Models;

namespace ProducaoAPI.Repositories.Interfaces
{
    public interface IProducaoMateriaPrimaRepository
    {
        Task<IEnumerable<ProcessoProducaoMateriaPrima>> ListarProducoesMateriasPrimas();
        Task<ProcessoProducaoMateriaPrima> BuscarProducaoMateriaPrimaPorIdDaProducaoEIdDaMateriaPrimaAsync(int idProducao, int idMateriaPrima);
        Task AdicionarAsync(ProcessoProducaoMateriaPrima producaoMateriaPrima);
        Task AtualizarAsync(ProcessoProducaoMateriaPrima producaoMateriaPrima);
        Task RemoverAsync(ProcessoProducaoMateriaPrima producaoMateriaPrima);
    }
}

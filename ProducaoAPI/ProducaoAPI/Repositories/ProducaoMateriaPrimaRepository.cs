using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Exceptions;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;

namespace ProducaoAPI.Repositories
{
    public class ProducaoMateriaPrimaRepository : IProducaoMateriaPrimaRepository
    {
        private readonly ProducaoContext _context;
        public ProducaoMateriaPrimaRepository(ProducaoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProcessoProducaoMateriaPrima>> ListarProducoesMateriasPrimas()
        {
            var producoesMateriasPrimas = await _context.ProducoesMateriasPrimas
                .Include(m => m.MateriaPrima)
                .ToListAsync();

            if (producoesMateriasPrimas == null || producoesMateriasPrimas.Count == 0) throw new NotFoundException("Nenhuma produção-matéria-prima encontrada.");
            return producoesMateriasPrimas;
        }

        public async Task<ProcessoProducaoMateriaPrima> BuscarProducaoMateriaPrimaPorIdDaProducaoEIdDaMateriaPrimaAsync(int idProducao, int idMateriaPrima)
        {
            var producaoMateriaPrima = await _context.ProducoesMateriasPrimas
                .Where(p => p.ProducaoId == idProducao)
                .Where(p => p.MateriaPrimaId == idMateriaPrima)
                .FirstAsync();

            return producaoMateriaPrima;
        }

        public async Task AdicionarAsync(ProcessoProducaoMateriaPrima producaoMateriaPrima)
        {
            await _context.ProducoesMateriasPrimas.AddAsync(producaoMateriaPrima);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(ProcessoProducaoMateriaPrima producaoMateriaPrima)
        {
            _context.ProducoesMateriasPrimas.Update(producaoMateriaPrima);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverAsync(ProcessoProducaoMateriaPrima producaoMateriaPrima)
        {
            _context.ProducoesMateriasPrimas.Remove(producaoMateriaPrima);
            await _context.SaveChangesAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Exceptions;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;

namespace ProducaoAPI.Repositories
{
    public class MateriaPrimaRepository : IMateriaPrimaRepository
    {
        private readonly IDbContextFactory<ProducaoContext> _contextFactory;
        public MateriaPrimaRepository(IDbContextFactory<ProducaoContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<MateriaPrima>> ListarMateriasPrimasAtivas()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var materiasPrimas = await context.MateriasPrimas
                .Where(m => m.Ativo == true)
                .ToListAsync();

                if (materiasPrimas == null || materiasPrimas.Count == 0) throw new NotFoundException("Nenhuma matéria-prima ativa encontrada.");
                return materiasPrimas;
            }
                
        }

        public async Task<IEnumerable<MateriaPrima>> ListarTodasMateriasPrimas()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var materiasPrimas = await context.MateriasPrimas
                .ToListAsync();

                if (materiasPrimas == null || materiasPrimas.Count == 0) throw new NotFoundException("Nenhuma matéria-prima ativa encontrada.");
                return materiasPrimas;
            }
                
        }

        public async Task<MateriaPrima> BuscarMateriaPrimaPorIdAsync(int id)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var materiaPrima = await context.MateriasPrimas
                .FirstOrDefaultAsync(m => m.Id == id);

                if (materiaPrima == null) throw new NotFoundException("ID da matéria-prima não encontrado.");
                return materiaPrima;
            }
                
        }

        public async Task AdicionarAsync(MateriaPrima materiaPrima)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                await context.MateriasPrimas.AddAsync(materiaPrima);
                await context.SaveChangesAsync();
            }
                
        }

        public async Task AtualizarAsync(MateriaPrima materiaPrima)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                context.MateriasPrimas.Update(materiaPrima);
                await context.SaveChangesAsync();
            }
                
        }

        public async Task<IEnumerable<string>> ListarNomes()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.MateriasPrimas
                .Select(m => m.Nome)
                .ToListAsync();
            }
                
        }
    }
}

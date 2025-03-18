using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Exceptions;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;

namespace ProducaoAPI.Repositories
{
    public class FormaRepository : IFormaRepository
    {
        private readonly IDbContextFactory<ProducaoContext> _contextFactory;
        public FormaRepository(IDbContextFactory<ProducaoContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<Forma>> ListarFormasAtivas()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var formas = await context.Formas
                .Where(m => m.Ativo == true)
                .Include(f => f.Maquinas)
                .Include(f => f.Produto)
                .ToListAsync();

                if (formas == null || formas.Count == 0) throw new NotFoundException("Nenhuma forma ativa encontrada.");
                return formas;
            }
        }

        public async Task<IEnumerable<Forma>> ListarTodasFormas()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var formas = await context.Formas
                .Include(f => f.Maquinas)
                .Include(f => f.Produto)
                .ToListAsync();

                if (formas == null || formas.Count == 0) throw new NotFoundException("Nenhuma forma encontrada.");
                return formas;
            }
        }

        public async Task<Forma> BuscarFormaPorIdAsync(int id)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var forma = await context.Formas
                .Include(f => f.Maquinas)
                .Include(f => f.Produto)
                .FirstOrDefaultAsync(f => f.Id == id);

                if (forma == null) throw new NotFoundException("ID da forma não encontrado.");
                return forma;
            }
        }

        public async Task AdicionarAsync(Forma forma)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                await context.Formas.AddAsync(forma);
                await context.SaveChangesAsync();
            }
        }

        public async Task AtualizarAsync(Forma forma)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                context.Formas.Update(forma);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<string>> ListarNomes()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.Formas
                .Select(f => f.Nome)
                .ToListAsync();
            }
        }
    }
}
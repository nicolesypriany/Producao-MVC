using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Exceptions;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;

namespace ProducaoAPI.Repositories
{
    public class MaquinaRepository : IMaquinaRepository
    {
        private readonly IDbContextFactory<ProducaoContext> _contextFactory;
        public MaquinaRepository(IDbContextFactory<ProducaoContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<Maquina>> ListarMaquinasAtivas()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var maquinas = await context.Maquinas
                .Where(m => m.Ativo == true)
                .ToListAsync();

                if (maquinas == null || maquinas.Count == 0) throw new NotFoundException("Nenhuma máquina ativa encontrada.");
                return maquinas;
            }
        }

        public async Task<IEnumerable<Maquina>> ListarTodasMaquinas()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var maquinas = await context.Maquinas
            .ToListAsync();

                if (maquinas == null || maquinas.Count == 0) throw new NotFoundException("Nenhuma máquina encontrada.");
                return maquinas;
            }
        }

        public async Task<Maquina> BuscarMaquinaPorIdAsync(int id)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var maquina = await context.Maquinas
                .FirstOrDefaultAsync(m => m.Id == id);

                if (maquina == null) throw new NotFoundException("ID da máquina não encontrado.");
                return maquina;
            }
        }

        public async Task AdicionarAsync(Maquina maquina)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                await context.Maquinas.AddAsync(maquina);
                await context.SaveChangesAsync();
            }
        }

        public async Task AtualizarAsync(Maquina maquina)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                context.Maquinas.Update(maquina);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<string>> ListarNomes()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.Maquinas
                .Select(m => m.Nome)
                .ToListAsync();
            }
        }
    }
}
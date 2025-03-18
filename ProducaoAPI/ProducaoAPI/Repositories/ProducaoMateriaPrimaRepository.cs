using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;

namespace ProducaoAPI.Repositories
{
    public class ProducaoMateriaPrimaRepository : IProducaoMateriaPrimaRepository
    {
        private readonly IDbContextFactory<ProducaoContext> _contextFactory;
        public ProducaoMateriaPrimaRepository(IDbContextFactory<ProducaoContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task AdicionarAsync(ProcessoProducaoMateriaPrima producaoMateriaPrima)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                await context.ProducoesMateriasPrimas.AddAsync(producaoMateriaPrima);
                await context.SaveChangesAsync();
            }

                
        }
    }
}

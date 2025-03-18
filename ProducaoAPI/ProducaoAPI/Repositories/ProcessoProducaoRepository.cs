using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Exceptions;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;

namespace ProducaoAPI.Repositories
{
    public class ProcessoProducaoRepository : IProcessoProducaoRepository
    {
        private readonly IDbContextFactory<ProducaoContext> _contextFactory;
        public ProcessoProducaoRepository(IDbContextFactory<ProducaoContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<ProcessoProducao>> ListarProducoesAtivas()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var producoes = await context.Producoes
            .Include(p => p.ProducaoMateriasPrimas)
            .ThenInclude(p => p.MateriaPrima)
            .Where(m => m.Ativo == true)
            .ToListAsync();

                if (producoes == null || producoes.Count == 0) throw new NotFoundException("Nenhuma produção encontrada.");
                return producoes;
            }

                
        }

        public async Task<IEnumerable<ProcessoProducao>> ListarTodasProducoes()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var producoes = await context.Producoes
            .Include(p => p.ProducaoMateriasPrimas)
            .ThenInclude(p => p.MateriaPrima)
            .ToListAsync();

                if (producoes == null || producoes.Count == 0) throw new NotFoundException("Nenhuma produção ativa encontrada.");
                return producoes;
            }

                
        }

        public async Task<ProcessoProducao> BuscarProducaoPorIdAsync(int id)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var producao = await context.Producoes
            .Include(p => p.ProducaoMateriasPrimas)
            .ThenInclude(p => p.MateriaPrima)
            .Where(m => m.Ativo == true)
            .FirstOrDefaultAsync(p => p.Id == id);

                if (producao == null) throw new NotFoundException("ID da produção não encontrado");
                return producao;
            }

                
        }

        public async Task AdicionarAsync(ProcessoProducao producao)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                await context.Producoes.AddAsync(producao);
                await context.SaveChangesAsync();
            }
                
        }

        public async Task AtualizarAsync(ProcessoProducao producao)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                context.Producoes.Update(producao);
                await context.SaveChangesAsync();
            }
                
        }

        public async Task<IEnumerable<ProcessoProducao>> ListarProducoesAtivasDetalhadas()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var producoes = await context.Producoes
          .Include(p => p.Maquina)
          .Include(p => p.Forma)
          .ThenInclude(p => p.Produto)
          .Include(p => p.ProducaoMateriasPrimas)
          .ThenInclude(p => p.MateriaPrima)
          .Where(m => m.Ativo == true)
          .ToListAsync();

                if (producoes == null || producoes.Count == 0) throw new NotFoundException("Nenhuma produção encontrada.");
                return producoes;
            }

              
        }
    }
}

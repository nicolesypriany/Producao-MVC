using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Exceptions;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;

namespace ProducaoAPI.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly IDbContextFactory<ProducaoContext> _contextFactory;
        public ProdutoRepository(IDbContextFactory<ProducaoContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<Produto>> ListarProdutosAtivos()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var produtos = await context.Produtos
                .Where(m => m.Ativo == true)
                .ToListAsync();

                if (produtos == null || produtos.Count == 0) throw new NotFoundException("Nenhum produto ativo.");
                return produtos;
            }

                
        }

        public async Task<IEnumerable<Produto>> ListarTodosProdutos()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var produtos = await context.Produtos
                .ToListAsync();

                if (produtos == null || produtos.Count == 0) throw new NotFoundException("Nenhum produto encontrado.");
                return produtos;
            }

                
        }

        public async Task<Produto> BuscarProdutoPorIdAsync(int id)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var produto = await context.Produtos
                .FirstOrDefaultAsync(m => m.Id == id);

                if (produto == null) throw new NotFoundException("ID do produto não encontrado.");
                return produto;
            }
                
        }

        public async Task AdicionarAsync(Produto produto)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                await context.Produtos.AddAsync(produto);
                await context.SaveChangesAsync();
            }
                
        }

        public async Task AtualizarAsync(Produto produto)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                context.Produtos.Update(produto);
                await context.SaveChangesAsync();
            }
                
        }

        public async Task<IEnumerable<string>> ListarNomes()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.Produtos
                .Select(p => p.Nome)
                .ToListAsync();
            }
                
        }
    }
}

using Microsoft.EntityFrameworkCore;
using OS_API.Data;
using OS_API.Interfaces.Repositories;
using OS_API.Models;

namespace OS_API.Repositories
{
    public class TipoAtendimentoRepository : ITipoAtendimentoRepository
    {
        private readonly AppDbContext _context;

        public TipoAtendimentoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TipoAtendimento> Adicionar(TipoAtendimento tipoAtendimento)
        {
            await _context.Set<TipoAtendimento>().AddAsync(tipoAtendimento);
            await _context.SaveChangesAsync();

            return tipoAtendimento;
        }

        public async Task<TipoAtendimento?> BuscarPorId(int id)
        {
            return await _context.Set<TipoAtendimento>()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> ExisteDescricao(string descricao)
        {
            return await _context.Set<TipoAtendimento>()
                .AnyAsync(x => x.Descricao == descricao);
        }

        public async Task<bool> ExisteDescricaoEmOutro(string descricao, int id)
        {
            return await _context.Set<TipoAtendimento>()
                .AnyAsync(x => x.Descricao == descricao && x.Id != id);
        }

        public async Task<List<TipoAtendimento>> Listar()
        {
            return await _context.Set<TipoAtendimento>()
                .ToListAsync();
        }

        public async Task Atualizar(TipoAtendimento tipoAtendimento)
        {
            if (_context.Entry(tipoAtendimento).State == EntityState.Detached)
                _context.Set<TipoAtendimento>().Update(tipoAtendimento);

            await _context.SaveChangesAsync();
        }

        public async Task Remover(TipoAtendimento tipoAtendimento)
        {
            _context.Set<TipoAtendimento>().Remove(tipoAtendimento);
            await _context.SaveChangesAsync();
        }
    }
}

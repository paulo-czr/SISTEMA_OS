using Microsoft.EntityFrameworkCore;
using OS_API.Data;
using OS_API.Interfaces.Repositories;
using OS_API.Models;

namespace OS_API.Repositories
{
    public class PermissaoRepository : IPermissaoRepository
    {
        private readonly AppDbContext _context;

        public PermissaoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PermissaoModel?> BuscarPorId(int id)
        {
            return await _context.Permissoes
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PermissaoModel?> BuscarPorNome(string nome)
        {
            return await _context.Permissoes
                .FirstOrDefaultAsync(x => x.Nome == nome);
        }

        public async Task<List<PermissaoModel>> Listar()
        {
            return await _context.Permissoes
                .ToListAsync();
        }
    }
}

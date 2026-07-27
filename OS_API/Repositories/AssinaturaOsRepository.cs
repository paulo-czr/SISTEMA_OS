using Microsoft.EntityFrameworkCore;
using OS_API.Data;
using OS_API.Interfaces.Repositories;
using OS_API.Models;
using OS_API.Models.Enum;

namespace OS_API.Repositories
{
    public class AssinaturaOsRepository : IAssinaturaOsRepository
    {
        private readonly AppDbContext _context;

        public AssinaturaOsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AssinaturaOsModel> Adicionar(AssinaturaOsModel assinatura)
        {
            try
            {
                await _context.Set<AssinaturaOsModel>().AddAsync(assinatura);
                await _context.SaveChangesAsync();
                return assinatura;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
            
        }

        public async Task<AssinaturaOsModel?> BuscarPorOsETipo(int idOs, TipoSignatario tipo)
        {
            return await _context.Set<AssinaturaOsModel>()
                .FirstOrDefaultAsync(x => x.IdOs == idOs && x.Tipo == tipo);
        }

        public async Task Atualizar(AssinaturaOsModel assinatura)
        {
            if (_context.Entry(assinatura).State == EntityState.Detached)
                _context.Set<AssinaturaOsModel>().Update(assinatura);

            await _context.SaveChangesAsync();
        }
    }
}
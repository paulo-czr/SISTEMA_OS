using Microsoft.EntityFrameworkCore;
using OS_API.Data;
using OS_API.Interfaces.Repositories;
using OS_API.Models;

namespace OS_API.Repositories
{
    public class OsFuncionarioRepository : IOsFuncionarioRepository
    {
        private readonly AppDbContext _context;

        public OsFuncionarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OsFuncionarioModel> AdicionarAsync(OsFuncionarioModel osFuncionario)
        {
            await _context.OsFuncionarios.AddAsync(osFuncionario);
            await _context.SaveChangesAsync();

            return osFuncionario;
        }

        public async Task<bool> RemoverAsync(int idOsFuncionario)
        {
            var osFuncionario = await _context.OsFuncionarios
                .FirstOrDefaultAsync(x => x.IdOsFuncionario == idOsFuncionario);

            if (osFuncionario == null)
                return false;

            _context.OsFuncionarios.Remove(osFuncionario);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<OsFuncionarioModel>> ObterPorOsAsync(int idOs)
        {
            return await _context.OsFuncionarios
                .Include(x => x.funcionario)
                .Where(x => x.IdOs == idOs)
                .ToListAsync();
        }

        public async Task<OsFuncionarioModel?> ObterPorIdAsync(int idOsFuncionario)
        {
            return await _context.OsFuncionarios
                .Include(x => x.funcionario)
                .FirstOrDefaultAsync(x => x.IdOsFuncionario == idOsFuncionario);
        }

        public async Task<OsFuncionarioModel> AlterarResponsavelAsync(int idOs, int idFuncionario)
        {
            var vinculos = await _context.OsFuncionarios
                .Where(x => x.IdOs == idOs)
                .ToListAsync();

            foreach (var vinculo in vinculos)
                vinculo.Responsavel = vinculo.IdFuncionario == idFuncionario;

            await _context.SaveChangesAsync();

            return vinculos.First(x => x.IdFuncionario == idFuncionario);
        }
    }
}

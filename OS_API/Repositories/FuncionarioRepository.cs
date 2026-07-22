using Microsoft.EntityFrameworkCore;
using OS_API.Data;
using OS_API.Interfaces.Repositories;
using OS_API.Models;

namespace OS_API.Repositories
{
    public class FuncionarioRepository : IFuncionarioRepository
    {
        private readonly AppDbContext _context;

        public FuncionarioRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<FuncionarioModel> Adicionar(FuncionarioModel tecnico)
        {
            await _context.Funcionarios.AddAsync(tecnico);
            await _context.SaveChangesAsync();

            return tecnico;
        }


        public async Task<FuncionarioModel?> BuscarPorId(int id)
        {
            return await _context.Funcionarios
                .FirstOrDefaultAsync(x => x.Id == id);
        }


        public async Task<List<FuncionarioModel>> Listar()
        {
            return await _context.Funcionarios
                .ToListAsync();
        }


        public async Task Atualizar(FuncionarioModel tecnico)
        {
            _context.Funcionarios.Update(tecnico);
            await _context.SaveChangesAsync();
        }


        public async Task Remover(FuncionarioModel tecnico)
        {
            _context.Funcionarios.Remove(tecnico);
            await _context.SaveChangesAsync();
        }
    }
}

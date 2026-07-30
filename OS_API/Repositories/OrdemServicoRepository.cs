using Microsoft.EntityFrameworkCore;
using OS_API.Data;
using OS_API.Interfaces.Repositories;
using OS_API.Models;
using OS_API.Models.Cliente;

namespace OS_API.Repositories
{
    public class OrdemServicoRepository : IOrdemServicoRepository
    {
        private readonly AppDbContext _context;

        public OrdemServicoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OrdemServicoModel> Adicionar(OrdemServicoModel ordemServico)
        {
            await _context.OrdensServico.AddAsync(ordemServico);
            await _context.SaveChangesAsync();

            return ordemServico;
        }



        public async Task<OrdemServicoModel?> BuscarPorId(int id)
        {
            return await _context.OrdensServico
                .Include(o => o.Cliente)
                .Include(o => o.TipoAtendimento)
                .Include(o => o.Funcionarios)
                    .ThenInclude(f => f.funcionario)
                .FirstOrDefaultAsync(x => x.IdOs == id);
        }

        public async Task<List<OrdemServicoModel>> BuscarPorIdUsuarioFuncionario(string idUsuario)
        {
            return await _context.OrdensServico
                .Include(o => o.Cliente)
                .Include(o => o.TipoAtendimento)
                .Include(o => o.Funcionarios)
                    .ThenInclude(f => f.funcionario)
                .Where(x => x.Funcionarios.Any(f => f.funcionario.UsuarioId == idUsuario))
                .ToListAsync();
        }

        public async Task<List<OrdemServicoModel>> Listar()
        {
            return await _context.OrdensServico
                .Include(o => o.Cliente)
                .Include(o => o.TipoAtendimento)
                .Include(o => o.Funcionarios)
                    .ThenInclude(f => f.funcionario)
                .ToListAsync();
        }

        public async Task Atualizar(OrdemServicoModel ordemServico)
        {
            if (_context.Entry(ordemServico).State == EntityState.Detached)
                _context.OrdensServico.Update(ordemServico);

            await _context.SaveChangesAsync();
        }

        public async Task Remover(OrdemServicoModel ordemServico)
        {
            _context.OrdensServico.Remove(ordemServico);
            await _context.SaveChangesAsync();
        }

        public async Task<OrdemServicoModel?> BuscarPorToken(string token)
        {
            return await _context.OrdensServico
                .Include(o => o.Cliente)
                .Include(o => o.TipoAtendimento)
                .Include(o => o.Funcionarios)
                    .ThenInclude(f => f.funcionario)
                .FirstOrDefaultAsync(x => x.TokenAssinaturaCliente == token);
        }

        public async Task<OrdemServicoModel?> BuscarPorTipoAtendimento(TipoAtendimento tipo)
        {
            return await _context.OrdensServico
               .Include(o => o.Cliente)
               .Include(o => o.TipoAtendimento)
               .Include(o => o.Funcionarios)
                   .ThenInclude(f => f.funcionario)
               .FirstOrDefaultAsync(x => x.TipoAtendimento.Id == tipo.Id);

        }

        public async Task<OrdemServicoModel?> BuscarPorCliente(ClienteModel cliente)
        {
            return await _context.OrdensServico
                .Include(o => o.Cliente)
                .Include(o => o.TipoAtendimento)
                .Include(o => o.Funcionarios)
                    .ThenInclude(f => f.funcionario)
                .FirstOrDefaultAsync(x => x.Cliente.IdCliente == cliente.IdCliente);
        }
    }
}

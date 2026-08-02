using Microsoft.EntityFrameworkCore;
using OS_API.Data;
using OS_API.DTOs.OrdemServico.Filtro;
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

        public async Task<(List<OrdemServicoModel> Itens, int Total)> ListarPaginado(FiltroOrdemServicoDto filtro, string? idUsuarioFuncionario)
        {
            var query = _context.OrdensServico
                .Include(o => o.Cliente)
                .Include(o => o.TipoAtendimento)
                .Include(o => o.Funcionarios)
                    .ThenInclude(f => f.funcionario)
                .AsQueryable();

            if (idUsuarioFuncionario != null)
                query = query.Where(o => o.Funcionarios.Any(f => f.funcionario.UsuarioId == idUsuarioFuncionario));

            if (filtro.Status != null)
                query = query.Where(o => o.Status == filtro.Status);

            if (filtro.IdCliente != null)
                query = query.Where(o => o.IdCliente == filtro.IdCliente);

            if (filtro.IdTipoAtendimento != null)
                query = query.Where(o => o.IdTipoAtendimento == filtro.IdTipoAtendimento);

            if (filtro.DataInicioDe != null)
                query = query.Where(o => o.DataHoraInicio >= filtro.DataInicioDe);

            if (filtro.DataInicioAte != null)
                query = query.Where(o => o.DataHoraInicio <= filtro.DataInicioAte);

            if (filtro.DataFimDe != null)
                query = query.Where(o => o.DataHoraFim >= filtro.DataFimDe);

            if (filtro.DataFimAte != null)
                query = query.Where(o => o.DataHoraFim <= filtro.DataFimAte);

            if (!string.IsNullOrWhiteSpace(filtro.Busca))
            {
                var termo = filtro.Busca.Trim().ToLower();
                query = query.Where(o =>
                    o.TituloOs.ToLower().Contains(termo) ||
                    o.Cliente.NomeFantasia.ToLower().Contains(termo));
            }

            query = query.OrderByDescending(o => o.DataHoraInicio);

            var total = await query.CountAsync();

            var itens = await query
                .Skip((filtro.Pagina - 1) * filtro.TamanhoPagina)
                .Take(filtro.TamanhoPagina)
                .ToListAsync();

            return (itens, total);
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
        
        public async Task<OrdemServicoModel?> BuscarPorTokenFotos(string token)
        {
            return await _context.OrdensServico
                .Include(o => o.Cliente)
                .FirstOrDefaultAsync(x => x.TokenFotos == token);
        }
    }
}

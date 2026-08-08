using Microsoft.EntityFrameworkCore;
using OS_API.Data;
using OS_API.DTOs.Cliente.Filtro;
using OS_API.Interfaces.Repositories;
using OS_API.Models.Cliente;

namespace OS_API.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly AppDbContext _context;

        public ClienteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ClienteModel> Adicionar(ClienteModel cliente)
        {
            await _context.Clientes.AddAsync(cliente);
            await _context.SaveChangesAsync();

            return cliente;
        }

        public async Task<ClienteModel?> BuscarPorId(int id)
        {
            return await _context.Clientes
                .FirstOrDefaultAsync(x => x.IdCliente == id);
        }

        public async Task<ClienteModel?> BuscarPorDocumento(string documento)
        {
            return await _context.Clientes
                .FirstOrDefaultAsync(x => x.Documento == documento);
        }

        public async Task<bool> ExisteDocumento(string documento)
        {
            return await _context.Clientes
                .AnyAsync(x => x.Documento == documento);
        }

        public async Task<bool> ExisteDocumentoEmOutroCliente(string documento, int idCliente)
        {
            return await _context.Clientes
                .AnyAsync(x => x.Documento == documento && x.IdCliente != idCliente);
        }

        public async Task<bool> ExisteEmailEmOutroCliente(string email, int idCliente)
        {
            return await _context.Clientes
                .AnyAsync(x => x.Email == email && x.IdCliente != idCliente);
        }

        public async Task<List<ClienteModel>> Listar()
        {
            return await _context.Clientes
                .ToListAsync();
        }

        public async Task<(List<ClienteModel> Itens, int Total)> ListarPaginado(FiltroClienteDto filtro)
        {
            var query = _context.Clientes.AsQueryable();
            query.Where(c => c.Ativo);
            if (!string.IsNullOrWhiteSpace(filtro.Busca))
            {
                var termo = filtro.Busca.Trim().ToLower();
                query = query.Where(c =>
                    c.NomeFantasia.ToLower().Contains(termo) ||
                    c.RazaoSocial.ToLower().Contains(termo)||
                    c.Documento.Contains(termo));
            }

            query = query.OrderByDescending(c => c.IdCliente);

            var total = await query.CountAsync();

            var itens = await query
                .Skip((filtro.Pagina - 1) * filtro.TamanhoPagina)
                .Take(filtro.TamanhoPagina)
                .ToListAsync();

            return (itens, total);

        }

        public async Task Atualizar(ClienteModel cliente)
        {
            // Evitando marcar o objeto inteiro como modificado sem necessidade.
            if (_context.Entry(cliente).State == EntityState.Detached)
                _context.Clientes.Update(cliente);

            await _context.SaveChangesAsync();
        }

        public async Task Remover(ClienteModel cliente)
        {
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
        }

        
    }
}
using Microsoft.EntityFrameworkCore;
using OS_API.Data;
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
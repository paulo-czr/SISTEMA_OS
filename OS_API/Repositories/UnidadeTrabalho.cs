using Microsoft.EntityFrameworkCore.Storage;
using OS_API.Data;
using OS_API.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_API.Repositories
{
    public class UnidadeTrabalho : IUnidadeTrabalho
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _transacao;

        public UnidadeTrabalho(AppDbContext context)
        {
            _context = context;
        }

        public async Task IniciarTransacaoAsync()
        {
            _transacao = await _context.Database.BeginTransactionAsync();
        }

        public async Task ConfirmarTransacaoAsync()
        {
            if (_transacao != null)
            {
                await _transacao.CommitAsync();
                await _transacao.DisposeAsync();
            }
        }

        public async Task DesfazerTransacaoAsync()
        {
            if (_transacao != null)
            {
                await _transacao.RollbackAsync();
                await _transacao.DisposeAsync();
            }
        }

        public async Task<int> SalvarAlteracoesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}

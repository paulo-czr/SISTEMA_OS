using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_API.Interfaces.Repositories
{
    public interface IUnidadeTrabalho
    {
        Task IniciarTransacaoAsync();
        Task ConfirmarTransacaoAsync();
        Task DesfazerTransacaoAsync();
        Task<int> SalvarAlteracoesAsync();
    }
}

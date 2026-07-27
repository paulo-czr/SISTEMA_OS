using OS_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_API.Interfaces.Repositories
{
    public interface IOsFuncionarioRepository
    {
        Task<OsFuncionarioModel> AdicionarAsync(OsFuncionarioModel osFuncionario);

        Task<bool> RemoverAsync(int idOsFuncionario);

        Task<List<OsFuncionarioModel>> ObterPorOsAsync(int idOs);

        Task<OsFuncionarioModel> BuscarPorIdOsFunc(int idOs, int idFuncionario);

        Task<OsFuncionarioModel> BuscarFuncionarioResponsavel(int idOs);

        Task<OsFuncionarioModel?> ObterPorIdAsync(int idOsFuncionario);

        Task<OsFuncionarioModel> AlterarResponsavelAsync(int idOs, int idFuncionario);
    }
}

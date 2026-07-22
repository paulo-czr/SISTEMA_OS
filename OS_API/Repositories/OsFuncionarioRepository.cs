using OS_API.Interfaces.Repositories;
using OS_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_API.Repositories
{
    public class OsFuncionarioRepository : IOsFuncionarioRepository
    {
        public Task<OsFuncionarioModel> AdicionarAsync(OsFuncionarioModel osFuncionario)
        {
            throw new NotImplementedException();
        }

        public Task<OsFuncionarioModel> AlterarResponsavelAsync(int idOs, int idFuncionario)
        {
            throw new NotImplementedException();
        }

        public Task<OsFuncionarioModel?> ObterPorIdAsync(int idOsFuncionario)
        {
            throw new NotImplementedException();
        }

        public Task<List<OsFuncionarioModel>> ObterPorOsAsync(int idOs)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoverAsync(int idOsFuncionario)
        {
            throw new NotImplementedException();
        }
    }
}

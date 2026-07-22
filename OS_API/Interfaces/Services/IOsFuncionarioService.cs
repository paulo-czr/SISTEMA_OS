using OS_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_API.Interfaces.Services
{
    public interface IOsFuncionarioService
    {
        Task AdicionarTecnicoAsync(int idOs, int idFuncionario, bool responsavel);

        Task RemoverTecnicoAsync(int idOsFuncionario);

        Task DefinirResponsavelAsync(int idOs, int idFuncionario);

        Task<List<OsFuncionarioModel>> ObterTecnicosDaOsAsync(int idOs);
    }
}

using OS_API.Interfaces.Repositories;
using OS_API.Interfaces.Services;
using OS_API.Models;

namespace OS_API.Services
{
    public class OsFuncionarioService : IOsFuncionarioService
    {
        private readonly IOsFuncionarioRepository _repository;

        public OsFuncionarioService(IOsFuncionarioRepository repository)
        {
            _repository = repository;
        }

        public async Task AdicionarTecnicoAsync(int idOs, int idFuncionario, bool responsavel)
        {
            // Validar se o funcionário já está vinculado a essa OS (evitar duplicidade).
            // Validar se o funcionário existe e está ativo.

            var osFuncionario = new OsFuncionarioModel
            {
                IdOs = idOs,
                IdFuncionario = idFuncionario,
                Responsavel = responsavel
            };

            await _repository.AdicionarAsync(osFuncionario);
        }

        public async Task RemoverTecnicoAsync(int idOsFuncionario)
        {
            // Validar se, ao remover, a OS não fica sem nenhum responsável.

            await _repository.RemoverAsync(idOsFuncionario);
        }

        public async Task DefinirResponsavelAsync(int idOs, int idFuncionario)
        {
            // Validar se o funcionário informado realmente está vinculado a essa OS.

            await _repository.AlterarResponsavelAsync(idOs, idFuncionario);
        }

        public async Task<List<OsFuncionarioModel>> ObterTecnicosDaOsAsync(int idOs)
        {
            return await _repository.ObterPorOsAsync(idOs);
        }
    }
}

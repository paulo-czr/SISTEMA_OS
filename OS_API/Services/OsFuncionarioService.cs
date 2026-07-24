using OS_API.DTOs.OSFuncionario;
using OS_API.Exceptionn;
using OS_API.Interfaces.Repositories;
using OS_API.Interfaces.Services;
using OS_API.Mappings;
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
            // Verifica se o funcionário já está vinculado a essa OS (evita duplicidade).
            var vinculosDaOs = await _repository.ObterPorOsAsync(idOs);

            if (vinculosDaOs.Any(v => v.IdFuncionario == idFuncionario))
                throw new ConflitoException("Esse funcionário já está vinculado a essa OS.");

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
            var vinculo = await _repository.ObterPorIdAsync(idOsFuncionario);

            if (vinculo == null)
                throw new EntidadeNaoEncontradaException("Vínculo entre OS e funcionário não encontrado.");

            // Não deixa remover o responsável se ele for o único funcionário vinculado à OS
            // (assim a OS nunca fica sem ninguém responsável).
            if (vinculo.Responsavel)
            {
                var vinculosDaOs = await _repository.ObterPorOsAsync(vinculo.IdOs);

                if (vinculosDaOs.Count > 1)
                    throw new ValidacaoException("Defina outro funcionário como responsável antes de remover este.");
            }

            await _repository.RemoverAsync(idOsFuncionario);
        }

        public async Task DefinirResponsavelAsync(int idOs, int idFuncionario)
        {
            // Verifica se o funcionário informado realmente está vinculado a essa OS.
            var vinculosDaOs = await _repository.ObterPorOsAsync(idOs);

            if (!vinculosDaOs.Any(v => v.IdFuncionario == idFuncionario))
                throw new EntidadeNaoEncontradaException("Esse funcionário não está vinculado a essa OS.");

            await _repository.AlterarResponsavelAsync(idOs, idFuncionario);
        }

        public async Task<List<OsFuncionarioDetalheDto>> ObterTecnicosDaOsAsync(int idOs)
        {
            var vinculos = await _repository.ObterPorOsAsync(idOs);

            return vinculos
                .Select(OsFuncionarioMapper.ParaDto)
                .ToList();
        }
    }
}

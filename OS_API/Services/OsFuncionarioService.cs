using OS_API.Interfaces.Repositories;
using OS_API.Interfaces.Services;
using OS_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_API.Services
{
    public class OsFuncionarioService : IOsFuncionarioService
    {
        private readonly IOsFuncionarioRepository _repository;

        public OsFuncionarioService(IOsFuncionarioRepository repository)
        {
            _repository = repository;
        }

        public async Task<OsFuncionarioModel> AdicionarAsync(OsFuncionarioModel osFuncionario)
        {
            // Regras de negócio aqui

            return await _repository.AdicionarAsync(osFuncionario);
        }

        public async Task<bool> RemoverAsync(int idOsFuncionario)
        {
            return await _repository.RemoverAsync(idOsFuncionario);
        }

        public async Task<List<OsFuncionarioModel>> ObterPorOsAsync(int idOs)
        {
            return await _repository.ObterPorOsAsync(idOs);
        }

        public async Task<OsFuncionarioModel?> ObterPorIdAsync(int idOsFuncionario)
        {
            return await _repository.ObterPorIdAsync(idOsFuncionario);
        }

        public async Task<OsFuncionarioModel> AlterarResponsavelAsync(int idOs, int idFuncionario)
        {
            // Aqui você pode implementar a regra:
            // 1. Remove o responsável atual.
            // 2. Define o novo responsável.

            return await _repository.AlterarResponsavelAsync(idOs, idFuncionario);
        }


        // Métodos que implementei para poder compilar. Ajustar depois de acordo

        public Task AdicionarTecnicoAsync(int idOs, int idFuncionario, bool responsavel)
        {
            throw new NotImplementedException();
        }

        public Task RemoverTecnicoAsync(int idOsFuncionario)
        {
            throw new NotImplementedException();
        }

        public Task DefinirResponsavelAsync(int idOs, int idFuncionario)
        {
            throw new NotImplementedException();
        }

        public Task<List<OsFuncionarioModel>> ObterTecnicosDaOsAsync(int idOs)
        {
            throw new NotImplementedException();
        }
    }
}

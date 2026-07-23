using OS_API.DTOs.OrdemServico;
using OS_API.Exceptionn;
using OS_API.Interfaces.Repositories;
using OS_API.Interfaces.Services;
using OS_API.Mappings;
using OS_API.Models;

namespace OS_API.Services
{
    public class OrdemServicoService : IOrdemServicoService
    {
        private readonly IOrdemServicoRepository _repository;
        private readonly IOsFuncionarioRepository _osFuncionarioRepository;
        private readonly IUnidadeTrabalho _unidadeTrabalho;
        private readonly IClienteService _clienteService;

        public OrdemServicoService(
            IOrdemServicoRepository repository,
            IOsFuncionarioRepository osFuncionarioRepository,
            IUnidadeTrabalho unidadeTrabalho,
            IClienteService clienteService)
        {
            _repository = repository;
            _osFuncionarioRepository = osFuncionarioRepository;
            _unidadeTrabalho = unidadeTrabalho;
            _clienteService = clienteService;
        }

        public async Task<BuscarOrdemServicoDto> Criar(CriarOrdemServicoDto dto)
        {
            // Validar se o Cliente informado existe.
            var cliente = await _clienteService.BuscarOuFalhar(dto.IdCliente);

            // Validar se o Tipo de Atendimento informado existe.

            // Validar se existe apenas um funcionário marcado como responsável em dto.Funcionarios.
            if (dto.Funcionarios.Count(f => f.Responsavel = true) != 1)
            {
                throw new EntidadeNaoEncontradaException("Deve existir exatamente um funcionário responsável.");
            }

            var ordemServico = OrdemServicoMapper.ParaModel(dto);

            await _unidadeTrabalho.IniciarTransacaoAsync();

            try
            {
                ordemServico = await _repository.Adicionar(ordemServico);

                // Salvar o relacionamento dos funcionários vinculados à OS.
                foreach (var funcionarioDto in dto.Funcionarios)
                {
                    var osFuncionario = new OsFuncionarioModel
                    {
                        IdOs = ordemServico.IdOs,
                        IdFuncionario = funcionarioDto.IdFuncionario,
                        Responsavel = funcionarioDto.Responsavel
                    };

                    await _osFuncionarioRepository.AdicionarAsync(osFuncionario);
                }

                await _unidadeTrabalho.ConfirmarTransacaoAsync();
            }
            catch
            {
                await _unidadeTrabalho.DesfazerTransacaoAsync();
                throw;
            }

            //var ordemServicoCompleta = await BuscarOuFalhar(ordemServico.IdOs);

            return OrdemServicoMapper.ParaDto(ordemServico);
        }

        public async Task<BuscarOrdemServicoDto> Atualizar(int id, AtualizarOrdemServicoDto dto)
        {
            // Validar se o OS
            var ordemServico = await BuscarOuFalhar(id);

            // Validar se o Cliente informado existe (caso esteja sendo alterado).
            // Validar se o Tipo de Atendimento informado existe (caso esteja sendo alterado).

            // Validar as regras de transição de Status (ex.: não permitir voltar de Concluída para Agendada).

            OrdemServicoMapper.AtualizarModel(ordemServico, dto);

            await _repository.Atualizar(ordemServico);

            return OrdemServicoMapper.ParaDto(ordemServico);
        }

        public async Task<BuscarOrdemServicoDto?> BuscarPorId(int id)
        {
            var ordemServico = await BuscarOuFalhar(id);

            return OrdemServicoMapper.ParaDto(ordemServico);
        }

        public async Task<List<BuscarOrdemServicoDto>> Listar()
        {
            var ordensServico = await _repository.Listar();

            return ordensServico
                .Select(OrdemServicoMapper.ParaDto)
                .ToList();
        }

        public async Task Remover(int id)
        {
            var ordemServico = await BuscarOuFalhar(id);

            // Validar se a OS pode ser removida (ex.: não permitir remoção de OS já concluída).

            await _repository.Remover(ordemServico);
        }

        private async Task<OrdemServicoModel> BuscarOuFalhar(int id)
        {
            var ordemServico = await _repository.BuscarPorId(id);

            if (ordemServico == null)
                throw new EntidadeNaoEncontradaException("Cliente não encontrada.");

            return ordemServico;
        }
    }
}

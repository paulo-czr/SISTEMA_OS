using OS_API.DTOs.OrdemServico;
using OS_API.Exceptionn;
using OS_API.Helpers.UsuarioLogado;
using OS_API.Interfaces.Repositories;
using OS_API.Interfaces.Services;
using OS_API.Mappings;
using OS_API.Migrations;
using OS_API.Models;
using OS_API.Models.Enum;

namespace OS_API.Services
{
    public class OrdemServicoService : IOrdemServicoService
    {
        private readonly IOrdemServicoRepository _repository;
        private readonly IOsFuncionarioRepository _osFuncionarioRepository;
        private readonly IOsFuncionarioService _osFuncionarioService;
        private readonly IUnidadeTrabalho _unidadeTrabalho;
        private readonly IClienteService _clienteService;
        private readonly ITipoAtendimentoService _TipoAtendimento;
        private readonly IUsuarioLogado _usuarioLogado;
        private readonly IUsuarioService _usuarioService;
        

        public OrdemServicoService(
            IOrdemServicoRepository repository,
            IOsFuncionarioRepository osFuncionarioRepository,
            IUnidadeTrabalho unidadeTrabalho,
            IClienteService clienteService,
            ITipoAtendimentoService tipoAtendimento,
            IUsuarioLogado usuarioLogado,
            IUsuarioService usuarioService,
            IOsFuncionarioService osFuncionarioService)
        {
            _repository = repository;
            _unidadeTrabalho = unidadeTrabalho;
            _clienteService = clienteService;
            _TipoAtendimento = tipoAtendimento;
            _usuarioLogado = usuarioLogado;
            _osFuncionarioService = osFuncionarioService;
            _osFuncionarioRepository = osFuncionarioRepository;
        }

        public async Task<BuscarOrdemServicoDto> Criar(CriarOrdemServicoDto dto)
        {
            // Validar se o Cliente informado existe.
            var cliente = await _clienteService.BuscarClienteOuFalhar(dto.IdCliente);

            // Validar se o Tipo de Atendimento informado existe.
            var tipoAten = await _TipoAtendimento.BuscarOuFalhar(dto.IdCliente);


            // Validar se existe apenas um funcionário marcado como responsável em dto.Funcionarios.
            if (dto.Funcionarios.Count(f => f.Responsavel) != 1)
            {
                throw new EntidadeNaoEncontradaException("Deve existir exatamente um funcionário responsável.");
            }
            // Validar se o funcionário não foi mandado varias vezes
            if (dto.Funcionarios.GroupBy(f => f.IdFuncionario).Any(g => g.Count() > 1))
            {
                throw new EntidadeNaoEncontradaException("Um funcionário não pode ser informado mais de uma vez.");
            }

            //pegar o usuario que registrou
            var idUsuario = _usuarioLogado.retornarUserLogado();

            var ordemServico = OrdemServicoMapper.ParaModel(dto, idUsuario);

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

            var ordemServicoCompleta = await BuscarOuFalhar(ordemServico.IdOs);


            return OrdemServicoMapper.ParaDto(ordemServicoCompleta);
        }

        public async Task<BuscarOrdemServicoDto> Atualizar(int id, AtualizarOrdemServicoDto dto)
        {
            // Validar se o OS
            var ordemServico = await BuscarOuFalhar(id);
            // Validar se o Cliente informado existe.
            var cliente = await _clienteService.BuscarClienteOuFalhar(dto.IdCliente);
            // Validar se o Tipo de Atendimento informado existe.
            var tipoAten = await _TipoAtendimento.BuscarOuFalhar(dto.IdCliente);

            // não atualizar se tiver concluida
            falharSeOSConcluida(ordemServico);

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

        // Só atualiza o relatório técnico, sem tocar em mais nenhum campo da OS.
        public async Task<BuscarOrdemServicoDto> AtualizarRelatorio(int id, AtualizarRelatorioDto dto)
        {
            var ordemServico = await BuscarOuFalhar(id);
            falharSeOSConcluida(ordemServico);
            //buscar id funcionario logado
            int idFuncionario = await _usuarioLogado.RetornarIdFuncionarioLogado();
            //validar se o funcionario está na OS e é o responsavel
            if(!await _osFuncionarioService.VerificarTecnicoEResponsavelAsync(ordemServico.IdOs, idFuncionario))
            {
                throw new ValidacaoException("Somente o funcionario pode inserir o relatorio.");
            }
            ordemServico.RelatorioTecnico = dto.RelatorioTecnico;

            await _repository.Atualizar(ordemServico);

            return OrdemServicoMapper.ParaDto(ordemServico);
        }

        // Só atualiza o status da OS, sem tocar em mais nenhum campo.
        public async Task<BuscarOrdemServicoDto> AlterarStatus(int id, AlterarStatusOsDto dto)
        {
            var ordemServico = await BuscarOuFalhar(id);

            // Validar a transição de status 
            falharSeOSConcluida(ordemServico);
            ordemServico.Status = dto.Status;

            await _repository.Atualizar(ordemServico);

            return OrdemServicoMapper.ParaDto(ordemServico);
        }

        public async Task Remover(int id)
        {
            var ordemServico = await BuscarOuFalhar(id);

            // Validar se a OS pode ser removida (ex.: não permitir remoção de OS já concluída).

            await _repository.Remover(ordemServico);
        }

        private void falharSeOSConcluida(OrdemServicoModel ordemServico)
        {
            if (ordemServico.Status == StatusOs.Concluida)
            {
                throw new ValidacaoException("OS ja concluida.");
            }
        }
        private async Task<OrdemServicoModel> BuscarOuFalhar(int id)
        {
            var ordemServico = await _repository.BuscarPorId(id);

            if (ordemServico == null)
                throw new EntidadeNaoEncontradaException("Ordem de Serviço não encontrada.");

            return ordemServico;
        }
    }
}

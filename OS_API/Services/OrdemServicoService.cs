using OS_API.DTOs.Assinatura;
using OS_API.DTOs.OrdemServico;
using OS_API.Exceptionn;
using OS_API.Helpers.Constantes;
using OS_API.Helpers.UsuarioLogado;
using OS_API.Interfaces.Repositories;
using OS_API.Interfaces.Services;
using OS_API.Mappings;
using OS_API.Migrations;
using OS_API.Models;
using OS_API.Models.Enum;
using System.Data;
using System.Security.Cryptography;

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

        private readonly IAssinaturaOsRepository _assinaturaRepository;  
        private readonly IFuncionarioService _funcionarioService;        
        private readonly IHttpContextAccessor _httpContextAccessor;


        public OrdemServicoService(
            IOrdemServicoRepository repository,
            IOsFuncionarioRepository osFuncionarioRepository,
            IUnidadeTrabalho unidadeTrabalho,
            IClienteService clienteService,
            ITipoAtendimentoService tipoAtendimento,
            IUsuarioLogado usuarioLogado,
            IUsuarioService usuarioService,
            IOsFuncionarioService osFuncionarioService,
            IAssinaturaOsRepository assinaturaRepository,   
            IFuncionarioService funcionarioService,         
            IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _unidadeTrabalho = unidadeTrabalho;
            _clienteService = clienteService;
            _TipoAtendimento = tipoAtendimento;
            _usuarioLogado = usuarioLogado;
            _osFuncionarioService = osFuncionarioService;
            _osFuncionarioRepository = osFuncionarioRepository;
            _assinaturaRepository = assinaturaRepository;
            _funcionarioService = funcionarioService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BuscarOrdemServicoDto> Criar(CriarOrdemServicoDto dto)
        {
            // Validar se o Cliente informado existe.
            var cliente = await _clienteService.BuscarClienteOuFalhar(dto.IdCliente);

            // Validar se o Tipo de Atendimento informado existe.
            var tipoAten = await _TipoAtendimento.BuscarOuFalhar(dto.IdTipoAtendimento);

            FalharDataInvalidas(dto.Prazo);


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

           

            try
            {
                await _unidadeTrabalho.IniciarTransacaoAsync();

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
            var tipoAten = await _TipoAtendimento.BuscarOuFalhar(dto.IdTipoAtendimento);

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
            ICollection<OrdemServicoModel> ordensServico = new List<OrdemServicoModel>();

            //verificar se tem a permissão para visualizar todas as OS
            var permitido = await _usuarioLogado.VerificarSeTemPermissao(Permissoes.OSVisualizarTodas);
            if (permitido)
            {
                 ordensServico = await _repository.Listar();
            }
            else
            {
                ordensServico = await _repository.BuscarPorIdUsuarioFuncionario(_usuarioLogado.IdUsuario!);
            }


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
                throw new ValidacaoException("Somente o funcionario responsavel pode inserir o relatorio.");
            }

            ordemServico.RelatorioTecnico = dto.RelatorioTecnico;

            await _repository.Atualizar(ordemServico);

            return OrdemServicoMapper.ParaDto(ordemServico);
        }

        // Só atualiza o status da OS, sem tocar em mais nenhum campo.
        public async Task<BuscarOrdemServicoDto> AlterarStatus(int id, AlterarStatusOsDto dto)
        {
            var ordemServico = await BuscarOuFalhar(id);

            // Validar se os esta concluida
            falharSeOSConcluida(ordemServico);

            if(dto.Status != StatusOs.Cancelada || dto.Status != StatusOs.Concluida)
            {
                ordemServico.DataHoraInicio = DateTime.UtcNow;
            }
           
            ordemServico.Status = dto.Status;
            await _repository.Atualizar(ordemServico);
            return OrdemServicoMapper.ParaDto(ordemServico);
        }

        public async Task Remover(int id)
        {
            var ordemServico = await BuscarOuFalhar(id);
            // Validar se a OS pode ser removida (ex.: não permitir remoção de OS já concluída).
            falharSeOSConcluida(ordemServico);
            await _repository.Remover(ordemServico);
        }

        private void falharSeOSConcluida(OrdemServicoModel ordemServico)
        {
            if (ordemServico.Status == StatusOs.Concluida)
            {
                throw new ValidacaoException("OS já concluída.");
            }
        }
        private async Task<OrdemServicoModel> BuscarOuFalhar(int id)
        {
            var ordemServico = await _repository.BuscarPorId(id);

            if (ordemServico == null)
                throw new EntidadeNaoEncontradaException("Ordem de Serviço não encontrada.");

            return ordemServico;
        }

        //-----------------------------------------------------------
        // Funcionário responsável assina e gera o link/token de assinatura pro cliente.
        public async Task<TokenAssinaturaDto> IniciarAssinatura(int id, IniciarAssinaturaDto dto)
        {
            try
            {
                await _unidadeTrabalho.IniciarTransacaoAsync();
                var ordemServico = await BuscarOuFalhar(id);
                falharSeOSConcluida(ordemServico);

                if (ordemServico.DataHoraFim == null)
                {
                    ordemServico.DataHoraFim = DateTime.UtcNow;
                }

                int idFuncionario = await _usuarioLogado.RetornarIdFuncionarioLogado();

                if (!await _osFuncionarioService.VerificarTecnicoEResponsavelAsync(ordemServico.IdOs, idFuncionario))
                {
                    throw new ValidacaoException("Somente o funcionário responsável pode gerar o relatório.");
                }

                var funcionario = await _funcionarioService.BuscarPorId(idFuncionario);
                if (funcionario == null)
                {
                    throw new EntidadeNaoEncontradaException("Funcionário não encontrado.");
                }

                if (string.IsNullOrWhiteSpace(ordemServico.RelatorioTecnico))
                {
                    throw new ValidacaoException("Preencha o relatório técnico antes de gerar o relatório assinado.");
                }

                var http = _httpContextAccessor.HttpContext;
                var ip = http?.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
                var userAgent = http?.Request.Headers["User-Agent"].ToString() ?? string.Empty;

                // Substitui a assinatura do funcionário já registrada nessa OS, se houver
                // (permite assinar de novo, trocando a que tinha sido usada antes).
                var assinaturaFuncionario = await _assinaturaRepository.BuscarPorOsETipo(id, TipoSignatario.Funcionario);
                if (assinaturaFuncionario != null)
                {
                    assinaturaFuncionario.ImagemAssinatura = dto.ImagemAssinaturaFuncionario;
                    assinaturaFuncionario.DataAssinatura = DateTime.UtcNow;
                    await _assinaturaRepository.Atualizar(assinaturaFuncionario);
                }
                else
                {
                    await _assinaturaRepository.Adicionar(new AssinaturaOsModel
                    {
                        IdOs = id,
                        Tipo = TipoSignatario.Funcionario,
                        NomeSignatario = funcionario.Nome,
                        ImagemAssinatura = dto.ImagemAssinaturaFuncionario,
                        DataAssinatura = DateTime.UtcNow,
                    });
                }

                // Se pedido, essa assinatura também vira a assinatura padrão do funcionário.
                if (dto.SalvarComoPadrao)
                {
                    await _funcionarioService.AtualizarAssinaturaPadrao(idFuncionario, dto.ImagemAssinaturaFuncionario);
                }

                // Token opaco de 24h — 24 é fixo aqui por decisão explícita do produto;
                // se precisar virar configurável, isso vira um parâmetro do método.
                var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(24));
                ordemServico.TokenAssinaturaCliente = token;
                ordemServico.TokenAssinaturaExpiraEm = DateTime.UtcNow.AddHours(24);
                await _repository.Atualizar(ordemServico);

                await _unidadeTrabalho.ConfirmarTransacaoAsync();

                return new TokenAssinaturaDto
                {
                    Token = token,
                    ExpiraEm = ordemServico.TokenAssinaturaExpiraEm.Value
                };

            }
            catch (Exception e)
            {
                await _unidadeTrabalho.DesfazerTransacaoAsync();
                throw;
            }
            
        }

        // Dados públicos (sem login) pra tela que o cliente abre pelo link/QR code.
        public async Task<AssinaturaPublicaDto> BuscarAssinaturaPublica(string token)
        {
            var ordemServico = await _repository.BuscarPorToken(token);
            if (ordemServico == null)
                throw new EntidadeNaoEncontradaException("Link de assinatura inválido. Verifique com o consultor se a assinatura ja está salva.");

            if (ordemServico.TokenAssinaturaExpiraEm == null || ordemServico.TokenAssinaturaExpiraEm < DateTime.UtcNow)
                throw new ValidacaoException("Este link de assinatura expirou. Peça pro responsável gerar um novo.");

            var assinaturaFuncionario = await _assinaturaRepository.BuscarPorOsETipo(ordemServico.IdOs, TipoSignatario.Funcionario);
            var assinaturaCliente = await _assinaturaRepository.BuscarPorOsETipo(ordemServico.IdOs, TipoSignatario.Cliente);

            return new AssinaturaPublicaDto
            {
                IdOs = ordemServico.IdOs,
                TituloOs = ordemServico.TituloOs,
                NomeCliente = ordemServico.Cliente.NomeFantasia,
                DocumentoCliente = ordemServico.Cliente.Documento,
                DataHoraInicio = ordemServico.DataHoraInicio,
                DataHoraFim = ordemServico.DataHoraFim,
                Descricao = ordemServico.Descricao,
                RelatorioTecnico = ordemServico.RelatorioTecnico,
                NomeFuncionario = assinaturaFuncionario?.NomeSignatario ?? string.Empty,
                AssinaturaFuncionarioBase64 = assinaturaFuncionario?.ImagemAssinatura ?? string.Empty,
                JaAssinadoPeloCliente = assinaturaCliente != null,
                NomeTipoAtendimento = ordemServico.TipoAtendimento.Descricao
            };
        }

        // Cliente confirma a assinatura  salva a assinatura + o PDF final, e invalida o token.
        public async Task SubmeterAssinaturaCliente(string token, SubmeterAssinaturaClienteDto dto)
        {
            try
            {
                await _unidadeTrabalho.IniciarTransacaoAsync();
                var ordemServico = await _repository.BuscarPorToken(token);
                if (ordemServico == null)
                    throw new EntidadeNaoEncontradaException("Link de assinatura inválido.");

                if (ordemServico.TokenAssinaturaExpiraEm == null || ordemServico.TokenAssinaturaExpiraEm < DateTime.UtcNow)
                    throw new ValidacaoException("Este link de assinatura expirou. Peça pro responsável gerar um novo.");

                var assinaturaClienteExistente = await _assinaturaRepository.BuscarPorOsETipo(ordemServico.IdOs, TipoSignatario.Cliente);
                if (assinaturaClienteExistente != null)
                    throw new ConflitoException("Esta Ordem de Serviço já foi assinada pelo cliente.");

                var http = _httpContextAccessor.HttpContext;

                await _assinaturaRepository.Adicionar(new AssinaturaOsModel
                {
                    IdOs = ordemServico.IdOs,
                    Tipo = TipoSignatario.Cliente,
                    NomeSignatario = dto.NomeSignatario,
                    DocumentoSignatario = dto.DocumentoSignatario ?? string.Empty,
                    ImagemAssinatura = dto.ImagemAssinatura,
                    DataAssinatura = DateTime.UtcNow,
                });

                ordemServico.ArquivoPdf = dto.ArquivoPdf;
                ordemServico.TokenAssinaturaCliente = null;  // invalida o token: não dá pra reusar o link
                ordemServico.TokenAssinaturaExpiraEm = null;

                // Cliente assinou -> conclui a OS automaticamente, sem precisar do botão manual.
                ordemServico.Status = StatusOs.Concluida;

                await _repository.Atualizar(ordemServico);

                await _unidadeTrabalho.ConfirmarTransacaoAsync();

            }
            catch (Exception e)
            {
                await _unidadeTrabalho.DesfazerTransacaoAsync();
                throw;
            }
           
        }

        // Devolve o PDF
        public async Task<byte[]?> ObterPdf(int id)
        {
            var ordemServico = await BuscarOuFalhar(id);
            return ordemServico.ArquivoPdf;
        }


        private void FalharDataInvalidas(DateTime? prazo)
        {
            var agora = DateTime.Now;

            var prazoLocal = prazo?.ToLocalTime();
            //var inicioLocal = inicio?.ToLocalTime();

            // Compara data E hora exata
            if (prazoLocal < agora)
            {
                throw new ValidacaoException("O prazo não pode ser anterior à data e hora atual.");
            }

            //if (inicioLocal < agora)
            //{
            //    throw new ValidacaoException("A data de início não pode ser anterior à data e hora atual.");
            //}

            //if (inicioLocal > prazoLocal)
            //{
            //    throw new ValidacaoException("A data de início não pode ser maior que a data do prazo.");
            //}
        }

        public async Task<BuscarOrdemServicoDto?> BuscarPorTipoAtendimento(TipoAtendimento tipo)
        {
            var os = await _repository.BuscarPorTipoAtendimento(tipo);
            return OrdemServicoMapper.ParaDto(os);

        }




        //parte das fotos pdf
        // Gera o link/token pra registrar fotos 2H
        public async Task<TokenAssinaturaDto> IniciarFotos(int id)
        {
            var ordemServico = await BuscarOuFalhar(id);
            //falharSeOSConcluida(ordemServico);

            int idFuncionario = await _usuarioLogado.RetornarIdFuncionarioLogado();
            if (!await _osFuncionarioService.VerificarTecnicoEResponsavelAsync(ordemServico.IdOs, idFuncionario))
            {
                throw new ValidacaoException("Somente o funcionário responsável pode gerar o link de fotos.");
            }

            var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(24));
            ordemServico.TokenFotos = token;
            ordemServico.TokenFotosExpiraEm = DateTime.UtcNow.AddHours(2);
            await _repository.Atualizar(ordemServico);

            return new TokenAssinaturaDto { Token = token, ExpiraEm = ordemServico.TokenFotosExpiraEm.Value };
        }

        //tela de fotos.
        public async Task<FotosPublicaDto> BuscarFotosPublica(string token)
        {
            var ordemServico = await _repository.BuscarPorTokenFotos(token);
            if (ordemServico == null)
                throw new EntidadeNaoEncontradaException("Link de fotos inválido. Verifique se a OS ja tem as fotos incluidas ou gere outro link.");

            if (ordemServico.TokenFotosExpiraEm == null || ordemServico.TokenFotosExpiraEm < DateTime.UtcNow)
                throw new ValidacaoException("Este link de fotos expirou. Peça pro responsável gerar um novo.");

            return new FotosPublicaDto
            {
                IdOs = ordemServico.IdOs,
                TituloOs = ordemServico.TituloOs,
                NomeCliente = ordemServico.Cliente.NomeFantasia,
            };
        }

        // Recebe o PDF de fotos montado no front e salva na OS, invalidando o token.
        public async Task SalvarFotos(string token, SalvarFotosDto dto)
        {
            try
            {
                await _unidadeTrabalho.IniciarTransacaoAsync();
                var ordemServico = await _repository.BuscarPorTokenFotos(token);
                if (ordemServico == null)
                    throw new EntidadeNaoEncontradaException("Link de fotos inválido. Verifique se ja existe registro de fotos na OS ou gere outro link.");

                if (ordemServico.TokenFotosExpiraEm == null || ordemServico.TokenFotosExpiraEm < DateTime.UtcNow)
                    throw new ValidacaoException("Este link de fotos expirou. gere um novo.");

                ordemServico.ArquivoPdfFotos = dto.ArquivoPdfFotos;
                ordemServico.TokenFotos = null;
                ordemServico.TokenFotosExpiraEm = null;
                await _repository.Atualizar(ordemServico);
                await _unidadeTrabalho.ConfirmarTransacaoAsync();

            }
            catch(Exception e)
            {
                await _unidadeTrabalho.DesfazerTransacaoAsync();
                throw;
            }
            
        }

        public async Task<byte[]?> ObterPdfFotos(int id)
        {
            var ordemServico = await BuscarOuFalhar(id);
            return ordemServico.ArquivoPdfFotos;
        }
    }
}

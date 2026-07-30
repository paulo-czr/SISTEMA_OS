using OS_API.DTOs.Assinatura;
using OS_API.DTOs.OrdemServico;
using OS_API.Models;

namespace OS_API.Interfaces.Services
{
    public interface IOrdemServicoService
    {
        Task<BuscarOrdemServicoDto> Criar(CriarOrdemServicoDto dto);

        Task<BuscarOrdemServicoDto> Atualizar(int id, AtualizarOrdemServicoDto dto);

        Task<BuscarOrdemServicoDto?> BuscarPorId(int id);

        Task<List<BuscarOrdemServicoDto>> Listar();

        Task<BuscarOrdemServicoDto> AtualizarRelatorio(int id, AtualizarRelatorioDto dto);

        // Método separado só pra mudar o status da OS.
        Task<BuscarOrdemServicoDto> AlterarStatus(int id, AlterarStatusOsDto dto);

        Task Remover(int id);

        // funcionário responsável assina e gera o link/token de assinatura pro cliente.
        Task<TokenAssinaturaDto> IniciarAssinatura(int id, IniciarAssinaturaDto dto);

        // dados públicos (sem login) pra tela de assinatura que o cliente abre pelo link.
        Task<AssinaturaPublicaDto> BuscarAssinaturaPublica(string token);

        // cliente confirma a assinatura dele e o PDF final é salvo.
        Task SubmeterAssinaturaCliente(string token, SubmeterAssinaturaClienteDto dto);

        Task<byte[]?> ObterPdf(int id);

        Task<BuscarOrdemServicoDto?> BuscarPorTipoAtendimento(TipoAtendimento tipo);


        Task<TokenAssinaturaDto> IniciarFotos(int id);
        Task<FotosPublicaDto> BuscarFotosPublica(string token);
        Task SalvarFotos(string token, SalvarFotosDto dto);
        Task<byte[]?> ObterPdfFotos(int id);
    }
}

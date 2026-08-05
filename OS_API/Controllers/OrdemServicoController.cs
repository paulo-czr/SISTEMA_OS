using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using OS_API.DTOs.Assinatura;
using OS_API.DTOs.OrdemServico;
using OS_API.DTOs.OrdemServico.Filtro;
using OS_API.DTOs.OSFuncionario;
using OS_API.Exceptionn;
using OS_API.Helpers.Constantes;
using OS_API.Interfaces.Services;

namespace OS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("FrontEnd")]
    public class OrdemServicoController : ControllerBase
    {
        private readonly IOrdemServicoService _service;
        private readonly IOsFuncionarioService _osFuncionarioService;

        public OrdemServicoController(
            IOrdemServicoService service,
            IOsFuncionarioService osFuncionarioService)
        {
            _service = service;
            _osFuncionarioService = osFuncionarioService;
        }

        [HttpPost]
        [Authorize(Policy = Permissoes.OSCriar)]
        public async Task<IActionResult> Criar([FromBody] CriarOrdemServicoDto dto)
        {
            var ordemServicoCriada = await _service.Criar(dto);

            return CreatedAtAction(
                nameof(BuscarPorId),
                new { id = ordemServicoCriada.IdOs },
                ordemServicoCriada);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = Permissoes.OSAtualizar)]
        public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarOrdemServicoDto dto)
        {
            var ordemServicoAtualizada = await _service.Atualizar(id, dto);
            return Ok(ordemServicoAtualizada);
        }

        // Rota separada só pro funcionário preencher/editar o relatório dele.
        [HttpPatch("{id}/relatorio")]
        [Authorize(Policy = Permissoes.OSAtualizar)]
        public async Task<IActionResult> AtualizarRelatorio(int id, [FromBody] AtualizarRelatorioDto dto)
        {
            var ordemServico = await _service.AtualizarRelatorio(id, dto);
            return Ok(ordemServico);
        }

        // Rota separada só pra mudar o status da OS.
        [HttpPatch("{id}/status")]
        [Authorize(Policy = Permissoes.OSAtualizar)]
        public async Task<IActionResult> AlterarStatus(int id, [FromBody] AlterarStatusOsDto dto)
        {
            var ordemServico = await _service.AlterarStatus(id, dto);
            return Ok(ordemServico);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var ordemServico = await _service.BuscarPorId(id);
            return Ok(ordemServico);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Listar()
        {
            var ordensServico = await _service.Listar();
            return Ok(ordensServico);
        }
       
        [HttpGet("paginado")]
        [Authorize]
        public async Task<IActionResult> ListarPaginado([FromQuery] FiltroOrdemServicoDto filtro)
        {
            var resultado = await _service.ListarPaginado(filtro);
            return Ok(resultado);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = Permissoes.OSExcluir)]
        public async Task<IActionResult> Remover(int id)
        {
            await _service.Remover(id);
            return NoContent();
        }


        // Lista os funcionários vinculados a uma OS.
        [HttpGet("{idOs}/funcionarios")]
        [Authorize]
        public async Task<IActionResult> ListarFuncionarios(int idOs)
        {
            var funcionarios = await _osFuncionarioService.ObterTecnicosDaOsAsync(idOs);
            return Ok(funcionarios);
        }

        // Vincula um funcionário a uma OS.
        [HttpPost("{idOs}/funcionarios")]
        [Authorize(Policy = Permissoes.OSAtualizar)]
        public async Task<IActionResult> AdicionarFuncionario(int idOs, [FromBody] OsFuncionarioDto dto)
        {
            await _osFuncionarioService.AdicionarTecnicoAsync(idOs, dto.IdFuncionario, dto.Responsavel);
            return NoContent();
        }

        // Remove o vínculo de um funcionário com a OS.
        [HttpDelete("funcionarios/{idOsFuncionario}")]
        [Authorize(Policy = Permissoes.OSAtualizar)]
        public async Task<IActionResult> RemoverFuncionario(int idOsFuncionario)
        {
            await _osFuncionarioService.RemoverTecnicoAsync(idOsFuncionario);
            return NoContent();
        }

        // Define qual funcionário é o responsável pela OS.
        [HttpPut("{idOs}/funcionarios/{idFuncionario}/responsavel")]
        [Authorize(Policy = Permissoes.OSAtualizar)]
        public async Task<IActionResult> DefinirResponsavel(int idOs, int idFuncionario)
        {
            await _osFuncionarioService.DefinirResponsavelAsync(idOs, idFuncionario);
            return NoContent();
        }

        
        // Funcionário responsável assina e gera o link/token de assinatura pro cliente.
        [HttpPost("{id}/relatorio/iniciar-assinatura")]
        [Authorize(Policy = Permissoes.OSAtualizar)]
        public async Task<IActionResult> IniciarAssinatura(int id, [FromBody] IniciarAssinaturaDto dto)
        {
            var resultado = await _service.IniciarAssinatura(id, dto);
            return Ok(resultado);
        }

        // Página PÚBLICA de assinatura — o cliente abre isso pelo link/QR code, sem estar logado.
        [HttpGet("assinatura/{token}")]
        [AllowAnonymous]
        [EnableRateLimiting("publico")]
        // [EnableCors("PublicoToken")]
        public async Task<IActionResult> BuscarAssinaturaPublica(string token)
        {
            var dados = await _service.BuscarAssinaturaPublica(token);
            return Ok(dados);
        }

        // assinatura do cliente
        [HttpPost("assinatura/{token}")]
        [AllowAnonymous]
        [EnableRateLimiting("publico")]
        // [EnableCors("PublicoToken")]
        public async Task<IActionResult> SubmeterAssinaturaCliente(string token, [FromBody] SubmeterAssinaturaClienteDto dto)
        {
            await _service.SubmeterAssinaturaCliente(token, dto);
            return NoContent();
        }

        // GET /api/OrdemServico/5/pdf — baixa o PDF assinado (binário, não JSON)
        [HttpGet("{id}/pdf")]
        [Authorize]
        public async Task<IActionResult> ObterPdf(int id)
        {
            var bytes = await _service.ObterPdf(id);
            if (bytes == null || bytes.Length == 0)
                throw new EntidadeNaoEncontradaException("Esta OS ainda não tem um relatório assinado.");

            return File(bytes, "application/pdf", $"os-{id}.pdf");
        }




        //parte das fotos
        [HttpPost("{id}/fotos/iniciar")]
        [Authorize(Policy = Permissoes.OSAtualizar)]
        public async Task<IActionResult> IniciarFotos(int id)
        {
            var resultado = await _service.IniciarFotos(id);
            return Ok(resultado);
        }

        [HttpGet("fotos/{token}")]
        [AllowAnonymous]
        [EnableRateLimiting("publico")]
        //[EnableCors("PublicoToken")]
        public async Task<IActionResult> BuscarFotosPublica(string token)
        {
            var dados = await _service.BuscarFotosPublica(token);
            return Ok(dados);
        }

        [HttpPost("fotos/{token}")]
        [AllowAnonymous]
        [EnableRateLimiting("publico")]
        // [EnableCors("PublicoToken")]
        public async Task<IActionResult> SalvarFotos(string token, [FromBody] SalvarFotosDto dto)
        {
            await _service.SalvarFotos(token, dto);
            return NoContent();
        }

        [HttpGet("{id}/pdf-fotos")]
        [Authorize]
        public async Task<IActionResult> ObterPdfFotos(int id)
        {
            var bytes = await _service.ObterPdfFotos(id);
            if (bytes == null || bytes.Length == 0)
                throw new EntidadeNaoEncontradaException("Esta OS ainda não tem fotos registradas.");

            return File(bytes, "application/pdf", $"os-{id}-fotos.pdf");
        }
    }
}

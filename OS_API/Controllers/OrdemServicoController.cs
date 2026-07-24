using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OS_API.DTOs.OrdemServico;
using OS_API.DTOs.OSFuncionario;
using OS_API.Helpers.Constantes;
using OS_API.Interfaces.Services;

namespace OS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        // Ex.: PATCH /api/OrdemServico/5/relatorio
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
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var ordemServico = await _service.BuscarPorId(id);
            return Ok(ordemServico);
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var ordensServico = await _service.Listar();
            return Ok(ordensServico);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(int id)
        {
            await _service.Remover(id);
            return NoContent();
        }

        // ---- Funcionários vinculados à OS (OsFuncionario) ----
        // Fica aqui dentro do OrdemServicoController porque um vínculo
        // OS-Funcionário não existe sozinho, sempre pertence a uma OS.

        // Lista os funcionários vinculados a uma OS.
        // Ex.: GET /api/OrdemServico/5/funcionarios
        [HttpGet("{idOs}/funcionarios")]
        public async Task<IActionResult> ListarFuncionarios(int idOs)
        {
            var funcionarios = await _osFuncionarioService.ObterTecnicosDaOsAsync(idOs);
            return Ok(funcionarios);
        }

        // Vincula um funcionário a uma OS.
        // Ex.: POST /api/OrdemServico/5/funcionarios
        [HttpPost("{idOs}/funcionarios")]
        [Authorize(Policy = Permissoes.OSAtualizar)]
        public async Task<IActionResult> AdicionarFuncionario(int idOs, [FromBody] OsFuncionarioDto dto)
        {
            await _osFuncionarioService.AdicionarTecnicoAsync(idOs, dto.IdFuncionario, dto.Responsavel);
            return NoContent();
        }

        // Remove o vínculo de um funcionário com a OS.
        // Ex.: DELETE /api/OrdemServico/funcionarios/12
        [HttpDelete("funcionarios/{idOsFuncionario}")]
        [Authorize(Policy = Permissoes.OSAtualizar)]
        public async Task<IActionResult> RemoverFuncionario(int idOsFuncionario)
        {
            await _osFuncionarioService.RemoverTecnicoAsync(idOsFuncionario);
            return NoContent();
        }

        // Define qual funcionário é o responsável pela OS.
        // Ex.: PUT /api/OrdemServico/5/funcionarios/3/responsavel
        [HttpPut("{idOs}/funcionarios/{idFuncionario}/responsavel")]
        [Authorize(Policy = Permissoes.OSAtualizar)]
        public async Task<IActionResult> DefinirResponsavel(int idOs, int idFuncionario)
        {
            await _osFuncionarioService.DefinirResponsavelAsync(idOs, idFuncionario);
            return NoContent();
        }
    }
}

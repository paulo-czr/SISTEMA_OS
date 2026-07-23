using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OS_API.DTOs.OrdemServico;
using OS_API.Helpers.Constantes;
using OS_API.Interfaces.Services;

namespace OS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdemServicoController : ControllerBase
    {
        private readonly IOrdemServicoService _service;

        public OrdemServicoController(IOrdemServicoService service)
        {
            _service = service;
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
    }
}

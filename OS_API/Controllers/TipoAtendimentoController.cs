using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OS_API.DTOs.TipoAtendimento;
using OS_API.Helpers.Constantes;
using OS_API.Interfaces.Services;

namespace OS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipoAtendimentoController : ControllerBase
    {
        private readonly ITipoAtendimentoService _service;

        public TipoAtendimentoController(ITipoAtendimentoService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Policy = Permissoes.TipoAtendimentoCriar)]
        public async Task<IActionResult> Criar([FromBody] CriarTipoAtendimentoDto dto)
        {
            var tipoCriado = await _service.Criar(dto);

            return CreatedAtAction(
                nameof(BuscarPorId),
                new { id = tipoCriado.Id },
                tipoCriado);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = Permissoes.TipoAtendimentoAtualizar)]
        public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarTipoAtendimentoDto dto)
        {
            var tipoAtualizado = await _service.Atualizar(id, dto);
            return Ok(tipoAtualizado);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = Permissoes.TipoAtendimentoVisualizar)]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var tipo = await _service.BuscarPorId(id);
            return Ok(tipo);
        }

        [HttpGet]
        [Authorize(Policy = Permissoes.TipoAtendimentoVisualizar)]
        public async Task<IActionResult> Listar()
        {
            var tipos = await _service.Listar();
            return Ok(tipos);
        }

        [HttpDelete("{id}")]
        [Authorize (Policy = Permissoes.TipoAtendimentoExcluir)]
        public async Task<IActionResult> Remover(int id)
        {
            await _service.Remover(id);
            return NoContent();
        }
    }
}

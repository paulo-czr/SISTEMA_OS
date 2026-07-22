using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OS_API.DTOs.Tecnico;
using OS_API.Helpers.Constantes;
using OS_API.Interfaces.Repositories;
using OS_API.Interfaces.Services;

namespace OS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class FuncionarioController : ControllerBase
    {

        private readonly IFuncionarioRepository _repository;
        private readonly IFuncionarioService _service;
        public FuncionarioController(IFuncionarioRepository repository, IFuncionarioService service)
        {
            _repository = repository;
            _service = service;
        }


        [HttpPost]
        public async Task<IActionResult> Criar(CriarFuncionarioDto dto)
        {

            var funcionarioCriado = await _service.Criar(dto);

            return CreatedAtAction(
                nameof(GetPorId),
                new { id = funcionarioCriado.Id }, funcionarioCriado);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = Permissoes.FuncionarioVisualizar)]
        public async Task<IActionResult> GetPorId(int id)
        {
            var f = await _service.BuscarPorId(id);
            return Ok(f);
        }

        [HttpGet]
        [Authorize(Policy = Permissoes.FuncionarioVisualizar)]
        public async Task<IActionResult> Listar()
        {
            var funcionarios = await _service.Listar();
            return Ok(funcionarios);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = Permissoes.FuncionarioAtualizar)]
        public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarFuncionarioDto dto)
        {
            var funcionarioAtualizado = await _service.Atualizar(id, dto);
            return Ok(funcionarioAtualizado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(int id)
        {
            await _service.Remover(id);
            return NoContent();
        }
    }
}

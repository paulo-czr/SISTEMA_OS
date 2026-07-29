using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OS_API.DTOs.Cliente;
using OS_API.Helpers.Constantes;
using OS_API.Interfaces.Services;

namespace OS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClientesController( IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        /// Cadastro de cliente consumindo o ViaCEP para preencher os campos de endereço antes de salvar.
        [HttpPost]
        [Authorize(Policy = Permissoes.ClienteCriar)]
        public async Task<IActionResult> CriarCliente([FromBody] CriarClienteDto dto)
        {
            var clienteCriado = await _clienteService.Criar(dto);

            return CreatedAtAction(
                nameof(BuscarPorId),
                new { id = clienteCriado.IdCliente },
                clienteCriado);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = Permissoes.ClienteAtualizar)]
        public async Task<IActionResult> AtualizarCliente(int id, [FromBody] AtualizarClienteDto dto)
        {
            var clienteAtualizado = await _clienteService.Atualizar(id, dto);
            return Ok(clienteAtualizado);
        }

        [HttpGet("id/{id}")]
        [Authorize(Policy = Permissoes.ClienteVisualizar)]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var cliente = await _clienteService.BuscarPorId(id);
            return Ok(cliente);
        }

        [HttpGet("documento/{documento}")]
        [Authorize(Policy = Permissoes.ClienteVisualizar)]
        public async Task<IActionResult> BuscarPorDocumento(string documento)
        {
            var cliente = await _clienteService.BuscarPorDocumento(documento);
            return Ok(cliente);
        }

        [HttpGet]
        [Authorize(Policy = Permissoes.ClienteVisualizar)]
        public async Task<IActionResult> Listar()
        {
            var clientes = await _clienteService.Listar();
            return Ok(clientes);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = Permissoes.ClienteExcluir)]
        public async Task<IActionResult> Remover(int id)
        {
            await _clienteService.Remover(id);
            return NoContent();
        }
    }
}
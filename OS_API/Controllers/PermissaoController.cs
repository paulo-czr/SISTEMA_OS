using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OS_API.DTOs.Usuario;
using OS_API.Helpers.Constantes;
using OS_API.Interfaces.Repositories;
using OS_API.Interfaces.Services;
using OS_API.Mappings;

namespace OS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissaoController : ControllerBase
    {
        private readonly IUsuarioService _service;
        private readonly IPermissaoRepository _permissaoRepository;

        public PermissaoController(IUsuarioService service, IPermissaoRepository permissaoRepository)
        {
            _service = service;
            _permissaoRepository = permissaoRepository;
        }

        //[HttpGet("{id}")]
        //[Authorize(Policy = Permissoes.UsuarioVisualizar)]
        //public async Task<IActionResult> BuscarPorId(string id)
        //{
        //    var usuario = await _service.BuscarPorId(id);
        //    return Ok(usuario);
        //}

        //[HttpGet]
        //[Authorize(Policy = Permissoes.UsuarioVisualizar)]
        //public async Task<IActionResult> Listar()
        //{
        //    var usuarios = await _service.Listar();
        //    return Ok(usuarios);
        //}

        //[HttpPut("{id}")]
        //[Authorize(Policy = Permissoes.UsuarioAtualizar)]
        //public async Task<IActionResult> Atualizar(string id, [FromBody] AtualizarUsuarioDto dto)
        //{
        //    var usuarioAtualizado = await _service.Atualizar(id, dto);
        //    return Ok(usuarioAtualizado);
        //}

        //[HttpDelete("{id}")]
        //[Authorize(Policy = Permissoes.UsuarioRemover)]
        //public async Task<IActionResult> Remover(string id)
        //{
        //    await _service.Remover(id);
        //    return NoContent();
        //}

        // Rotas de gerenciamento de permissões do usuário 
        [HttpGet("{id}")]
        [Authorize(Policy = Permissoes.UsuarioVisualizar)]
        public async Task<IActionResult> ListarPermissoes(string id)
        {
            var permissoes = await _service.ListarPermissoes(id);
            return Ok(permissoes);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = Permissoes.UsuarioGerenciarPermissoes)]
        public async Task<IActionResult> AtualizarPermissoes(string id, [FromBody] AtualizarPermissoesUsuarioDto dto)
        {
            var permissoes = await _service.AtualizarPermissoes(id, dto.IdsPermissao);
            return Ok(permissoes);
        }



        //  catálogo completo de permissões cadastradas 
        [HttpGet]
        [Authorize(Policy = Permissoes.UsuarioVisualizar)]
        public async Task<IActionResult> ListarTodas()
        {
            var permissoes = await _permissaoRepository.Listar();
            return Ok(permissoes.Select(PermissaoMapper.ParaDto));
        }
       
    }
}

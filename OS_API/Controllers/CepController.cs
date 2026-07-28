using Microsoft.AspNetCore.Mvc;
using OS_API.Interfaces.Services;

namespace OS_API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CepController : ControllerBase
    {
        private readonly IViaCepService _viaCepService;

        public CepController(IViaCepService viaCepService)
        {
            _viaCepService = viaCepService;
        }

        [HttpGet("consulta-cep/{cep}")]
        public async Task<IActionResult> ConsultarCep(string cep)
        {
            var dadosCep = await _viaCepService.ObterEnderecoPorCepAsync(cep);

            if (dadosCep == null)
                return NotFound(new { mensagem = "CEP não encontrado ou inválido." });

            return Ok(new
            {
                cep = dadosCep.Cep,
                uf = dadosCep.Uf,
                cidade = dadosCep.Cidade,
                rua = dadosCep.Rua,
                bairro = dadosCep.Bairro,
                complemento = dadosCep.Complemento
            });
        }

    }
}

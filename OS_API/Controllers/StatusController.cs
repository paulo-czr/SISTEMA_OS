using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace OS_API.Controllers
{
    [ApiController]
    [Route("api")]
    public class StatusController : ControllerBase
    {
        private readonly HealthCheckService _healthCheckService;

        public StatusController(HealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        [HttpGet("status-banco")]
        public async Task<IActionResult> StatusBanco()
        {
            var report = await _healthCheckService.CheckHealthAsync();

            var resultado = new
            {
                status = report.Status.ToString(),
                checks = report.Entries.Select(e => new
                {
                    nome = e.Key,
                    status = e.Value.Status.ToString(),
                    descricao = e.Value.Description,
                    duracaoMs = e.Value.Duration.TotalMilliseconds
                })
            };

            return report.Status == HealthStatus.Healthy
                ? Ok(resultado)
                : StatusCode(503, resultado);
        }
    }
}
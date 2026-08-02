using OS_API.Models.Enum;

namespace OS_API.DTOs.OrdemServico.Filtro
{
    public class FiltroOrdemServicoDto
    {
        public int Pagina { get; set; } = 1;
        public int TamanhoPagina { get; set; } = 20;

        public StatusOs? Status { get; set; }
        public int? IdCliente { get; set; }
        public int? IdTipoAtendimento { get; set; }

        public DateTime? DataInicioDe { get; set; }
        public DateTime? DataInicioAte { get; set; }
        public DateTime? DataFimDe { get; set; }
        public DateTime? DataFimAte { get; set; }

 
        public string? Busca { get; set; }
    }
}

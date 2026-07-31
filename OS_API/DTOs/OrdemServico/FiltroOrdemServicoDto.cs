using OS_API.Models.Enum;

namespace OS_API.DTOs.OrdemServico
{
    // Recebido via query string em GET /OrdemServico (ex.: ?pagina=1&tamanhoPagina=20&status=2).
    // Todos os campos são opcionais — se não vier nada, lista normalmente (com paginação padrão).
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

        // Texto livre: filtra por título da OS ou nome do cliente.
        public string? Busca { get; set; }
    }
}

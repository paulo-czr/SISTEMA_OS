namespace OS_API.DTOs.OrdemServico
{
    // Resposta de GET /OrdemServico — traz só os itens da página pedida,
    // mais o total pra o front montar os botões de paginação.
    public class ResultadoPaginadoOrdemServicoDto
    {
        public List<BuscarOrdemServicoDto> Itens { get; set; } = new();
        public int TotalRegistros { get; set; }
        public int Pagina { get; set; }
        public int TamanhoPagina { get; set; }
    }
}

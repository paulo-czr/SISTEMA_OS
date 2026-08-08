namespace OS_API.DTOs.OrdemServico.Filtro
{
  
    public class ResultadoPaginadoOrdemServicoDto
    {
        public List<BuscarOrdemServicoDto> Itens { get; set; } = new();
        public int TotalRegistros { get; set; }
        public int Pagina { get; set; }
        public int TamanhoPagina { get; set; }
    }
}

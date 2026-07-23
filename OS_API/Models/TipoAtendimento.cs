namespace OS_API.Models
{
    public class TipoAtendimento
    {
        public int Id { get; set; }
        public string? Descricao { get; set; }
        public ICollection<OrdemServicoModel> OrdensServico { get; set; } = new List<OrdemServicoModel>();

    }
}

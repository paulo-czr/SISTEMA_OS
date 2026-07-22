namespace OS_API.Models
{
    public class AssinaturaOsModel
    {
        public int IdAssinatura { get; set; }

        public int IdOs { get; set; }

        public OrdemServicoModel OrdemServico { get; set; } = null!;

        public string NomeSignatario { get; set; } = string.Empty;

        public string DocumentoSignatario { get; set; } = string.Empty;

        public string ImagemAssinatura { get; set; } = string.Empty;

        public DateTime DataAssinatura { get; set; }

        public string Ip { get; set; } = string.Empty;

        public string UserAgente { get; set; } = string.Empty;

    }
}

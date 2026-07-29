using OS_API.Models.Enum;

namespace OS_API.Models
{
    public class AssinaturaOsModel
    {
        public int Id { get; set; }

        public int IdOs { get; set; }

        public OrdemServicoModel OrdemServico { get; set; } = null!;

        public string NomeSignatario { get; set; } = string.Empty;

        public string DocumentoSignatario { get; set; } = string.Empty;

        public string ImagemAssinatura { get; set; } = string.Empty;

        public DateTime DataAssinatura { get; set; }

        public TipoSignatario Tipo { get; set; }

    }
}

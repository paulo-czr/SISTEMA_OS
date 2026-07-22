using OS_API.Models.Cliente;
using OS_API.Models.Enum;

namespace OS_API.Models
{
    public class OrdemServicoModel
    {
        public int IdOs { get; set; }

        public string TituloOs { get; set; } = string.Empty;

        public int IdTipoAtendimento { get; set; }

        public TipoAtendimento TipoAtendimento { get; set; } = null!;

        public int IdCliente { get; set; }

        public ClienteModel Cliente { get; set; } = null!;

        public StatusOs Status { get; set; }

        public DateTime? DataHoraInicio { get; set; }

        public DateTime DataHoraFim { get; set; }

        public DateOnly? Prazo { get; set; }

        public string? RelatorioTecnico { get; set; }

        public string? Observacao { get; set; }

        public string Descricao { get; set; } = string.Empty;

        public byte[] ArquivoPdf { get; set; } = Array.Empty<byte>();

        public string CogigoPdf { get; set; } = string.Empty;

        public ICollection<AssinaturaOsModel?> Assinatura { get; set; } = new List<AssinaturaOsModel?>();

        public ICollection<OsFuncionarioModel> Tecnicos { get; set; } = new List<OsFuncionarioModel>();

        public OrdemServicoModel(
            string tituloOs,
            int idTipoAtendimento,
            int idCliente,
            StatusOs status,
            DateTime? dataHoraInicio,
            DateTime dataHoraFim,
            DateOnly? prazo,
            string descricao,
            string? observacao,
            string? observacao1)
        {
            TituloOs = tituloOs;
            IdTipoAtendimento = idTipoAtendimento;
            IdCliente = idCliente;
            Status = status;
            Status = StatusOs.Agendada;
            DataHoraInicio = dataHoraInicio;
            DataHoraFim = dataHoraFim;
            Prazo = prazo;
            Descricao = descricao;
            Observacao = observacao;
        }
    }
}

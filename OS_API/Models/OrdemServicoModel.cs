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

        public DateTime? DataHoraFim { get; set; }

        public DateTime? Prazo { get; set; }

        public string? RelatorioTecnico { get; set; }

        public string? Observacao { get; set; }

        public string Descricao { get; set; } = string.Empty;

        public byte[] ArquivoPdf { get; set; } = Array.Empty<byte>();

        public string CogigoPdf { get; set; } = string.Empty;

        public ICollection<AssinaturaOsModel?> Assinatura { get; set; } = new List<AssinaturaOsModel?>();

        public ICollection<OsFuncionarioModel> Funcionarios { get; set; } = new List<OsFuncionarioModel>();

        public string IdUsuarioQueRegistrou { get; set; }
        public UsuarioModel UsuarioQueRegistrou { get; set; }

        protected OrdemServicoModel(){}

        public OrdemServicoModel(
            string tituloOs,
            int idTipoAtendimento,
            int idCliente,
            DateTime? dataHoraInicio,
            DateTime? prazo,
            string descricao,
            string? observacao,
            string idUsuario)
        {
            TituloOs = tituloOs;
            IdTipoAtendimento = idTipoAtendimento;
            IdCliente = idCliente;
            Status = StatusOs.Agendada;
            DataHoraInicio = dataHoraInicio;
            Prazo = prazo;
            Descricao = descricao;
            Observacao = observacao;
            IdUsuarioQueRegistrou = idUsuario;
        }

       
    }
}

using OS_API.Models.Cliente;
using OS_API.Models.Enum;
using System.ComponentModel.DataAnnotations.Schema;

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

        public string? TokenAssinaturaCliente { get; set; }

        public DateTime? TokenAssinaturaExpiraEm { get; set; }

        public ICollection<AssinaturaOsModel> Assinaturas { get; set; }
    = new List<AssinaturaOsModel>();

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

        [NotMapped]
        public StatusOs StatusAtual
        {
            get
            {
                // OS já finalizada não muda de status
                if (Status == StatusOs.Concluida)
                    return StatusOs.Concluida;

                var agora = DateTime.Now;

                // Garante que todas as datas estejam no fuso horário local antes de comparar
                var prazo = Prazo?.ToLocalTime();
                var inicio = DataHoraInicio?.ToLocalTime();

                // Se estourou o prazo, está Atrasada
                if (prazo < agora)
                    return StatusOs.Atrasada;

                //  Se tem data de início, verifica se já começou ou se ainda vai começar
                if (inicio.HasValue)
                {
                    return inicio <= agora
                        ? StatusOs.EmAtendimento
                        : StatusOs.Agendada;
                }

                return Status;
            }
        }

    }
}

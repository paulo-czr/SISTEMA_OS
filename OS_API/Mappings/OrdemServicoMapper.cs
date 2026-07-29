using OS_API.DTOs.OrdemServico;
using OS_API.DTOs.OSFuncionario;
using OS_API.Models;
using System.Linq;

namespace OS_API.Mappings
{
    public class OrdemServicoMapper
    {
        public static OrdemServicoModel ParaModel(CriarOrdemServicoDto dto, string idUsuario)
        {
            return new OrdemServicoModel(
                dto.TituloOs,
                dto.IdTipoAtendimento,
                dto.IdCliente,
                dto.DataHoraInicio,
                dto.Prazo,
                dto.Descricao,
                dto.Observacao,
                idUsuario
            );
        }

        public static BuscarOrdemServicoDto ParaDto(OrdemServicoModel model)
        {
            return new BuscarOrdemServicoDto
            {
                IdOs = model.IdOs,
                TituloOs = model.TituloOs,
                Descricao = model.Descricao,
                IdTipoAtendimento = model.IdTipoAtendimento,
                NomeTipoAtendimento = model.TipoAtendimento.Descricao,
                IdCliente = model.IdCliente,
                NomeCliente = model.Cliente.NomeFantasia,
                Status = model.StatusAtual,
                DataHoraInicio = model.DataHoraInicio,
                DataHoraFim = model.DataHoraFim,
                Prazo = model.Prazo,
                RelatorioTecnico = model.RelatorioTecnico,
                Observacao = model.Observacao,
                CogigoPdf = model.CogigoPdf,
                IdUsuarioRegistrou = model.IdUsuarioQueRegistrou,
                Funcionarios = model.Funcionarios
                    .Select(f => new OsFuncionarioDto
                    {
                        IdFuncionario = f.IdFuncionario,
                        Responsavel = f.Responsavel
                    })
                    .ToList(),
                PossuiPdfAssinado = model.ArquivoPdf != null && model.ArquivoPdf.Length > 0,
                PossuiPdfFotos = model.ArquivoPdfFotos != null && model.ArquivoPdfFotos.Length > 0,
            };
        }

        public static void AtualizarModel(OrdemServicoModel model, AtualizarOrdemServicoDto dto)
        {
            model.TituloOs = dto.TituloOs;
            model.Descricao = dto.Descricao;
            model.IdTipoAtendimento = dto.IdTipoAtendimento;
            model.IdCliente = dto.IdCliente;
            //model.Status = dto.Status;
            model.DataHoraInicio = dto.DataHoraInicio;
            model.DataHoraFim = dto.DataHoraFim;
            model.Prazo = dto.Prazo;
            //model.RelatorioTecnico = dto.RelatorioTecnico;
            model.Observacao = dto.Observacao;
        }
    }
}

using OS_API.DTOs.OrdemServico;
using OS_API.Models;

namespace OS_API.Mappings
{
    public class OrdemServicoMapper
    {
        public static OrdemServicoModel ParaModel(CriarOrdemServicoDto dto)
        {
            return new OrdemServicoModel(
                dto.TituloOs,
                dto.IdTipoAtendimento,
                dto.IdCliente,
                dto.Status,
                dto.DataHoraInicio,
                dto.DataHoraFim,
                dto.Prazo,
                dto.Descricao,
                dto.RelatorioTecnico,
                dto.Observacao
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
                Status = model.Status,
                DataHoraInicio = (DateTime)model.DataHoraInicio,
                DataHoraFim = model.DataHoraFim,
                Prazo = (DateOnly)model.Prazo,
                RelatorioTecnico = model.RelatorioTecnico,
                Observacao = model.Observacao,
                CogigoPdf = model.CogigoPdf,
                Funcionarios = model.Tecnicos
                    .Select(t => t.IdFuncionario)
                    .ToList()
            };
        }

        //public static void AtualizarModel(OrdemServicoModel model, AtualizarOrdemServicoDto dto)
        //{
        //    model.TituloOs = dto.TituloOs;
        //    model.SolicitacaoCliente = dto.SolicitacaoCliente;
        //    model.IdTipoAtendimento = dto.IdTipoAtendimento;
        //    model.IdCliente = dto.IdCliente;
        //    model.Status = dto.Status;
        //    model.DataHoraInicio = dto.DataHoraInicio;
        //    model.DataHoraFim = dto.DataHoraFim;
        //    model.Prazo = dto.Prazo;
        //    model.RelatorioTecnico = dto.RelatorioTecnico;
        //    model.Observacao = dto.Observacao;
        //}
    }
}

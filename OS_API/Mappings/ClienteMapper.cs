using OS_API.DTOs.Cliente;
using OS_API.DTOs.ViaCepDto;
using OS_API.Models.Cliente;
using OS_API.Validation.Helpers;

namespace OS_API.Mappings
{
    public static class ClienteMapper
    {
        public static ClienteModel ParaModel(
            CriarClienteDto dto,
            // ViaCepDto dadosCep,
            string documentoNormalizado,
            string? emailNormalizado)
        {
            return new ClienteModel
            {
                // Se for PF, RazaoSocial vai nula para o banco. Se for PJ, remove os espaços.
                RazaoSocial = string.IsNullOrWhiteSpace(dto.RazaoSocial) ? null : dto.RazaoSocial.Trim(),

                NomeFantasia = dto.NomeFantasia.Trim(),

                TipoPessoa = dto.TipoPessoa,
                Documento = documentoNormalizado,
                Telefone = dto.Telefone,
                Email = emailNormalizado,

                Cep = SomenteDigitos.Extrair(dto.Cep),
                Uf = dto.Uf,
                Cidade = dto.Cidade,
                Bairro = dto.Bairro,
                Rua = dto.Rua,           
                Complemento = dto.Complemento,
                Numero = dto.Numero,
                Ativo = true
            };
        }

        public static ClienteDto ParaDto(ClienteModel model)
        {
            return new ClienteDto
            {
                IdCliente = model.IdCliente,
                RazaoSocial = model.RazaoSocial,
                NomeFantasia = model.NomeFantasia,
                TipoPessoa = model.TipoPessoa,
                Documento = model.Documento,
                Telefone = model.Telefone,
                Email = model.Email,
                Cep = model.Cep,
                Uf = model.Uf,
                Cidade = model.Cidade,
                Rua = model.Rua,
                Numero = model.Numero,
                Ativo = model.Ativo
            };
        }
    }
}
using OS_API.DTOs.Cliente;
using OS_API.DTOs.ViaCepDto;
using OS_API.Exceptionn;
using OS_API.Interfaces.Repositories;
using OS_API.Interfaces.Services;
using OS_API.Mappings;
using OS_API.Models;
using OS_API.Models.Cliente;
using OS_API.Models.Enum;
using OS_API.Validation.Helpers;

namespace OS_API.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _repository;

        public ClienteService(IClienteRepository repository)
        {
            _repository = repository;

        }

        // Creat
        public async Task<ClienteDto> Criar(CriarClienteDto dto)
        {
            // Documento é sempre normalizado (somente dígitos) antes de qualquer verificação ou persistência,
            // para que "123.456.789-09" e "12345678909" sejam sempre tratados como o mesmo valor.
            var documentoSoDigitos = NormalizarDocumento(dto.Documento);

            // Impede o cadastro de dois clientes com o mesmo CPF/CNPJ.
            await GarantirDocumentoDisponivel(documentoSoDigitos);

            var emailNormalizado = NormalizarEmail(dto.Email);

            var clienteModel = ClienteMapper.ParaModel(dto, documentoSoDigitos, emailNormalizado);

            clienteModel = await _repository.Adicionar(clienteModel);

            return ClienteMapper.ParaDto(clienteModel);
        }

        
        public async Task<ClienteDto?> BuscarPorId(int id)
        {
            var cliente = await BuscarClienteOuFalhar(id);

            return ClienteMapper.ParaDto(cliente);
        }

        public async Task<ClienteDto?> BuscarPorDocumento(string documento)
        {
            var documentoSoDigitos = NormalizarDocumento(documento);

            var cliente = await _repository.BuscarPorDocumento(documentoSoDigitos);

            if (cliente == null)
                throw new EntidadeNaoEncontradaException("Cliente não encontrado para o documento informado.");

            return ClienteMapper.ParaDto(cliente);
        }

        public async Task<List<ClienteDto>> Listar()
        {
            var clientes = await _repository.Listar();

            return clientes
                .Select(ClienteMapper.ParaDto)
                .ToList();
        }


        //Update
        public async Task<ClienteDto> Atualizar(int id, AtualizarClienteDto dto)
        {
            var cliente = await BuscarClienteOuFalhar(id);

            var documentoNormalizado = NormalizarDocumento(dto.Documento);

            // Só consulta o banco por unicidade se o documento realmente estiver mudando
            if (documentoNormalizado != cliente.Documento)
            {
                await GarantirDocumentoDisponivelParaOutroCliente(documentoNormalizado, id);
            }

            var emailNormalizado = NormalizarEmail(dto.Email);

            if (!string.IsNullOrWhiteSpace(emailNormalizado) && emailNormalizado != cliente.Email)
            {
                await GarantirEmailDisponivelParaOutroCliente(emailNormalizado, id);
            }

            var cepNormalizado = SomenteDigitos.Extrair(dto.Cep);

            cliente.AtualizarDados(
                    dto.TipoPessoa,
                    dto.NomeFantasia,
                    dto.RazaoSocial,
                    documentoNormalizado,
                    dto.Telefone,
                    emailNormalizado,
                    cepNormalizado,
                    dto.Rua,
                    dto.Cidade,
                    dto.Uf,
                    dto.Bairro,
                    dto.Complemento,
                    dto.Numero,
                    dto.Ativo);

            await _repository.Atualizar(cliente);

            return ClienteMapper.ParaDto(cliente);
        }

        //Delete
        public async Task Remover(int id)
        {
            var cliente = await BuscarClienteOuFalhar(id);

            await _repository.Remover(cliente);
        }


        // Métodos auxiliares

        private static string NormalizarDocumento(string documento)
        {
            return SomenteDigitos.Extrair(documento);
        }

        private static string? NormalizarEmail(string? email)
        {
            return string.IsNullOrWhiteSpace(email)
                ? null
                : email.Trim().ToLowerInvariant();
        }

        private async Task GarantirDocumentoDisponivel(string documentoNormalizado)
        {
            var documentoJaCadastrado = await _repository.ExisteDocumento(documentoNormalizado);

            if (documentoJaCadastrado)
                throw new ConflitoException("Já existe um cliente cadastrado com esse documento.");
        }

        private async Task GarantirDocumentoDisponivelParaOutroCliente(string documentoNormalizado, int idClienteAtual)
        {
            var documentoJaPertenceAOutroCliente =
                await _repository.ExisteDocumentoEmOutroCliente(documentoNormalizado, idClienteAtual);

            if (documentoJaPertenceAOutroCliente)
                throw new ConflitoException("Já existe outro cliente cadastrado com esse documento.");
        }

        private async Task GarantirEmailDisponivelParaOutroCliente(string? emailNormalizado, int idClienteAtual)
        {
            if (string.IsNullOrWhiteSpace(emailNormalizado))
                return;

            var emailJaPertenceAOutroCliente =
                await _repository.ExisteEmailEmOutroCliente(emailNormalizado, idClienteAtual);

            if (emailJaPertenceAOutroCliente)
                throw new ConflitoException("Já existe outro cliente cadastrado com esse e-mail.");
        }

        //private async Task<ViaCepDto> ObterEnderecoOuFalhar(string cep)
        //{
        //    var dadosCep = await _viaCepService.ObterEnderecoPorCepAsync(cep);

        //    if (dadosCep == null)
        //        throw new ValidacaoException("O CEP informado é inválido ou não foi encontrado.");

        //    return dadosCep;
        //}

        public async Task<ClienteModel> BuscarClienteOuFalhar(int id)
        {
            var cliente = await _repository.BuscarPorId(id);

            if (cliente == null)
                throw new EntidadeNaoEncontradaException("Cliente não encontrado.");

            return cliente;
        }
    }    
}
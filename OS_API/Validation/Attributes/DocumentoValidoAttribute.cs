using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using OS_API.Validation.Helpers;

namespace OS_API.Validation.Attributes
{
    /// <summary>
    /// Valida CPF ou CNPJ com base em uma propriedade de Enum informada.
    /// </summary>
    /// <typeparam name="TEnum"> O tipo do Enum que define se é Física ou Jurídica. </typeparam>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class DocumentoValidoAttribute<TEnum> : ValidationAttribute where TEnum : struct, Enum
    {
        private readonly string _nomePropriedadeTipoPessoa;

        // Cache estático para evitar o custo de Reflection a cada requisição
        private static readonly ConcurrentDictionary<(Type, string), PropertyInfo?> CachePropriedades = new();

        public DocumentoValidoAttribute(string nomePropriedadeTipoPessoa)
        {
            _nomePropriedadeTipoPessoa = nomePropriedadeTipoPessoa;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var documento = value as string;

            if (string.IsNullOrWhiteSpace(documento))
                return ValidationResult.Success;

            // Recupera o valor convertido com segurança para o tipo TEnum
            if (!TentarObterValorEnum(validationContext, out TEnum tipoPessoa))
            {
                return new ValidationResult(
                    $"Não foi possível validar o documento: a propriedade '{_nomePropriedadeTipoPessoa}' não possui um valor mapeado ou válido.",
                    new[] { validationContext.MemberName! });
            }

            // Compara o Enum convertendo para string ou int de forma genérica
            var nomeEnum = tipoPessoa.ToString();

            bool documentoValido;
            string mensagemErro;

            switch(nomeEnum)
            {
                case "Fisica":
                    documentoValido = CpfValidator.EhValido(documento);
                    mensagemErro = "O CPF informado é inválido.";
                    break;

                case "Juridica":
                    documentoValido = CnpjValidator.EhValido(documento);
                    mensagemErro = "O CNPJ informado é inválido.";
                    break;

                default:
                    documentoValido = false;
                    mensagemErro = "Tipo de pessoa desconhecido para validação de documento.";
                    break;
            }

            if (documentoValido)
                return ValidationResult.Success;

            return new ValidationResult(mensagemErro, new[] { validationContext.MemberName! });
        }

        private bool TentarObterValorEnum(ValidationContext validationContext, out TEnum resultado)
        {
            resultado = default;
            var objeto = validationContext.ObjectInstance;
            var tipoObjeto = objeto.GetType();

            // Busca do cache para alta performance
            var propriedade = CachePropriedades.GetOrAdd((tipoObjeto, _nomePropriedadeTipoPessoa),
                t => t.Item1.GetProperty(t.Item2));

            if (propriedade == null)
            {
                throw new InvalidOperationException(
                    $"A propriedade '{_nomePropriedadeTipoPessoa}' não foi encontrada na classe '{tipoObjeto.Name}'.");
            }

            var valorBruto = propriedade.GetValue(objeto);
            if (valorBruto == null) return false;

            // Tenta converter o valor bruto (que pode ser int, string ou outro Enum) para o TEnum esperado
            if (Enum.TryParse(valorBruto.ToString(), true, out TEnum convertido))
            {
                resultado = convertido;
                return true;
            }

            return false;
        }
    }
}
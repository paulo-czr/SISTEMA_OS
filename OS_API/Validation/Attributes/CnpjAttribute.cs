using OS_API.Validation.Helpers;
using System.ComponentModel.DataAnnotations;

namespace OS_API.Validation.Attributes
{
    /// <summary>
    /// Valida se o valor informado é um CNPJ válido (formato + dígitos verificadores).
    /// Assim como o CpfAttribute, campos nulos ou vazios são considerados válidos;
    /// a obrigatoriedade é responsabilidade do [Required].
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class CnpjAttribute : ValidationAttribute
    {
        public CnpjAttribute() : base("O CNPJ informado é inválido.")
        {
        }

        public override bool IsValid(object? value)
        {
            if (value is not string cnpj || string.IsNullOrWhiteSpace(cnpj))
                return true;

            return CnpjValidator.EhValido(cnpj);
        }
    }
}

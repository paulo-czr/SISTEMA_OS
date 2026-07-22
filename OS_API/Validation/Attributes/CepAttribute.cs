using OS_API.Validation.Helpers;
using System.ComponentModel.DataAnnotations;

namespace OS_API.Validation.Attributes
{
    /// <summary>
    /// Valida se o valor informado tem o formato de um CEP brasileiro (8 dígitos),
    /// aceitando tanto "01310100" quanto "01310-100". Rejeitar o formato aqui, antes
    /// de chegar à camada de serviço, evita chamadas desnecessárias à API do ViaCEP
    /// para valores que já são claramente inválidos.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class CepAttribute : ValidationAttribute
    {
        private const int TamanhoCep = 8;

        public CepAttribute() : base("O CEP informado é inválido.")
        {
        }

        public override bool IsValid(object? value)
        {
            // Validação de ausência de dados fica para o [Required]
            if (value is not string cep || string.IsNullOrWhiteSpace(cep))
                return true;

            return SomenteDigitos.Extrair(cep).Length == TamanhoCep;
        }
    }
}

namespace OS_API.Validation.Helpers
{
    /// <summary>
    /// Valida um CPF: confere os 11 dígitos e recalcula os dois
    /// dígitos verificadores para ver se batem com os informados.
    /// </summary>
    public static class CpfValidator
    {
        public static bool EhValido(string? cpf)
        {
            var numeros = SomenteDigitos.Extrair(cpf);

            // CPF sempre tem 11 dígitos
            if (numeros.Length != 11)
                return false;

            // Sequências como "111.111.111-11" ou "000.000.000-00" passariam
            // no cálculo do dígito verificador, mas não são CPFs reais.
            if (TodosOsDigitosSaoIguais(numeros))
                return false;

            var primeirosNoveDigitos = numeros.Substring(0, 9);
            var digito1 = CalcularDigitoVerificador(primeirosNoveDigitos);

            var primeirosDezDigitos = primeirosNoveDigitos + digito1;
            var digito2 = CalcularDigitoVerificador(primeirosDezDigitos);

            var digitosInformados = numeros.Substring(9, 2);
            var digitosCalculados = $"{digito1}{digito2}";

            return digitosInformados == digitosCalculados;
        }

        // Multiplica cada dígito por um peso decrescente (começando em "quantidade de dígitos + 1")
        // e soma tudo. Depois disso, o dígito verificador é o resto dessa soma dividido por 11.
        // Essa é a fórmula oficial usada pela Receita Federal para gerar os dois dígitos do CPF.
        private static int CalcularDigitoVerificador(string digitos)
        {
            var soma = 0;
            var peso = digitos.Length + 1;

            foreach (var caractere in digitos)
            {
                var digito = caractere - '0';
                soma += digito * peso;
                peso--;
            }

            var resto = soma % 11;
            return resto < 2 ? 0 : 11 - resto;
        }

        private static bool TodosOsDigitosSaoIguais(string numeros)
        {
            foreach (var caractere in numeros)
            {
                if (caractere != numeros[0])
                    return false;
            }

            return true;
        }
    }
}

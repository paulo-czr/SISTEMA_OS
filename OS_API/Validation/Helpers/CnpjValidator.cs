namespace OS_API.Validation.Helpers
{
    /// <summary>
    /// Valida um CNPJ, recalculando os dois dígitos verificadores.
    /// Segue a mesma ideia do CpfValidator, mudando apenas o tamanho do
    /// documento e os pesos usados no cálculo (regra oficial do CNPJ).
    /// </summary>
    public static class CnpjValidator
    {
        private static readonly int[] PesosPrimeiroDigito = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        private static readonly int[] PesosSegundoDigito = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        public static bool EhValido(string? cnpj)
        {
            var numeros = SomenteDigitos.Extrair(cnpj);

            // CNPJ sempre tem 14 dígitos
            if (numeros.Length != 14)
                return false;

            if (TodosOsDigitosSaoIguais(numeros))
                return false;

            var primeirosDozeDigitos = numeros.Substring(0, 12);
            var digito1 = CalcularDigitoVerificador(primeirosDozeDigitos, PesosPrimeiroDigito);

            var primeirosTrezeDigitos = primeirosDozeDigitos + digito1;
            var digito2 = CalcularDigitoVerificador(primeirosTrezeDigitos, PesosSegundoDigito);

            var digitosInformados = numeros.Substring(12, 2);
            var digitosCalculados = $"{digito1}{digito2}";

            return digitosInformados == digitosCalculados;
        }

        // Multiplica cada dígito pelo peso correspondente (tabela oficial do CNPJ)
        // e soma tudo. O dígito verificador é o resto dessa soma dividido por 11.
        private static int CalcularDigitoVerificador(string digitos, int[] pesos)
        {
            var soma = 0;

            for (var i = 0; i < digitos.Length; i++)
            {
                var digito = digitos[i] - '0';
                soma += digito * pesos[i];
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

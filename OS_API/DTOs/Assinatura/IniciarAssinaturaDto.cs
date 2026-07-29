namespace OS_API.DTOs.Assinatura
{
    // Enviado pelo funcionário responsável ao clicar em "Gerar Relatório".
    public class IniciarAssinaturaDto
    {
        public string ImagemAssinaturaFuncionario { get; set; } = string.Empty; // base64 (PNG)

        // Se true, essa assinatura também vira a assinatura padrão do funcionário
        public bool SalvarComoPadrao { get; set; }
    }
}

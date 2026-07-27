namespace OS_API.DTOs.Funcionario
{
    // Usado pra salvar/trocar a assinatura padrão do funcionário logado.
    public class AtualizarAssinaturaFuncionarioDto
    {
        public string ImagemAssinatura { get; set; } = string.Empty; // base64 (PNG)
    }
}

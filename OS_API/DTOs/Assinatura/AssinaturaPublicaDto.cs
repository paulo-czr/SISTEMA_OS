namespace OS_API.DTOs.Assinatura
{
    // Dados mínimos mostrados na página PÚBLICA de assinatura (sem login).
    // Só o necessário pro cliente conferir o que está assinando — nunca a
    // OS inteira (sem id interno, sem dados de outros clientes, etc.).
    public class AssinaturaPublicaDto
    {

        public int IdOs { get; set; }
        public string NomeTipoAtendimento { get; set; } = string.Empty;
        public string TituloOs { get; set; } = string.Empty;
        public string NomeCliente { get; set; } = string.Empty;
        public string DocumentoCliente { get; set; } = string.Empty;
        public DateTime? DataHoraInicio { get; set; }
        public DateTime? DataHoraFim { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public string? RelatorioTecnico { get; set; }
        public string NomeFuncionario { get; set; } = string.Empty;
        public string AssinaturaFuncionarioBase64 { get; set; } = string.Empty;
        public bool JaAssinadoPeloCliente { get; set; }
    }
}

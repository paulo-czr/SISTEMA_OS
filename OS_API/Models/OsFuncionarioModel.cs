namespace OS_API.Models
{
    public class OsFuncionarioModel
    {
        public int IdOsFuncionario { get; set; }

        // Chave estrangeira para OrdemServico
        public int IdOs { get; set; }
        public OrdemServicoModel OrdemServico { get; set; } = null!;

        // Chave estrangeira para Tecnico
        public int IdFuncionario { get; set; }
        public FuncionarioModel funcionario { get; set; } = null!;

        // Indica se este técnico é o responsável pela OS
        public bool Responsavel { get; set; }
    }
}
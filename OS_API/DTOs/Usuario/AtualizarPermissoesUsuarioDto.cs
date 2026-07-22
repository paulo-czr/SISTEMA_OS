using System.ComponentModel.DataAnnotations;

namespace OS_API.DTOs.Usuario
{
    /// <summary>
    /// Define de uma vez a lista completa de permissões do usuário.
    /// O back apaga todas as permissões atuais e insere só as informadas aqui
    /// (evita duplicidade e não exige uma rota separada para adicionar/remover uma por uma).
    /// </summary>
    public class AtualizarPermissoesUsuarioDto
    {
        [Required(ErrorMessage = "É obrigatório informar a lista de permissões (pode ser vazia).")]
        public List<int> IdsPermissao { get; set; } = new();
    }
}
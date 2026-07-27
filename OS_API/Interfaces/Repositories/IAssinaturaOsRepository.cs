using OS_API.Models;
using OS_API.Models.Enum;

namespace OS_API.Interfaces.Repositories
{
    public interface IAssinaturaOsRepository
    {
        Task<AssinaturaOsModel> Adicionar(AssinaturaOsModel assinatura);

        Task<AssinaturaOsModel?> BuscarPorOsETipo(int idOs, TipoSignatario tipo);

        Task Atualizar(AssinaturaOsModel assinatura);
    }
}

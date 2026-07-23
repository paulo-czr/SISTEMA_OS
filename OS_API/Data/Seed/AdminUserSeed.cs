using Microsoft.AspNetCore.Identity;
using OS_API.Interfaces.Repositories;
using OS_API.Models;
using OS_API.Models.Enum;

namespace OS_API.Data.Seed
{
    public static class AdminUserSeed
    {
        public const string UserName = "ADM";

        public static string GerarSenhaDoDia()
        {
            var hoje = DateTime.Now;
            var calculo = hoje.Day * hoje.Month;

            // Ex.: dia 5, mês 3 -> calculo = 15 -> senha "Adm015"
            return $"Adm{calculo:000}";
        }

        public static async Task SeedAdminAsync(
            UserManager<UsuarioModel> userManager,
            IUsuarioRepository usuarioRepository)
        {
            var senhaDoDia = GerarSenhaDoDia();
            var usuarioAdm = await userManager.FindByNameAsync(UserName);

            if (usuarioAdm == null)
            {
                usuarioAdm = new UsuarioModel
                {
                    UserName = UserName,
                    Email = "adm@sistema.local",
                    EmailConfirmed = true,
                    Ativo = true
                };

                var resultado = await userManager.CreateAsync(usuarioAdm, senhaDoDia);
                if (!resultado.Succeeded)
                    throw new Exception(string.Join(", ", resultado.Errors.Select(e => e.Description)));

                await usuarioRepository.AdicionarPermissaoPorTipoUser(usuarioAdm, TipoUsuario.Administrador);
            }
            else
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(usuarioAdm);
                var resultado = await userManager.ResetPasswordAsync(usuarioAdm, token, senhaDoDia);
                if (!resultado.Succeeded)
                    throw new Exception(string.Join(", ", resultado.Errors.Select(e => e.Description)));
            }
        }
    }
}
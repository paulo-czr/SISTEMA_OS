using OS_API.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_API.Helpers.Constantes
{
    public static class Roles
    {
        public const string Administrador = nameof(TipoUsuario.Administrador);
        public const string Atendente = nameof(TipoUsuario.Gestor);
        public const string Tecnico = nameof(TipoUsuario.Tecnico);
    }
}

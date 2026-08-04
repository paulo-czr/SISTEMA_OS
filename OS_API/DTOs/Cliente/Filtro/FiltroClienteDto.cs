using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_API.DTOs.Cliente.Filtro
{
    public class FiltroClienteDto
    {
        public int Pagina { get; set; } = 1;
        public int TamanhoPagina { get; set; } = 20;
        public string? Busca { get; set; }
    }
}

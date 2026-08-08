using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_API.DTOs.Cliente.Filtro
{
    public class ResultadoPaginadoClienteDto
    {
        public List<ClienteDto> Itens { get; set; } = new();
        public int TotalRegistros { get; set; }
        public int Pagina { get; set; }
        public int TamanhoPagina { get; set; }
    }
}

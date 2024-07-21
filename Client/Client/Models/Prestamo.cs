using Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    public class Prestamo
    {
        public int IdCliente { get; set; }
        public int IdPelicula { get; set; }
        public int IdSucursal { get; set; }
        public string Accion { get; set; }
    }
}
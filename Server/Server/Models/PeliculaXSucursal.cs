using System.Collections.Generic;

namespace Server.Models
{
    public class PeliculaXSucursal
    {
        public Sucursal IdSucursal { get; set; }
        public List<Pelicula> Peliculas { get; set; }
        public int Cantidad { get; set; }
    }
}

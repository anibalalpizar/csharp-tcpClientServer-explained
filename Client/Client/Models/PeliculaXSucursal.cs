using System.Collections.Generic;

namespace Client.Models
{
    public class PeliculaXSucursal
    {
        public Sucursal IdSucursal { get; set; }
        public List<Pelicula> Peliculas { get; set; }
        public int Cantidad { get; set; }
        public string Accion { get; set; }
    }
}

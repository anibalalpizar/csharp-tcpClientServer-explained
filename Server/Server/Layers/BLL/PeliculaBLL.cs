using Server.Layers.DAL;
using Server.Models;
using System;
using System.Collections.Generic;

namespace Server.Layers.BLL
{
    public class PeliculaBLL
    {
        private PeliculaDAL peliculaDAL = new PeliculaDAL();

        public string RegistrarPelicula(Pelicula pelicula)
        {
            return peliculaDAL.InsertarPelicula(pelicula);
        }

        public List<Pelicula> ObteniendoPelicula()
        {
            return peliculaDAL.ObtenerTodasPeliculas();
        }
    }
}

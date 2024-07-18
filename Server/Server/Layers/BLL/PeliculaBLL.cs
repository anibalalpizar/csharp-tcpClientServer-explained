using Server.Layers.DAL; // Importa la capa de acceso a datos (DAL)
using Server.Models; // Importa los modelos de datos
using System; // Importa funcionalidades básicas del sistema
using System.Collections.Generic; // Importa funcionalidades para trabajar con colecciones genéricas

namespace Server.Layers.BLL // Define el espacio de nombres 'Server.Layers.BLL'
{
    // Define la clase 'PeliculaBLL' que maneja la lógica de negocio relacionada con películas
    public class PeliculaBLL
    {
        private PeliculaDAL peliculaDAL = new PeliculaDAL(); // Instancia de 'PeliculaDAL' para manejar el acceso a datos de películas

        // Método para registrar una película
        public string RegistrarPelicula(Pelicula pelicula)
        {
            return peliculaDAL.InsertarPelicula(pelicula); // Llama al método para insertar la película en la base de datos y retorna el resultado
        }

        // Método para obtener todas las películas
        public List<Pelicula> ObteniendoPelicula()
        {
            return peliculaDAL.ObtenerTodasPeliculas(); // Llama al método para obtener todas las películas de la base de datos y retorna la lista de películas
        }
    }
}

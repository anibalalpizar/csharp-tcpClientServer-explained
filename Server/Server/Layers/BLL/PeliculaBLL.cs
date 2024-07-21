using Server.Layers.DAL; // Importa la capa de acceso a datos (DAL)
using Server.Models; // Importa los modelos de datos
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

        // Método para prestar una película
        public string PrestarPelicula(Prestamo prestamo)
        {
            return peliculaDAL.PrestarPelicula(prestamo); // Llama al método para prestar la película en la base de datos y retorna el resultado
        }

        // Metodo para obtener las peliculas de un cliente
        public List<object> ObtenerMisPeliculas(int idCliente)
        {
            return peliculaDAL.MisPeliculas(idCliente); // Llama al método para obtener las películas de un cliente de la base de datos y retorna la lista de películas
        }
    }
}

// Importamos las clases necesarias desde el espacio de nombres Server.Layers.DAL
using Server.Layers.DAL;
// Importamos los modelos necesarios desde el espacio de nombres Server.Models
using Server.Models;
// Importamos la biblioteca estándar de C# para manejar colecciones genéricas
using System.Collections.Generic;

// Definimos un espacio de nombres específico para la capa de lógica de negocio (Business Logic Layer - BLL)
namespace Server.Layers.BLL
{
    // Definimos la clase 'CategoriaPeliculaBLL' que representa la lógica de negocio para las categorías de películas
    public class CategoriaPeliculaBLL
    {
        // Declaramos una variable privada '_categoriaDAL' para manejar las operaciones de acceso a datos
        private CategoriaPeliculaDAL _categoriaDAL;

        // Constructor de la clase 'CategoriaPeliculaBLL'
        public CategoriaPeliculaBLL()
        {
            // Instanciamos la clase 'CategoriaPeliculaDAL' y asignamos a '_categoriaDAL'
            _categoriaDAL = new CategoriaPeliculaDAL();
        }

        // Método para registrar una nueva categoría de película
        public string RegistrarCategoria(CategoriaPelicula request)
        {
            // Llamamos al método 'InsertarCategoria' de la capa de acceso a datos (DAL) y devolvemos el resultado
            return _categoriaDAL.InsertarCategoria(request);
        }

        // Método para obtener todas las categorías de películas
        public List<CategoriaPelicula> ObteniendoCategoria()
        {
            // Llamamos al método 'ObtenerTodasCategorias' de la capa de acceso a datos (DAL) y devolvemos la lista de categorías
            return _categoriaDAL.ObtenerTodasCategorias();
        }
    }
}

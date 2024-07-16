// Importamos los modelos necesarios desde el espacio de nombres Server.Models
using Server.Models;
using Server.Utils;

// Importamos las utilidades necesarias desde el espacio de nombres Server.Utils
using System.Collections.Generic;
// Importamos la biblioteca para manejar conexiones a bases de datos SQL
using System.Data.SqlClient;

// Definimos un espacio de nombres específico para la capa de acceso a datos (Data Access Layer - DAL)
namespace Server.Layers.DAL
{
    // Definimos la clase 'CategoriaPeliculaDAL' que contiene métodos para interactuar con la base de datos
    public class CategoriaPeliculaDAL
    {
        // Cadena de conexión para conectarse a la base de datos, obtenida a través de una utilidad
        string connectionString = DatabaseUtils.GetConnection().ConnectionString;

        // Método para insertar una nueva categoría en la base de datos
        public string InsertarCategoria(CategoriaPelicula categoria)
        {
            // Usamos una conexión a la base de datos SQL con la cadena de conexión proporcionada
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Consulta SQL para verificar si el IdCategoria ya existe en la base de datos
                string checkQuery = "SELECT COUNT(*) FROM CategoriaPelicula WHERE IdCategoria = @IdCategoria";
                // Preparamos el comando SQL con la consulta
                using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                {
                    // Añadimos el parámetro @IdCategoria con el valor del IdCategoria de la categoría proporcionada
                    checkCommand.Parameters.AddWithValue("@IdCategoria", categoria.IdCategoria);
                    // Abrimos la conexión a la base de datos
                    connection.Open();
                    // Ejecutamos la consulta y obtenemos el resultado (el número de filas que coinciden)
                    int count = (int)checkCommand.ExecuteScalar();

                    // Si el IdCategoria ya existe, devolvemos un mensaje de error
                    if (count > 0)
                    {
                        return "Error: El IdCategoria ya existe en la base de datos.";
                    }
                }

                // Consulta SQL para insertar una nueva categoría en la base de datos
                string insertQuery = "INSERT INTO CategoriaPelicula (IdCategoria, NombreCategoria, Descripcion) VALUES (@IdCategoria, @NombreCategoria, @Descripcion)";
                // Preparamos el comando SQL con la consulta de inserción
                using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                {
                    // Añadimos los parámetros con los valores correspondientes de la categoría proporcionada
                    insertCommand.Parameters.AddWithValue("@IdCategoria", categoria.IdCategoria);
                    insertCommand.Parameters.AddWithValue("@NombreCategoria", categoria.NombreCategoria);
                    insertCommand.Parameters.AddWithValue("@Descripcion", categoria.Descripcion);

                    // Ejecutamos la consulta de inserción
                    insertCommand.ExecuteNonQuery();
                }
            }

            // Si todo sale bien, devolvemos un mensaje de éxito
            return "Éxito: Categoría insertada correctamente.";
        }

        // Método para obtener todas las categorías de la base de datos
        public List<CategoriaPelicula> ObtenerTodasCategorias()
        {
            // Creamos una lista para almacenar las categorías obtenidas
            List<CategoriaPelicula> categorias = new List<CategoriaPelicula>();

            // Usamos una conexión a la base de datos SQL con la cadena de conexión proporcionada
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Consulta SQL para obtener todas las categorías de la base de datos
                string query = "SELECT IdCategoria, NombreCategoria, Descripcion FROM CategoriaPelicula";
                // Preparamos el comando SQL con la consulta
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Abrimos la conexión a la base de datos
                    connection.Open();
                    // Ejecutamos la consulta y obtenemos un lector de datos (SqlDataReader)
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Leemos los resultados fila por fila
                        while (reader.Read())
                        {
                            // Creamos una nueva instancia de 'CategoriaPelicula' y la llenamos con los datos obtenidos
                            CategoriaPelicula categoria = new CategoriaPelicula
                            {
                                IdCategoria = reader.GetInt32(0), // Obtenemos el IdCategoria (primera columna)
                                NombreCategoria = reader.GetString(1), // Obtenemos el NombreCategoria (segunda columna)
                                Descripcion = reader.GetString(2) // Obtenemos la Descripcion (tercera columna)
                            };

                            // Añadimos la categoría a la lista
                            categorias.Add(categoria);
                        }
                    }
                }
            }

            // Devolvemos la lista de categorías obtenidas
            return categorias;
        }
    }
}

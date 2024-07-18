using Server.Models; // Importa los modelos de datos
using Server.Utils; // Importa utilidades del servidor, como funciones de base de datos
using System.Collections.Generic; // Importa funcionalidades para trabajar con colecciones genéricas
using System.Data.SqlClient; // Importa funcionalidades para trabajar con SQL Server

namespace Server.Layers.DAL // Define el espacio de nombres 'Server.Layers.DAL'
{
    // Define la clase 'PeliculaDAL' que maneja el acceso a datos de películas
    public class PeliculaDAL
    {
        string connectionString = DatabaseUtils.GetConnection().ConnectionString; // Cadena de conexión a la base de datos

        // Método para insertar una película en la base de datos
        public string InsertarPelicula(Pelicula pelicula)
        {
            using (SqlConnection connection = new SqlConnection(connectionString)) // Abre una conexión a la base de datos
            {
                string checkQuery = "SELECT COUNT(*) FROM Pelicula WHERE IdPelicula = @IdPelicula"; // Consulta para verificar si ya existe la película
                using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@IdPelicula", pelicula.IdPelicula); // Agrega el parámetro de 'IdPelicula' a la consulta
                    connection.Open(); // Abre la conexión
                    int count = (int)checkCommand.ExecuteScalar(); // Ejecuta la consulta y obtiene el conteo

                    if (count > 0) // Si el conteo es mayor a 0, la película ya existe
                    {
                        return "Error: El IdPelicula ya existe en la base de datos."; // Retorna un mensaje de error
                    }
                }

                string insertQuery = "INSERT INTO Pelicula (IdPelicula, idCategoria, Titulo, AnioLanzamiento, Idioma) VALUES (@IdPelicula, @idCategoria, @Titulo, @AnioLanzamiento, @Idioma)"; // Consulta para insertar una nueva película

                using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                {
                    insertCommand.Parameters.AddWithValue("@IdPelicula", pelicula.IdPelicula); // Agrega el parámetro de 'IdPelicula' a la consulta
                    insertCommand.Parameters.AddWithValue("@idCategoria", pelicula.CategoriaPelicula.IdCategoria); // Agrega el parámetro de 'idCategoria' a la consulta
                    insertCommand.Parameters.AddWithValue("@Titulo", pelicula.Titulo); // Agrega el parámetro de 'Titulo' a la consulta
                    insertCommand.Parameters.AddWithValue("@AnioLanzamiento", pelicula.AnoLanzamiento); // Agrega el parámetro de 'AnioLanzamiento' a la consulta
                    insertCommand.Parameters.AddWithValue("@Idioma", pelicula.Idioma); // Agrega el parámetro de 'Idioma' a la consulta

                    insertCommand.ExecuteNonQuery(); // Ejecuta la consulta de inserción
                }
            }

            return "Éxito: Película insertada correctamente."; // Retorna un mensaje de éxito
        }

        // Método para obtener todas las películas de la base de datos
        public List<Pelicula> ObtenerTodasPeliculas()
        {
            List<Pelicula> peliculas = new List<Pelicula>(); // Lista para almacenar las películas obtenidas

            using (SqlConnection connection = new SqlConnection(connectionString)) // Abre una conexión a la base de datos
            {
                string query = "SELECT p.IdPelicula, p.idCategoria, p.Titulo, p.AnioLanzamiento, p.Idioma, c.IdCategoria, c.NombreCategoria, c.Descripcion FROM Pelicula p JOIN CategoriaPelicula c ON p.idCategoria = c.IdCategoria"; // Consulta para obtener todas las películas junto con sus categorías

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open(); // Abre la conexión
                    SqlDataReader reader = command.ExecuteReader(); // Ejecuta la consulta y obtiene un lector de datos

                    while (reader.Read()) // Lee los datos de la consulta
                    {
                        Pelicula pelicula = new Pelicula
                        {
                            IdPelicula = reader.GetInt32(reader.GetOrdinal("IdPelicula")), // Obtiene el Id de la película
                            Titulo = reader.GetString(reader.GetOrdinal("Titulo")), // Obtiene el título de la película
                            AnoLanzamiento = reader.GetInt32(reader.GetOrdinal("AnioLanzamiento")), // Obtiene el año de lanzamiento de la película
                            Idioma = reader.GetString(reader.GetOrdinal("Idioma")), // Obtiene el idioma de la película
                            CategoriaPelicula = new CategoriaPelicula
                            {
                                IdCategoria = reader.GetInt32(reader.GetOrdinal("idCategoria")), // Obtiene el Id de la categoría
                                NombreCategoria = reader.GetString(reader.GetOrdinal("NombreCategoria")), // Obtiene el nombre de la categoría
                                Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")) // Obtiene la descripción de la categoría
                            }
                        };

                        peliculas.Add(pelicula); // Agrega la película a la lista
                    }
                }
            }
            return peliculas; // Retorna la lista de películas
        }
    }
}
using Server.Models;
using Server.Utils;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Server.Layers.DAL
{
    public class PeliculaDAL
    {
        string connectionString = DatabaseUtils.GetConnection().ConnectionString;

        public string InsertarPelicula(Pelicula pelicula)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string checkQuery = "SELECT COUNT(*) FROM Pelicula WHERE IdPelicula = @IdPelicula";
                using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@IdPelicula", pelicula.IdPelicula);
                    connection.Open();
                    int count = (int)checkCommand.ExecuteScalar();

                    if (count > 0)
                    {
                        return "Error: El IdPelicula ya existe en la base de datos.";
                    }
                }

                string insertQuery = "INSERT INTO Pelicula (IdPelicula, idCategoria, Titulo, AnioLanzamiento, Idioma) VALUES (@IdPelicula, @idCategoria, @Titulo, @AnioLanzamiento, @Idioma)";

                using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                {
                    insertCommand.Parameters.AddWithValue("@IdPelicula", pelicula.IdPelicula);
                    insertCommand.Parameters.AddWithValue("@idCategoria", pelicula.CategoriaPelicula.IdCategoria);
                    insertCommand.Parameters.AddWithValue("@Titulo", pelicula.Titulo);
                    insertCommand.Parameters.AddWithValue("@AnioLanzamiento", pelicula.AnoLanzamiento);
                    insertCommand.Parameters.AddWithValue("@Idioma", pelicula.Idioma);

                    insertCommand.ExecuteNonQuery();
                }
            }

            return "Éxito: Película insertada correctamente.";
        }

        public List<Pelicula> ObtenerTodasPeliculas()
        {
            List<Pelicula> peliculas = new List<Pelicula>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT p.IdPelicula, p.idCategoria, p.Titulo, p.AnioLanzamiento, p.Idioma, c.IdCategoria, c.NombreCategoria, c.Descripcion FROM Pelicula p JOIN CategoriaPelicula c ON p.idCategoria = c.IdCategoria";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Pelicula pelicula = new Pelicula
                        {
                            IdPelicula = reader.GetInt32(reader.GetOrdinal("IdPelicula")),
                            Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                            AnoLanzamiento = reader.GetInt32(reader.GetOrdinal("AnioLanzamiento")),
                            Idioma = reader.GetString(reader.GetOrdinal("Idioma")),
                            CategoriaPelicula = new CategoriaPelicula
                            {
                                IdCategoria = reader.GetInt32(reader.GetOrdinal("idCategoria")),
                                NombreCategoria = reader.GetString(reader.GetOrdinal("NombreCategoria")),
                                Descripcion = reader.GetString(reader.GetOrdinal("Descripcion"))
                            }
                        };

                        peliculas.Add(pelicula);
                    }
                }
            }
            return peliculas;
        }
    }
}



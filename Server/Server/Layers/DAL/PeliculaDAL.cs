using Server.Models; // Importa los modelos de datos
using Server.Utils; // Importa utilidades del servidor, como funciones de base de datos
using System;
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

        // Método para prestar una película
        public string PrestarPelicula(Prestamo prestamo)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open(); // Abre la conexión

                // Verifica si el cliente ya tiene un préstamo pendiente de la misma película
                string checkPrestamoQuery = @"
                    SELECT COUNT(*) 
                    FROM Prestamo 
                    WHERE IdCliente = @IdCliente AND IdPelicula = @IdPelicula AND PendienteDevolucion = 1";

                using (SqlCommand checkPrestamoCommand = new SqlCommand(checkPrestamoQuery, connection))
                {
                    checkPrestamoCommand.Parameters.AddWithValue("@IdCliente", prestamo.IdCliente);
                    checkPrestamoCommand.Parameters.AddWithValue("@IdPelicula", prestamo.IdPelicula);

                    int count = (int)checkPrestamoCommand.ExecuteScalar();

                    if (count > 0) // Si el conteo es mayor a 0, ya existe un préstamo pendiente
                    {
                        return "Error: El cliente ya tiene un préstamo pendiente de esta película.";
                    }
                }

                // Verifica si la película tiene suficiente cantidad en la sucursal seleccionada
                string checkCantidadQuery = @"
                    SELECT Cantidad 
                    FROM PeliculaXSucursal 
                    WHERE IdPelicula = @IdPelicula AND IdSucursal = @IdSucursal";

                using (SqlCommand checkCantidadCommand = new SqlCommand(checkCantidadQuery, connection))
                {
                    checkCantidadCommand.Parameters.AddWithValue("@IdPelicula", prestamo.IdPelicula);
                    checkCantidadCommand.Parameters.AddWithValue("@IdSucursal", prestamo.IdSucursal);

                    object result = checkCantidadCommand.ExecuteScalar();
                    int cantidad = result != DBNull.Value ? Convert.ToInt32(result) : 0;

                    if (cantidad <= 0) // Si la cantidad es 0 o menor
                    {
                        return "Error: No hay suficiente cantidad disponible en la sucursal seleccionada.";
                    }
                }

                // Inicia una transacción para asegurar la atomicidad de las operaciones
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // Inserta el préstamo en la tabla Prestamo
                    string insertPrestamoQuery = @"
                        INSERT INTO Prestamo (IdCliente, IdSucursal, IdPelicula, FechaPrestamo, PendienteDevolucion) 
                        VALUES (@IdCliente, @IdSucursal, @IdPelicula, @FechaPrestamo, @PendienteDevolucion)";

                    using (SqlCommand insertPrestamoCommand = new SqlCommand(insertPrestamoQuery, connection, transaction))
                    {
                        insertPrestamoCommand.Parameters.AddWithValue("@IdCliente", prestamo.IdCliente);
                        insertPrestamoCommand.Parameters.AddWithValue("@IdSucursal", prestamo.IdSucursal);
                        insertPrestamoCommand.Parameters.AddWithValue("@IdPelicula", prestamo.IdPelicula);
                        insertPrestamoCommand.Parameters.AddWithValue("@FechaPrestamo", DateTime.Now);
                        insertPrestamoCommand.Parameters.AddWithValue("@PendienteDevolucion", false);

                        insertPrestamoCommand.ExecuteNonQuery();
                    }

                    // Actualiza la cantidad de la película en la tabla PeliculaXSucursal
                    string updateCantidadQuery = @"
                        UPDATE PeliculaXSucursal 
                        SET Cantidad = Cantidad - 1 
                        WHERE IdPelicula = @IdPelicula AND IdSucursal = @IdSucursal";

                    using (SqlCommand updateCantidadCommand = new SqlCommand(updateCantidadQuery, connection, transaction))
                    {
                        updateCantidadCommand.Parameters.AddWithValue("@IdPelicula", prestamo.IdPelicula);
                        updateCantidadCommand.Parameters.AddWithValue("@IdSucursal", prestamo.IdSucursal);

                        updateCantidadCommand.ExecuteNonQuery();
                    }

                    // Confirma la transacción
                    transaction.Commit();

                    return $"Éxito: Préstamo registrado correctamente. A la película con ID {prestamo.IdPelicula} se le restó 1 de la cantidad.";
                }
                catch (Exception ex)
                {
                    // Si ocurre un error, revierte la transacción
                    transaction.Rollback();
                    return $"Error: No se pudo realizar el préstamo. Detalle del error: {ex.Message}";
                }
            }
        }

        // Método para obtener las películas de un cliente
        public List<object> MisPeliculas(int idCliente)
        {
            List<object> misPeliculas = new List<object>(); // Lista para almacenar las películas obtenidas

            using (SqlConnection connection = new SqlConnection(connectionString)) // Abre una conexión a la base de datos
            {
                string query = @"
                    SELECT 
                        p.IdPrestamo,
                        p.FechaPrestamo,
                        p.PendienteDevolucion,
                        pel.IdPelicula,
                        pel.Titulo,
                        pel.AnioLanzamiento,
                        pel.Idioma,
                        c.NombreCategoria
                    FROM Prestamo p
                    INNER JOIN Pelicula pel ON p.IdPelicula = pel.IdPelicula
                    INNER JOIN CategoriaPelicula c ON pel.idCategoria = c.IdCategoria
                    WHERE p.IdCliente = @IdCliente";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdCliente", idCliente); // Agrega el parámetro de 'IdCliente' a la consulta
                    connection.Open(); // Abre la conexión
                    SqlDataReader reader = command.ExecuteReader(); // Ejecuta la consulta y obtiene un lector de datos

                    while (reader.Read()) // Lee los datos de la consulta
                    {
                        object pelicula = new
                        {
                            IdPrestamo = reader.GetInt32(reader.GetOrdinal("IdPrestamo")), // Obtiene el Id del préstamo
                            FechaPrestamo = reader.GetDateTime(reader.GetOrdinal("FechaPrestamo")), // Obtiene la fecha de préstamo
                            PendienteDevolucion = reader.GetBoolean(reader.GetOrdinal("PendienteDevolucion")), // Obtiene el estado de devolución
                            IdPelicula = reader.GetInt32(reader.GetOrdinal("IdPelicula")), // Obtiene el Id de la película
                            Titulo = reader.GetString(reader.GetOrdinal("Titulo")), // Obtiene el título de la película
                            AnioLanzamiento = reader.GetInt32(reader.GetOrdinal("AnioLanzamiento")), // Obtiene el año de lanzamiento de la película
                            Idioma = reader.GetString(reader.GetOrdinal("Idioma")), // Obtiene el idioma de la película
                            NombreCategoria = reader.GetString(reader.GetOrdinal("NombreCategoria")) // Obtiene el nombre de la categoría de la película
                        };

                        misPeliculas.Add(pelicula); // Agrega la película a la lista
                    }

                }
                return misPeliculas;
            }
        }
    }
}
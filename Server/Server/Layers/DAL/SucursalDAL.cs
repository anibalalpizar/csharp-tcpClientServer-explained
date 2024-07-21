using Server.Models;
using Server.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Server.Layers.DAL
{
    public class SucursalDAL
    {
        string connectionString = DatabaseUtils.GetConnection().ConnectionString;

        // Método para insertar una sucursal en la base de datos
        public string InsertarSucursal(Sucursal sucursal)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string checkQuery = "SELECT COUNT(*) FROM Sucursal WHERE IdSucursal = @IdSucursal"; // Consulta para verificar si ya existe la sucursal
                using (SqlCommand command = new SqlCommand(checkQuery, connection))
                {
                    command.Parameters.AddWithValue("@IdSucursal", sucursal.IdSucursal);
                    connection.Open();
                    int count = (int)command.ExecuteScalar();

                    if (count > 0)
                    {
                        return "Error: El IdSucursal ya existe en la base de datos.";
                    }
                }

                string insertQuery = "INSERT INTO Sucursal (IdSucursal, idEncargado, Nombre, Direccion, Telefono, Activo) VALUES (@IdSucursal, @idEncargado, @Nombre, @Direccion, @Telefono, @Activo)";

                using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                {
                    insertCommand.Parameters.AddWithValue("@IdSucursal", sucursal.IdSucursal); // Agrega el parámetro de 'IdSucursal' a la consulta
                    insertCommand.Parameters.AddWithValue("@IdEncargado", sucursal.Encargado.IdEncargado); // Agrega el parámetro de 'idEncargado' a la consulta
                    insertCommand.Parameters.AddWithValue("@Nombre", sucursal.Nombre); // Agrega el parámetro de 'Nombre' a la consulta
                    insertCommand.Parameters.AddWithValue("@Direccion", sucursal.Direccion); // Agrega el parámetro de 'Direccion' a la consulta
                    insertCommand.Parameters.AddWithValue("@Telefono", sucursal.Telefono); // Agrega el parámetro de 'Telefono' a la consulta
                    insertCommand.Parameters.AddWithValue("@Activo", sucursal.Activo); // Agrega el parámetro de 'Activo' a la consulta

                    insertCommand.ExecuteNonQuery(); // Ejecuta la consulta de inserción


                }
            }
            return "Éxito: Sucursal insertada correctamente.";
        }

        // Método para obtener todas las sucursales de la base de datos
        public List<object> ObtenerTodasSucursales()
        {
            try
            {
                List<object> sucursales = new List<object>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"
                SELECT 
                    S.IdSucursal, 
                    S.IdEncargado, 
                    S.Nombre AS SucursalNombre, 
                    S.Direccion, 
                    S.Telefono, 
                    S.Activo, 
                    E.Identificacion AS EncargadoIdentificacion, 
                    E.FechaIngreso AS EncargadoFechaIngreso, 
                    P.Nombre AS PersonaNombre, 
                    P.PrimerApellido, 
                    P.SegundoApellido, 
                    P.FechaNacimiento 
                FROM 
                    Sucursal S
                JOIN 
                    Encargado E ON S.IdEncargado = E.IdEncargado
                JOIN 
                    Persona P ON E.Identificacion = P.Identificacion";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var sucursal = new
                                {
                                    IdSucursal = reader.GetInt32(0),
                                    IdEncargado = reader.GetInt32(1),
                                    SucursalNombre = reader.GetString(2),
                                    Direccion = reader.GetString(3),
                                    Telefono = reader.GetString(4),
                                    Activo = reader.GetBoolean(5),
                                    EncargadoIdentificacion = reader.GetString(6),
                                    EncargadoFechaIngreso = reader.GetDateTime(7),
                                    PersonaNombre = reader.GetString(8),
                                    PrimerApellido = reader.GetString(9),
                                    SegundoApellido = reader.GetString(10),
                                    FechaNacimiento = reader.GetDateTime(11)
                                };
                                sucursales.Add(sucursal);
                            }
                        }
                    }
                }
                return sucursales;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener las sucursales: {ex.Message}");
            }
        }

        // Método para obtener las peliculas de una sucursal
        public List<object> ObtenerPeliculasPorSucursal(int idSucursal)
        {
            try
            {
                List<object> peliculas = new List<object>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"
                    
                    SELECT 
                        ps.IdSucursal,
                        ps.IdPelicula,
                        ps.Cantidad,
                        p.IdCategoria,
                        p.Titulo,
                        p.AnioLanzamiento,
                        p.Idioma,
                        c.IdCategoria,
                        c.NombreCategoria,
                        c.Descripcion
                    FROM 
                        PeliculaXSucursal ps
                    JOIN 
                        Pelicula p ON ps.IdPelicula = p.IdPelicula
                    JOIN 
                        CategoriaPelicula c ON p.IdCategoria = c.IdCategoria
                    WHERE 
                        ps.IdSucursal = @IdSucursal";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdSucursal", idSucursal);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var pelicula = new
                                {
                                    IdSucursal = reader.GetInt32(0),
                                    IdPelicula = reader.GetInt32(1),
                                    Cantidad = reader.GetInt32(2),
                                    IdCategoria = reader.GetInt32(3),
                                    Titulo = reader.GetString(4),
                                    AnioLanzamiento = reader.GetInt32(5),
                                    Idioma = reader.GetString(6),
                                    NombreCategoria = reader.GetString(8),
                                    Descripcion = reader.GetString(9)
                                };
                                peliculas.Add(pelicula);
                            }
                        }
                    }
                }
                return peliculas;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener las películas por sucursal: {ex.Message}");
            }
        }

    }
}

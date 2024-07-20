using Server.Models;
using Server.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Layers.DAL
{
    public class PeliculaXSucursalDAL
    {
        string connectionString = DatabaseUtils.GetConnection().ConnectionString;

        public string InsertarPeliculaXSucursal(PeliculaXSucursal request)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    foreach (var pelicula in request.Peliculas)
                    {
                        // Validar si la combinación de sucursal y película ya existe
                        string queryValidar = "SELECT COUNT(*) FROM PeliculaXSucursal WHERE idSucursal = @idSucursal AND idPelicula = @idPelicula";
                        using (SqlCommand cmdValidar = new SqlCommand(queryValidar, connection, transaction))
                        {
                            cmdValidar.Parameters.AddWithValue("@idSucursal", request.IdSucursal.IdSucursal);
                            cmdValidar.Parameters.AddWithValue("@idPelicula", pelicula.IdPelicula);

                            int count = (int)cmdValidar.ExecuteScalar();
                            if (count > 0)
                            {
                                transaction.Rollback();
                                return $"Error: La combinación de sucursal {request.IdSucursal.IdSucursal} y película {pelicula.IdPelicula} ya existe.";
                            }
                        }

                        // Insertar la nueva relación si no existe
                        string queryInsertar = "INSERT INTO PeliculaXSucursal (idSucursal, idPelicula, Cantidad) VALUES (@idSucursal, @idPelicula, @cantidad)";
                        using (SqlCommand cmdInsertar = new SqlCommand(queryInsertar, connection, transaction))
                        {
                            cmdInsertar.Parameters.AddWithValue("@idSucursal", request.IdSucursal.IdSucursal);
                            cmdInsertar.Parameters.AddWithValue("@idPelicula", pelicula.IdPelicula);
                            cmdInsertar.Parameters.AddWithValue("@cantidad", request.Cantidad);

                            cmdInsertar.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                    return "Éxito: Películas y sucursal relacionadas correctamente.";
                }
                catch (Exception ex)
                {
                    return $"Error: {ex.Message}";
                }
            }
        }

        public List<object> ObtenerTodasPeliculasXSucursales()
        {
            try
            {
                List<object> pelicualasXSucursales = new List<object>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Consulta unificada con LEFT JOIN para obtener sucursales, películas y categorías
                    string query = @"
                    SELECT 
                        S.IdSucursal, 
                        S.Nombre AS SucursalNombre, 
                        S.Direccion, 
                        S.Telefono, 
                        S.Activo,
                        P.IdPelicula,
                        P.Titulo AS PeliculaTitulo,
                        P.AnioLanzamiento,
                        C.IdCategoria,
                        C.NombreCategoria AS CategoriaNombre,
                        C.Descripcion AS CategoriaDescripcion,
                        PS.Cantidad
                    FROM 
                        Sucursal S
                    LEFT JOIN 
                        PeliculaXSucursal PS ON S.IdSucursal = PS.IdSucursal
                    LEFT JOIN 
                        Pelicula P ON PS.IdPelicula = P.IdPelicula
                    LEFT JOIN 
                        CategoriaPelicula C ON P.IdCategoria = C.IdCategoria";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            List<object> datosCombinados = new List<object>();

                            while (reader.Read())
                            {
                                var dato = new
                                {
                                    IdSucursal = reader.GetInt32(0),
                                    SucursalNombre = reader.GetString(1),
                                    Direccion = reader.GetString(2),
                                    Telefono = reader.GetString(3),
                                    Activo = reader.GetBoolean(4),
                                    IdPelicula = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5),
                                    PeliculaTitulo = reader.IsDBNull(6) ? null : reader.GetString(6),
                                    AnioLanzamiento = reader.IsDBNull(7) ? (int?)null : reader.GetInt32(7),
                                    IdCategoria = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8),
                                    CategoriaNombre = reader.IsDBNull(9) ? null : reader.GetString(9),
                                    CategoriaDescripcion = reader.IsDBNull(10) ? null : reader.GetString(10),
                                    Cantidad = reader.IsDBNull(11) ? (int?)null : reader.GetInt32(11)
                                };
                                pelicualasXSucursales.Add(dato);
                            }
                        }
                    }
                }

                return pelicualasXSucursales;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener toda la información: {ex.Message}");
            }
        }

    }
}
using Server.Models;
using Server.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Server.Layers.DAL
{
    public class ClienteDAL
    {
        string connectionString = DatabaseUtils.GetConnection().ConnectionString;

        // Método para insertar un cliente
        public string InsertarCliente(Cliente cliente)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // Verificar si el IdCliente ya existe
                    string checkIdQuery = "SELECT COUNT(*) FROM Cliente WHERE IdCliente = @IdCliente";
                    using (SqlCommand checkIdCommand = new SqlCommand(checkIdQuery, connection, transaction))
                    {
                        checkIdCommand.Parameters.AddWithValue("@IdCliente", cliente.IdCliente);
                        int idCount = (int)checkIdCommand.ExecuteScalar();
                        if (idCount > 0)
                        {
                            return "Error: El IdCliente ya existe en la base de datos.";
                        }
                    }

                    // Verificar si la Identificacion ya existe en la tabla Cliente con un IdCliente diferente
                    string checkIdentificacionQuery = "SELECT COUNT(*) FROM Cliente WHERE Identificacion = @Identificacion AND IdCliente != @IdCliente";
                    using (SqlCommand checkIdentificacionCommand = new SqlCommand(checkIdentificacionQuery, connection, transaction))
                    {
                        checkIdentificacionCommand.Parameters.AddWithValue("@Identificacion", cliente.Identificacion);
                        checkIdentificacionCommand.Parameters.AddWithValue("@IdCliente", cliente.IdCliente);
                        int identificacionCount = (int)checkIdentificacionCommand.ExecuteScalar();
                        if (identificacionCount > 0)
                        {
                            return "Error: La Identificación ya existe en la base de datos con un IdCliente diferente.";
                        }
                    }

                    // Verificar si la persona existe en la tabla Persona
                    string checkPersonaQuery = "SELECT COUNT(*) FROM Persona WHERE Identificacion = @Identificacion";
                    using (SqlCommand checkPersonaCommand = new SqlCommand(checkPersonaQuery, connection, transaction))
                    {
                        checkPersonaCommand.Parameters.AddWithValue("@Identificacion", cliente.Identificacion);
                        int personaCount = (int)checkPersonaCommand.ExecuteScalar();
                        if (personaCount == 0)
                        {
                            // Insertar en la tabla Persona si no existe
                            string insertPersonaQuery = @"
                                INSERT INTO Persona (Identificacion, Nombre, PrimerApellido, SegundoApellido, FechaNacimiento)
                                VALUES (@Identificacion, @Nombre, @PrimerApellido, @SegundoApellido, @FechaNacimiento)";
                            using (SqlCommand insertPersonaCommand = new SqlCommand(insertPersonaQuery, connection, transaction))
                            {
                                insertPersonaCommand.Parameters.AddWithValue("@Identificacion", cliente.Identificacion);
                                insertPersonaCommand.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                                insertPersonaCommand.Parameters.AddWithValue("@PrimerApellido", cliente.Apellido1);
                                insertPersonaCommand.Parameters.AddWithValue("@SegundoApellido", cliente.Apellido2);
                                insertPersonaCommand.Parameters.AddWithValue("@FechaNacimiento", cliente.FechaNacimiento);

                                insertPersonaCommand.ExecuteNonQuery();
                            }
                        }
                    }

                    // Insertar el registro del Cliente
                    string insertClienteQuery = @"
                        INSERT INTO Cliente (IdCliente, Identificacion, FechaRegistro, Activo)
                        VALUES (@IdCliente, @Identificacion, @FechaIngreso, @Activo)";
                    using (SqlCommand insertClienteCommand = new SqlCommand(insertClienteQuery, connection, transaction))
                    {
                        insertClienteCommand.Parameters.AddWithValue("@IdCliente", cliente.IdCliente);
                        insertClienteCommand.Parameters.AddWithValue("@Identificacion", cliente.Identificacion);
                        insertClienteCommand.Parameters.AddWithValue("@FechaIngreso", cliente.FechaIngreso);
                        insertClienteCommand.Parameters.AddWithValue("@Activo", cliente.Activo);

                        insertClienteCommand.ExecuteNonQuery();
                    }

                    // Confirmar la transacción
                    transaction.Commit();
                    return "Cliente registrado exitosamente.";


                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return "Error: " + ex.Message;
                }
            }
        }

        // Método para obtener todos los clientes
        public List<Cliente> ObtenerTodosClientes()
        {
            List<Cliente> clientes = new List<Cliente>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT C.IdCliente, C.Identificacion, P.Nombre, P.PrimerApellido, P.SegundoApellido, P.FechaNacimiento, C.FechaRegistro, C.Activo FROM Cliente C JOIN Persona P ON C.Identificacion = P.Identificacion";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Cliente cliente = new Cliente
                            {
                                IdCliente = reader.GetInt32(0),
                                Identificacion = reader.GetString(1),
                                Nombre = reader.GetString(2),
                                Apellido1 = reader.GetString(3),
                                Apellido2 = reader.GetString(4),
                                FechaNacimiento = reader.GetDateTime(5),
                                FechaIngreso = reader.GetDateTime(6),
                                Activo = reader.GetBoolean(7)
                            };
                            clientes.Add(cliente);
                        }
                    }
                }
                return clientes;
            }
        }
    }
}
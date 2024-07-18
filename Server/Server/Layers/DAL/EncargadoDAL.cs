using Server.Models; // Importa los modelos de datos del servidor
using Server.Utils; // Importa las utilidades del servidor
using System; // Importa funcionalidades básicas del sistema
using System.Collections.Generic; // Importa funcionalidades para trabajar con colecciones genéricas
using System.Data.SqlClient; // Importa funcionalidades para trabajar con bases de datos SQL

namespace Server.Layers.DAL // Define el espacio de nombres 'Server.Layers.DAL'
{
    // Define la clase 'EncargadoDAL' para manejar operaciones de acceso a datos para 'Encargado'
    public class EncargadoDAL
    {
        // Obtiene la cadena de conexión desde las utilidades de la base de datos
        string connectionString = DatabaseUtils.GetConnection().ConnectionString;

        // Método para insertar un nuevo encargado en la base de datos
        public string InsertarEncargado(Encargado encargado)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // Verificar si el IdEncargado ya existe
                    string checkIdQuery = "SELECT COUNT(*) FROM Encargado WHERE IdEncargado = @IdEncargado";
                    using (SqlCommand checkIdCommand = new SqlCommand(checkIdQuery, connection, transaction))
                    {
                        checkIdCommand.Parameters.AddWithValue("@IdEncargado", encargado.IdEncargado);
                        int idCount = (int)checkIdCommand.ExecuteScalar();
                        if (idCount > 0)
                        {
                            return "Error: El IdEncargado ya existe en la base de datos.";
                        }
                    }

                    // Verificar si la Identificacion ya existe en la tabla Encargado con un IdEncargado diferente
                    string checkIdentificacionQuery = "SELECT COUNT(*) FROM Encargado WHERE Identificacion = @Identificacion AND IdEncargado != @IdEncargado";
                    using (SqlCommand checkIdentificacionCommand = new SqlCommand(checkIdentificacionQuery, connection, transaction))
                    {
                        checkIdentificacionCommand.Parameters.AddWithValue("@Identificacion", encargado.Identificacion);
                        checkIdentificacionCommand.Parameters.AddWithValue("@IdEncargado", encargado.IdEncargado);
                        int identificacionCount = (int)checkIdentificacionCommand.ExecuteScalar();
                        if (identificacionCount > 0)
                        {
                            return "Error: La Identificación ya existe en la base de datos con un IdEncargado diferente.";
                        }
                    }

                    // Verificar si la persona existe en la tabla Persona
                    string checkPersonaQuery = "SELECT COUNT(*) FROM Persona WHERE Identificacion = @Identificacion";
                    using (SqlCommand checkPersonaCommand = new SqlCommand(checkPersonaQuery, connection, transaction))
                    {
                        checkPersonaCommand.Parameters.AddWithValue("@Identificacion", encargado.Identificacion);
                        int personaCount = (int)checkPersonaCommand.ExecuteScalar();
                        if (personaCount == 0)
                        {
                            // Insertar en la tabla Persona si no existe
                            string insertPersonaQuery = @"
                                INSERT INTO Persona (Identificacion, Nombre, PrimerApellido, SegundoApellido, FechaNacimiento)
                                VALUES (@Identificacion, @Nombre, @PrimerApellido, @SegundoApellido, @FechaNacimiento)";
                            using (SqlCommand insertPersonaCommand = new SqlCommand(insertPersonaQuery, connection, transaction))
                            {
                                insertPersonaCommand.Parameters.AddWithValue("@Identificacion", encargado.Identificacion);
                                insertPersonaCommand.Parameters.AddWithValue("@Nombre", encargado.Nombre);
                                insertPersonaCommand.Parameters.AddWithValue("@PrimerApellido", encargado.Apellido1);
                                insertPersonaCommand.Parameters.AddWithValue("@SegundoApellido", encargado.Apellido2);
                                insertPersonaCommand.Parameters.AddWithValue("@FechaNacimiento", encargado.FechaNacimiento);

                                insertPersonaCommand.ExecuteNonQuery();
                            }
                        }
                    }

                    // Insertar el registro del Encargado
                    string insertEncargadoQuery = @"
                        INSERT INTO Encargado (IdEncargado, Identificacion, FechaIngreso)
                        VALUES (@IdEncargado, @Identificacion, @FechaIngreso)";
                    using (SqlCommand insertEncargadoCommand = new SqlCommand(insertEncargadoQuery, connection, transaction))
                    {
                        insertEncargadoCommand.Parameters.AddWithValue("@IdEncargado", encargado.IdEncargado);
                        insertEncargadoCommand.Parameters.AddWithValue("@Identificacion", encargado.Identificacion);
                        insertEncargadoCommand.Parameters.AddWithValue("@FechaIngreso", encargado.FechaIngreso);

                        insertEncargadoCommand.ExecuteNonQuery();
                    }

                    // Confirma la transacción
                    transaction.Commit();
                    return "Encargado registrado exitosamente.";
                }
                catch (Exception ex)
                {
                    // Revierte la transacción en caso de error
                    transaction.Rollback();
                    return $"Error al registrar el encargado: {ex.Message}";
                }
            }
        }

        // Método para obtener todos los encargados de la base de datos
        public List<Encargado> ObtenerTodosEncargados()
        {
            List<Encargado> encargados = new List<Encargado>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT E.IdEncargado, P.Identificacion, P.Nombre, P.PrimerApellido, P.SegundoApellido, P.FechaNacimiento, E.FechaIngreso
                    FROM Encargado E
                    JOIN Persona P ON E.Identificacion = P.Identificacion";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Encargado encargado = new Encargado
                            {
                                IdEncargado = reader.GetInt32(0),
                                Identificacion = reader.GetString(1),
                                Nombre = reader.GetString(2),
                                Apellido1 = reader.GetString(3),
                                Apellido2 = reader.GetString(4),
                                FechaNacimiento = reader.GetDateTime(5),
                                FechaIngreso = reader.GetDateTime(6)
                            };
                            encargados.Add(encargado);
                        }
                    }
                }
            }
            return encargados;
        }
    }
}

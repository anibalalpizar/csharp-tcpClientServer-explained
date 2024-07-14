using Server.Utils;               // Importa el espacio de nombres para utilidades, como `DatabaseUtils` para obtener la cadena de conexión a la base de datos.
using System;                    // Importa el espacio de nombres para los tipos básicos de .NET.
using System.Data.SqlClient;     // Importa el espacio de nombres para la interacción con bases de datos SQL Server.

namespace Server.Layers.DAL        // Define un espacio de nombres para la capa de acceso a datos (DAL).
{
    public class ClientDAL         // Define una clase pública que contiene métodos para acceder a la base de datos relacionada con clientes.
    {
        // Método estático para validar un cliente basado en su identificación.
        public static bool ValidateCliente(string idOrIdentification)
        {
            try
            {
                // Obtiene la cadena de conexión desde las utilidades de la base de datos.
                string connectionString = DatabaseUtils.GetConnection().ConnectionString;

                // Crea una nueva conexión a la base de datos utilizando la cadena de conexión.
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open(); // Abre la conexión a la base de datos.

                    // Consulta SQL para verificar si existe un cliente con el ID o la identificación proporcionada y que esté activo.
                    string query = @"
                        SELECT COUNT(*) 
                        FROM Cliente 
                        WHERE (IdCliente = @IdOrIdentification OR Identificacion = @IdOrIdentification) 
                          AND Activo = 1";

                    // Crea un comando SQL utilizando la consulta y la conexión.
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Agrega el parámetro a la consulta SQL para prevenir inyecciones SQL.
                        command.Parameters.AddWithValue("@IdOrIdentification", idOrIdentification);

                        // Ejecuta el comando y obtiene el resultado de la consulta (número de registros).
                        int count = (int)command.ExecuteScalar();

                        // Retorna true si se encontró al menos un cliente, false en caso contrario.
                        return count > 0;
                    }
                }
            }
            catch (Exception ex) // Captura cualquier excepción que ocurra durante la validación.
            {
                // Imprime el mensaje de la excepción en la consola.
                Console.WriteLine($"Error al validar el cliente: {ex.Message}");
                // Imprime el detalle de la excepción (pila de llamadas) en la consola para depuración.
                Console.WriteLine($"Detalle del error: {ex.StackTrace}");
                // Retorna false si ocurre una excepción durante la validación.
                return false;
            }
        }

        // Método estático para obtener el nombre completo de una persona basado en su identificación.
        public static string GetFullName(string idOrIdentification)
        {
            try
            {
                // Obtiene la cadena de conexión desde las utilidades de la base de datos.
                string connectionString = DatabaseUtils.GetConnection().ConnectionString;

                // Crea una nueva conexión a la base de datos utilizando la cadena de conexión.
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open(); // Abre la conexión a la base de datos.

                    string identificacion = idOrIdentification; // Inicializa la variable de identificación con el valor proporcionado.

                    // Consulta SQL para obtener la identificación del cliente basada en el ID del cliente.
                    string queryCliente = @"
                        SELECT Identificacion
                        FROM Cliente
                        WHERE IdCliente = @IdOrIdentification";

                    // Crea un comando SQL utilizando la consulta y la conexión.
                    using (SqlCommand commandCliente = new SqlCommand(queryCliente, connection))
                    {
                        // Agrega el parámetro a la consulta SQL.
                        commandCliente.Parameters.AddWithValue("@IdOrIdentification", idOrIdentification);

                        // Ejecuta el comando y obtiene el resultado (identificación del cliente).
                        object resultCliente = commandCliente.ExecuteScalar();
                        if (resultCliente != null)
                        {
                            // Si se encuentra una identificación, actualiza la variable `identificacion`.
                            identificacion = resultCliente.ToString();
                        }
                    }

                    // Consulta SQL para obtener el nombre completo basado en la identificación.
                    string queryPersona = @"
                        SELECT CONCAT(Nombre, ' ', PrimerApellido, ' ', SegundoApellido) AS FullName
                        FROM Persona
                        WHERE Identificacion = @Identificacion";

                    // Crea un comando SQL utilizando la consulta y la conexión.
                    using (SqlCommand commandPersona = new SqlCommand(queryPersona, connection))
                    {
                        // Agrega el parámetro a la consulta SQL.
                        commandPersona.Parameters.AddWithValue("@Identificacion", identificacion);

                        // Ejecuta el comando y obtiene el resultado (nombre completo).
                        object resultPersona = commandPersona.ExecuteScalar();
                        // Retorna el nombre completo si se encuentra, null en caso contrario.
                        return resultPersona != null ? resultPersona.ToString() : null;
                    }
                }
            }
            catch (Exception ex) // Captura cualquier excepción que ocurra durante la obtención del nombre completo.
            {
                // Imprime el mensaje de la excepción en la consola.
                Console.WriteLine($"Error al obtener el nombre completo: {ex.Message}");
                // Imprime el detalle de la excepción (pila de llamadas) en la consola para depuración.
                Console.WriteLine($"Detalle del error: {ex.StackTrace}");
                // Retorna null si ocurre una excepción durante la obtención del nombre completo.
                return null;
            }
        }
    }
}

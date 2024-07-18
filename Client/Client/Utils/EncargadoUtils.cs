using Client.Models; // Importa los modelos de datos del cliente
using Newtonsoft.Json; // Importa la librería JSON para serialización y deserialización
using Newtonsoft.Json.Linq; // Importa la librería JSON para manipulación de objetos JSON
using System; // Importa funcionalidades básicas del sistema
using System.Collections.Generic; // Importa funcionalidades para trabajar con colecciones genéricas
using System.Net.Sockets; // Importa funcionalidades para trabajar con conexiones de red
using System.Text; // Importa funcionalidades para trabajar con texto y codificación

namespace Client.Utils // Define el espacio de nombres 'Client.Utils'
{
    // Define la clase 'EncargadoUtils' para utilidades relacionadas con 'Encargado'
    public class EncargadoUtils
    {
        // Método para registrar un nuevo encargado
        public string RegistrarEncargado(int idEncargado, string identificacion, string nombre, string apellido1, string apellido2, DateTime fechaNacimiento, DateTime fechaIngreso)
        {
            // Crea una nueva instancia de 'Encargado' con los datos proporcionados
            Encargado encargado = new Encargado
            {
                IdEncargado = idEncargado,
                Identificacion = identificacion,
                Nombre = nombre,
                Apellido1 = apellido1,
                Apellido2 = apellido2,
                FechaNacimiento = fechaNacimiento,
                FechaIngreso = fechaIngreso,
                Accion = "Creando"
            };

            // Serializa el objeto 'Encargado' a JSON
            string jsonData = JsonConvert.SerializeObject(encargado);

            // Convierte el JSON a un objeto JObject para manipulación adicional
            JObject jsonObject = JObject.Parse(jsonData);
            jsonObject["FechaNacimiento"] = fechaNacimiento;
            jsonObject["FechaIngreso"] = fechaIngreso;

            // Convierte el JObject modificado de vuelta a una cadena JSON
            string modifiedJsonData = jsonObject.ToString();
            // Codifica la cadena JSON a bytes
            byte[] data = Encoding.UTF8.GetBytes(modifiedJsonData);

            try
            {
                // Establece una conexión TCP con el servidor
                using (TcpClient client = new TcpClient("127.0.0.1", 15500))
                {
                    NetworkStream stream = client.GetStream();
                    // Envía los datos al servidor
                    stream.Write(data, 0, data.Length);

                    // Lee la respuesta del servidor
                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    // Devuelve la respuesta del servidor
                    return response;
                }
            }
            catch (Exception ex)
            {
                // Devuelve un mensaje de error en caso de excepción
                return $"Error al registrar el encargado: {ex.Message}";
            }
        }

        // Método para obtener todos los encargados
        public List<Encargado> ObtenerTodos()
        {
            // Crea una nueva instancia de 'Encargado' para solicitar datos
            Encargado solicitud = new Encargado
            {
                Accion = "Obteniendo"
            };

            // Serializa el objeto de solicitud a JSON
            string jsonData = JsonConvert.SerializeObject(solicitud);
            // Codifica la cadena JSON a bytes
            byte[] data = Encoding.UTF8.GetBytes(jsonData);

            try
            {
                // Establece una conexión TCP con el servidor
                using (TcpClient client = new TcpClient("127.0.0.1", 15500))
                {
                    NetworkStream stream = client.GetStream();
                    // Envía los datos al servidor
                    stream.Write(data, 0, data.Length);

                    // Lee la respuesta del servidor
                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    // Deserializa la respuesta JSON a una lista de encargados
                    List<Encargado> encargados = JsonConvert.DeserializeObject<List<Encargado>>(response);
                    return encargados; // Devuelve la lista de encargados
                }
            }
            catch (Exception ex)
            {
                // Muestra un mensaje de error en caso de excepción
                Console.WriteLine($"Error al obtener los encargados: {ex.Message}");
                return null; // Devuelve null en caso de error
            }
        }
    }
}

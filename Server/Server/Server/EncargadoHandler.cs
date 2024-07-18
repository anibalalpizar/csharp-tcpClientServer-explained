using Newtonsoft.Json; // Importa la librería JSON para serialización y deserialización
using Server.Layers.BLL; // Importa la capa de lógica de negocio
using Server.Models; // Importa los modelos de datos del servidor
using System; // Importa funcionalidades básicas del sistema
using System.Collections.Generic; // Importa funcionalidades para trabajar con colecciones genéricas
using System.Net.Sockets; // Importa funcionalidades para trabajar con conexiones de red
using System.Text; // Importa funcionalidades para trabajar con texto y codificación

namespace Server.Server // Define el espacio de nombres 'Server.Server'
{
    // Define la clase 'EncargadoHandler' para manejar operaciones relacionadas con 'Encargado'
    public class EncargadoHandler
    {
        // Crea una instancia de la capa de lógica de negocio para 'Encargado'
        private EncargadoBLL encargadoBLL = new EncargadoBLL();

        // Método para manejar las solicitudes relacionadas con 'Encargado'
        public void HandlerEncargado(TcpClient client, string request, Action<string> onUserAction)
        {
            try
            {
                // Verifica si la solicitud es para crear un nuevo encargado
                if (request.Contains("Creando"))
                {
                    // Deserializa la solicitud JSON a un objeto 'Encargado'
                    Encargado encargado = JsonConvert.DeserializeObject<Encargado>(request);
                    // Llama al método de la capa de lógica de negocio para registrar el encargado
                    string resultMessage = encargadoBLL.RegistrarEncargado(encargado);

                    // Obtiene el stream de red del cliente
                    using (NetworkStream stream = client.GetStream())
                    {
                        // Convierte el mensaje de resultado a bytes
                        byte[] responseBytes = Encoding.UTF8.GetBytes(resultMessage);
                        // Envía el mensaje de respuesta al cliente
                        stream.Write(responseBytes, 0, responseBytes.Length);

                        // Invoca la acción de usuario (si está definida) para registrar la creación
                        onUserAction?.Invoke("Un usuario ha creado o intentado crear un Encargado");
                    }
                }
                // Verifica si la solicitud es para obtener todos los encargados
                else if (request.Contains("Obteniendo"))
                {
                    // Llama al método de la capa de lógica de negocio para obtener la lista de encargados
                    List<Encargado> encargados = encargadoBLL.ObtenerEncargados();
                    // Serializa la lista de encargados a JSON
                    string resultMessage = JsonConvert.SerializeObject(encargados);

                    // Obtiene el stream de red del cliente
                    using (NetworkStream stream = client.GetStream())
                    {
                        // Convierte el mensaje de resultado a bytes
                        byte[] responseBytes = Encoding.UTF8.GetBytes(resultMessage);
                        // Envía el mensaje de respuesta al cliente
                        stream.Write(responseBytes, 0, responseBytes.Length);

                        // Invoca la acción de usuario (si está definida) para registrar la obtención
                        onUserAction?.Invoke("Un usuario ha obtenido encargados");
                    }
                }
            }
            catch (Exception ex)
            {
                // Muestra un mensaje de error en caso de excepción
                Console.WriteLine($"Error inesperado al manejar el encargado: {ex.Message}");
            }
        }
    }
}

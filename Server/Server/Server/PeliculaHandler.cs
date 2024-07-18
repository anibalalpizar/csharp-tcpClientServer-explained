using Newtonsoft.Json; // Importa la biblioteca Newtonsoft.Json para la serialización y deserialización de JSON
using Server.Layers.BLL; // Importa la capa de lógica de negocios (BLL)
using Server.Models; // Importa los modelos de datos
using System; // Importa funcionalidades básicas del sistema
using System.Collections.Generic; // Importa funcionalidades para trabajar con colecciones genéricas
using System.Net.Sockets; // Importa funcionalidades para trabajar con sockets de red
using System.Text; // Importa funcionalidades para trabajar con texto y codificaciones

namespace Server.Server // Define el espacio de nombres 'Server.Server'
{
    // Define la clase 'PeliculaHandler' que maneja las solicitudes relacionadas con películas
    public class PeliculaHandler
    {
        private PeliculaBLL peliculaBLL = new PeliculaBLL(); // Instancia de 'PeliculaBLL' para manejar la lógica de negocio de películas

        // Método que maneja las solicitudes relacionadas con películas
        public void HandlerPelicula(TcpClient client, string request, Action<string> onUserAction)
        {
            try
            {
                if (request.Contains("Creando")) // Verifica si la solicitud es para crear una película
                {
                    Pelicula pelicula = JsonConvert.DeserializeObject<Pelicula>(request); // Deserializa la solicitud JSON en un objeto 'Pelicula'
                    string resultMessage = peliculaBLL.RegistrarPelicula(pelicula); // Llama al método para registrar la película

                    using (NetworkStream stream = client.GetStream()) // Obtiene el flujo de red del cliente
                    {
                        byte[] responseBytes = Encoding.UTF8.GetBytes(resultMessage); // Convierte el mensaje de resultado en bytes
                        stream.Write(responseBytes, 0, responseBytes.Length); // Escribe la respuesta en el flujo de red

                        onUserAction?.Invoke("Un usuario ha creado o intentado crear una película."); // Invoca la acción 'onUserAction' si está definida
                    }
                }
                else if (request.Contains("Obteniendo")) // Verifica si la solicitud es para obtener películas
                {
                    List<Pelicula> peliculas = peliculaBLL.ObteniendoPelicula(); // Llama al método para obtener la lista de películas
                    string resultMessage = JsonConvert.SerializeObject(peliculas); // Serializa la lista de películas en un JSON

                    using (NetworkStream stream = client.GetStream()) // Obtiene el flujo de red del cliente
                    {
                        byte[] responseBytes = Encoding.UTF8.GetBytes(resultMessage); // Convierte el mensaje de resultado en bytes
                        stream.Write(responseBytes, 0, responseBytes.Length); // Escribe la respuesta en el flujo de red

                        onUserAction?.Invoke("Un usuario ha obtenido películas."); // Invoca la acción 'onUserAction' si está definida
                    }
                }
            }
            catch (Exception ex) // Captura cualquier excepción
            {
                Console.WriteLine($"Error inesperado al manejar el cliente: {ex.Message}"); // Imprime el mensaje de error
            }
        }
    }
}

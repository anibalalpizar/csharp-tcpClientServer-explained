using Server.Layers.BLL; // Importa la capa de lógica de negocios (BLL)
using Server.Layers.DAL; // Importa la capa de acceso a datos (DAL)
using Server.Models; // Importa los modelos de datos
using System; // Importa funcionalidades básicas del sistema
using System.Collections.Generic; // Importa funcionalidades para trabajar con colecciones genéricas
using System.IO; // Importa funcionalidades para trabajar con E/S de archivos
using System.Net.Sockets; // Importa funcionalidades para trabajar con sockets de red
using System.Text; // Importa funcionalidades para trabajar con texto y codificaciones
using System.Threading; // Importa funcionalidades para trabajar con hilos y cancelación de tareas

namespace Server.Server // Define el espacio de nombres 'Server.Server'
{
    // Define la clase 'ClientHandler' que maneja las conexiones con clientes
    public class ClientHandler
    {
        private CategoriaPeliculaHandler _categoriaPeliculaHandler; // Instancia de 'CategoriaPeliculaHandler' para manejar categorías de películas

        // Constructor que inicializa la instancia de 'CategoriaPeliculaHandler'
        public ClientHandler()
        {
            _categoriaPeliculaHandler = new CategoriaPeliculaHandler();
        }

        // Método de instancia que maneja la conexión con un cliente específico
        public void HandleClient(
            TcpClient client, // Cliente TCP que se conectó
            List<TcpClient> connectedClients, // Lista de clientes conectados
            CancellationToken cancellationToken, // Token de cancelación para manejar la cancelación de la tarea
            Action<string> onUserConnected, // Acción que se ejecuta cuando un usuario se conecta
            Action<string> onUserAction) // Acción que se ejecuta cuando un usuario realiza una acción
        {
            NetworkStream stream = null;

            try
            {
                stream = client.GetStream(); // Obtiene el flujo de red del cliente
                byte[] buffer = new byte[1024]; // Buffer para leer datos del flujo de red
                int bytesRead;

                while (!cancellationToken.IsCancellationRequested) // Bucle principal que se ejecuta mientras no se cancele la tarea
                {
                    try
                    {
                        if (stream.CanRead && stream.DataAvailable) // Verifica si el flujo de red se puede leer y hay datos disponibles
                        {
                            bytesRead = stream.Read(buffer, 0, buffer.Length); // Lee datos del flujo de red
                            if (bytesRead == 0)
                            {
                                break; // Si no se leen bytes, se rompe el bucle
                            }

                            string request = Encoding.UTF8.GetString(buffer, 0, bytesRead); // Convierte los bytes leídos en una cadena

                            if (request.Contains("IdCategoria")) // Verifica si la solicitud contiene "IdCategoria"
                            {
                                _categoriaPeliculaHandler.HandlerCategoriaPelicula(client, request, onUserAction); // Maneja la solicitud de categoría de película
                            }
                            else
                            {
                                string response;
                                bool isAuthenticated = ClientBLL.Authenticate(request); // Autentica la solicitud
                                if (isAuthenticated)
                                {
                                    string fullName = ClientDAL.GetFullName(request); // Obtiene el nombre completo del cliente
                                    response = fullName != null ? fullName : "Nombre no encontrado"; // Establece la respuesta

                                    onUserConnected?.Invoke(fullName); // Invoca la acción 'onUserConnected' si está definida
                                }
                                else
                                {
                                    response = null;
                                }

                                byte[] responseBytes = Encoding.UTF8.GetBytes(response); // Convierte la respuesta en bytes
                                stream.Write(responseBytes, 0, responseBytes.Length); // Escribe la respuesta en el flujo de red
                            }
                        }
                    }
                    catch (IOException ex) // Captura excepciones de E/S
                    {
                        Console.WriteLine($"Error de IO al manejar el cliente: {ex.Message}");
                        break; // Si ocurre un error de E/S, se rompe el bucle
                    }
                }
            }
            catch (OperationCanceledException) // Captura excepciones de operación cancelada
            {
                Console.WriteLine("Operación cancelada.");
            }
            catch (Exception ex) // Captura cualquier otra excepción
            {
                Console.WriteLine($"Error al manejar el cliente: {ex.Message}");
                Console.WriteLine($"Detalle del error: {ex.StackTrace}");
            }
            finally
            {
                try
                {
                    if (stream != null)
                    {
                        stream.Close(); // Cierra el flujo de red si no es nulo
                    }
                    client.Close(); // Cierra la conexión del cliente
                }
                catch (Exception ex) // Captura cualquier excepción al cerrar el cliente
                {
                    Console.WriteLine($"Error al cerrar el cliente: {ex.Message}");
                }
                lock (connectedClients)
                {
                    connectedClients.Remove(client); // Elimina el cliente de la lista de clientes conectados de manera segura para hilos
                }
            }
        }
    }
}

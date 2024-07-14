using Server.Layers.BLL;         // Importa el espacio de nombres BLL (Business Logic Layer) para usar la lógica de negocio.
using Server.Layers.DAL;         // Importa el espacio de nombres DAL (Data Access Layer) para acceder a la base de datos.
using System;                   // Importa tipos básicos del sistema como DateTime, Exception, etc.
using System.Collections.Generic; // Importa tipos genéricos como List<T> para manejar colecciones de objetos.
using System.IO;                // Importa tipos para manejar operaciones de entrada/salida, como streams y archivos.
using System.Net.Sockets;       // Importa tipos para manejar la comunicación de red a través de sockets.
using System.Text;              // Importa tipos para manejar la codificación de texto, como Encoding.
using System.Threading;         // Importa tipos para manejar la multitarea, como CancellationToken y Task.

namespace Server.Server          // Define un espacio de nombres para organizar el código relacionado con el servidor.
{
    public class ClientHandler   // Define una clase pública que maneja la interacción con los clientes conectados.
    {
        // Método estático que maneja la conexión con un cliente específico.
        public static void HandleClient(
            TcpClient client,                        // El cliente TCP que se está manejando.
            List<TcpClient> connectedClients,        // Lista de clientes conectados al servidor.
            CancellationToken cancellationToken,     // Token para cancelar la operación de manejo del cliente si es necesario.
            Action<string> onUserConnected)          // Acción que se llama cuando un usuario se conecta, pasando el nombre del usuario.
        {
            NetworkStream stream = null;             // Stream para leer y escribir datos desde/hacia el cliente.

            try
            {
                stream = client.GetStream();         // Obtiene el NetworkStream del cliente para la comunicación de red.
                byte[] buffer = new byte[1024];     // Buffer para almacenar datos leídos del stream.
                int bytesRead;                     // Variable para almacenar la cantidad de bytes leídos del stream.

                // Bucle para manejar la comunicación con el cliente mientras la operación no haya sido cancelada.
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        // Verifica si el stream puede leer datos y si hay datos disponibles para leer.
                        if (stream.CanRead && stream.DataAvailable)
                        {
                            bytesRead = stream.Read(buffer, 0, buffer.Length); // Lee datos del stream en el buffer.
                            if (bytesRead == 0) // Si no se leyeron datos (es decir, el cliente cerró la conexión).
                            {
                                break; // Sale del bucle si no se leyeron datos.
                            }

                            // Convierte los bytes leídos en una cadena usando la codificación ASCII.
                            string request = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                            string response; // Variable para almacenar la respuesta que se enviará al cliente.
                            bool isAuthenticated = ClientBLL.Authenticate(request); // Llama a la lógica de negocio para autenticar la solicitud.
                            if (isAuthenticated) // Si la autenticación es exitosa.
                            {
                                // Obtiene el nombre completo del usuario desde la capa de acceso a datos.
                                string fullName = ClientDAL.GetFullName(request);
                                response = fullName != null ? fullName : "Nombre no encontrado"; // Respuesta a enviar al cliente.

                                onUserConnected?.Invoke(fullName); // Llama a la acción para notificar que un usuario se ha conectado.
                            }
                            else
                            {
                                response = null; // Si la autenticación falla, no hay respuesta.
                            }

                            // Convierte la respuesta en bytes y la envía al cliente.
                            byte[] responseBytes = Encoding.ASCII.GetBytes(response);
                            stream.Write(responseBytes, 0, responseBytes.Length);
                        }
                    }
                    catch (IOException ex) // Captura excepciones relacionadas con operaciones de entrada/salida.
                    {
                        Console.WriteLine($"Error de IO al manejar el cliente: {ex.Message}"); // Muestra el error en la consola.
                        break; // Sale del bucle si ocurre un error de IO.
                    }
                }
            }
            catch (OperationCanceledException) // Captura excepciones relacionadas con la cancelación de la operación.
            {
                Console.WriteLine("Operación cancelada."); // Muestra un mensaje de cancelación en la consola.
            }
            catch (Exception ex) // Captura cualquier otra excepción no específica.
            {
                Console.WriteLine($"Error al manejar el cliente: {ex.Message}"); // Muestra el error en la consola.
                Console.WriteLine($"Detalle del error: {ex.StackTrace}"); // Muestra el detalle del error (traza de la pila).
            }
            finally
            {
                try
                {
                    if (stream != null) // Si el stream no es nulo.
                    {
                        stream.Close(); // Cierra el stream para liberar recursos.
                    }
                    client.Close(); // Cierra la conexión con el cliente.
                }
                catch (Exception ex) // Captura cualquier excepción al cerrar el cliente.
                {
                    Console.WriteLine($"Error al cerrar el cliente: {ex.Message}"); // Muestra el error en la consola.
                }
                lock (connectedClients) // Bloquea la lista de clientes para evitar problemas de concurrencia.
                {
                    connectedClients.Remove(client); // Elimina el cliente de la lista de clientes conectados.
                }
            }
        }
    }
}

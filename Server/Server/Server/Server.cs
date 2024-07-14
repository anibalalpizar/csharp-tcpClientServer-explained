using System;                  // Importa el espacio de nombres para los tipos básicos de .NET, como excepciones y console.
using System.Collections.Generic; // Importa el espacio de nombres para manejar colecciones genéricas como List<T>.
using System.Net.Sockets;      // Importa el espacio de nombres para manejar comunicaciones de red a través de sockets.
using System.Net;             // Importa el espacio de nombres para manejar direcciones IP y otros aspectos relacionados con la red.
using System.Threading;        // Importa el espacio de nombres para manejar la multitarea y la sincronización, como CancellationToken.
using System.Threading.Tasks;  // Importa el espacio de nombres para trabajar con tareas asíncronas.
using Server.Utils;           // Importa el espacio de nombres para utilizar métodos de utilidad relacionados con la base de datos y otras funciones.

namespace Server.Server        // Define un espacio de nombres para organizar las clases relacionadas con el servidor.
{
    public class Server        // Define una clase pública que gestiona el servidor y las conexiones de los clientes.
    {
        private TcpListener listener;                   // Instancia para escuchar las conexiones entrantes de clientes TCP.
        private List<TcpClient> connectedClients = new List<TcpClient>(); // Lista para almacenar los clientes conectados.
        private CancellationTokenSource cancellationTokenSource; // Fuente de token para cancelar operaciones asíncronas.

        public bool IsRunning { get; private set; } // Propiedad pública que indica si el servidor está en ejecución.

        public event Action<string> OnUserConnected; // Evento que se activa cuando un usuario se conecta, pasando el nombre del usuario.

        // Método público para iniciar el servidor.
        public void StartServer()
        {
            IsRunning = true; // Marca el servidor como en ejecución.
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1"); // Configura la dirección IP del servidor (localhost en este caso).
            int port = 15500; // Define el puerto en el que el servidor escuchará las conexiones.

            listener = new TcpListener(ipAddress, port); // Crea una instancia de TcpListener para aceptar conexiones en la IP y puerto especificados.
            listener.Start(); // Inicia el listener para aceptar conexiones entrantes.
            Console.WriteLine($"Servidor iniciado en {ipAddress}:{port}"); // Muestra un mensaje en la consola indicando que el servidor está en ejecución.

            // Verifica la conexión a la base de datos usando un método de utilidad.
            if (DatabaseUtils.TestDatabaseConnection())
            {
                Console.WriteLine("Conexion a la base de datos realizada con exito"); // Muestra un mensaje si la conexión a la base de datos es exitosa.
            }
            else
            {
                Console.WriteLine("Error al conectar a la base de datos"); // Muestra un mensaje si la conexión a la base de datos falla.
                StopServer(); // Detiene el servidor si no se puede conectar a la base de datos.
                return; // Sale del método StartServer para evitar seguir ejecutando el código.
            }

            // Crea un CancellationTokenSource para cancelar operaciones asíncronas.
            cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token; // Obtiene el token de cancelación asociado.

            // Inicia una tarea asíncrona para aceptar conexiones de clientes.
            Task.Run(() =>
            {
                while (IsRunning) // Ejecuta el bucle mientras el servidor esté en ejecución.
                {
                    try
                    {
                        TcpClient client = listener.AcceptTcpClient(); // Acepta una nueva conexión de cliente.
                        lock (connectedClients) // Bloquea la lista de clientes conectados para evitar problemas de concurrencia.
                        {
                            connectedClients.Add(client); // Agrega el cliente a la lista de clientes conectados.
                        }
                        // Inicia una tarea para manejar al cliente, pasando el token de cancelación y el evento OnUserConnected.
                        Task.Run(() => ClientHandler.HandleClient(client, connectedClients, cancellationToken, OnUserConnected), cancellationToken);
                    }
                    catch (SocketException ex) // Captura excepciones relacionadas con sockets.
                    {
                        if (IsRunning) // Solo muestra el mensaje si el servidor sigue en ejecución.
                        {
                            Console.WriteLine($"Error: {ex.Message}"); // Muestra el error en la consola.
                        }
                    }
                }
            }, cancellationToken); // Ejecuta la tarea con el token de cancelación.
        }

        // Método para verificar si hay clientes conectados antes de detener el servidor.
        public void CheckConnectedClientsBeforeStopping()
        {
            lock (connectedClients) // Bloquea la lista de clientes conectados para evitar problemas de concurrencia.
            {
                if (connectedClients.Count > 0) // Si hay clientes conectados.
                {
                    throw new InvalidOperationException($"Hay {connectedClients.Count} usuarios en la aplicación."); // Lanza una excepción si hay clientes conectados.
                }
                else
                {
                    StopServer(); // Detiene el servidor si no hay clientes conectados.
                }
            }
        }

        // Método para cerrar todas las conexiones de cliente.
        public void CloseAllClientConnections()
        {
            lock (connectedClients) // Bloquea la lista de clientes conectados para evitar problemas de concurrencia.
            {
                foreach (var client in connectedClients.ToArray()) // Recorre una copia de la lista de clientes conectados.
                {
                    try
                    {
                        client.GetStream().Close(); // Cierra el stream del cliente.
                        client.Close(); // Cierra la conexión del cliente.
                    }
                    catch (Exception ex) // Captura cualquier excepción al cerrar los clientes.
                    {
                        Console.WriteLine($"Error al cerrar cliente: {ex.Message}"); // Muestra el error en la consola.
                    }
                }
                connectedClients.Clear(); // Limpia la lista de clientes conectados.
            }
        }

        // Método para detener el servidor.
        public void StopServer()
        {
            IsRunning = false; // Marca el servidor como no en ejecución.
            cancellationTokenSource?.Cancel(); // Cancela las operaciones asíncronas.
            CloseAllClientConnections(); // Cierra todas las conexiones de cliente.
            listener.Stop(); // Detiene el listener.
        }
    }
}

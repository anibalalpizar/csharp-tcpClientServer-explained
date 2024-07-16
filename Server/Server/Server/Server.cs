using Server.Utils; // Importa utilidades del servidor, por ejemplo, para la conexión a la base de datos.
using System; // Importa funcionalidades básicas del sistema.
using System.Collections.Generic; // Importa funcionalidades para trabajar con colecciones genéricas.
using System.Net; // Importa funcionalidades para trabajar con redes.
using System.Net.Sockets; // Importa funcionalidades para trabajar con sockets de red.
using System.Threading; // Importa funcionalidades para trabajar con hilos y cancelación de tareas.
using System.Threading.Tasks; // Importa funcionalidades para trabajar con tareas asíncronas.

namespace Server.Server // Define el espacio de nombres 'Server.Server'.
{
    // Define la clase 'Server' que maneja el servidor TCP.
    public class Server
    {
        private TcpListener listener; // Declara un listener TCP para escuchar conexiones entrantes.
        private List<TcpClient> connectedClients = new List<TcpClient>(); // Lista de clientes TCP conectados.
        private CancellationTokenSource cancellationTokenSource; // Fuente del token de cancelación para manejar la cancelación de tareas.
        private ClientHandler clientHandler; // Manejador de clientes para gestionar las conexiones.

        public bool IsRunning { get; private set; } // Propiedad para verificar si el servidor está en ejecución.

        public event Action<string> OnUserConnected; // Evento para registrar cuando un usuario se conecta.
        public event Action<string> OnUserAction; // Nuevo evento para registrar acciones del usuario.

        // Constructor que inicializa el manejador de clientes.
        public Server()
        {
            clientHandler = new ClientHandler();
        }

        // Método para iniciar el servidor.
        public void StartServer()
        {
            IsRunning = true;
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1"); // Dirección IP en la que se ejecuta el servidor.
            int port = 15500; // Puerto en el que se ejecuta el servidor.

            listener = new TcpListener(ipAddress, port); // Inicializa el listener TCP.
            listener.Start(); // Inicia el listener.
            Console.WriteLine($"Servidor iniciado en {ipAddress}:{port}");

            // Verifica la conexión a la base de datos usando un método de utilidad.
            if (DatabaseUtils.TestDatabaseConnection())
            {
                Console.WriteLine("Conexion a la base de datos realizada con exito");
            }
            else
            {
                Console.WriteLine("Error al conectar a la base de datos");
                StopServer(); // Detiene el servidor si no se puede conectar a la base de datos.
                return;
            }

            cancellationTokenSource = new CancellationTokenSource(); // Inicializa la fuente del token de cancelación.
            var cancellationToken = cancellationTokenSource.Token; // Obtiene el token de cancelación.

            // Ejecuta una tarea para aceptar conexiones de clientes de forma asíncrona.
            Task.Run(() =>
            {
                while (IsRunning)
                {
                    try
                    {
                        TcpClient client = listener.AcceptTcpClient(); // Acepta una conexión de cliente.
                        lock (connectedClients) // Bloquea la lista de clientes conectados para realizar operaciones seguras para hilos.
                        {
                            connectedClients.Add(client); // Añade el cliente a la lista de clientes conectados.
                        }
                        // Maneja la conexión del cliente en una nueva tarea.
                        Task.Run(() => clientHandler.HandleClient(client, connectedClients, cancellationToken, OnUserConnected, OnUserAction), cancellationToken);
                    }
                    catch (SocketException ex) // Captura excepciones de socket.
                    {
                        if (IsRunning)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }
            }, cancellationToken);
        }

        // Método para verificar si hay clientes conectados antes de detener el servidor.
        public void CheckConnectedClientsBeforeStopping()
        {
            lock (connectedClients)
            {
                if (connectedClients.Count > 0)
                {
                    throw new InvalidOperationException($"Hay {connectedClients.Count} usuarios en la aplicación."); // Lanza una excepción si hay clientes conectados.
                }
                else
                {
                    StopServer(); // Detiene el servidor si no hay clientes conectados.
                }
            }
        }

        // Método para cerrar todas las conexiones de clientes.
        public void CloseAllClientConnections()
        {
            lock (connectedClients)
            {
                foreach (var client in connectedClients.ToArray()) // Itera sobre una copia de la lista de clientes conectados.
                {
                    try
                    {
                        client.GetStream().Close(); // Cierra el flujo de red del cliente.
                        client.Close(); // Cierra la conexión del cliente.
                    }
                    catch (Exception ex) // Captura cualquier excepción al cerrar el cliente.
                    {
                        Console.WriteLine($"Error al cerrar cliente: {ex.Message}");
                    }
                }
                connectedClients.Clear(); // Limpia la lista de clientes conectados.
            }
        }

        // Método para detener el servidor.
        public void StopServer()
        {
            IsRunning = false; // Establece que el servidor no está en ejecución.
            cancellationTokenSource?.Cancel(); // Cancela todas las tareas asíncronas.
            CloseAllClientConnections(); // Cierra todas las conexiones de clientes.
            listener.Stop(); // Detiene el listener TCP.
        }
    }
}

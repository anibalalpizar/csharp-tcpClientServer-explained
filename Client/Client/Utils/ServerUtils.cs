using System;                    // Importa el espacio de nombres para tipos básicos, como `Exception`.
using System.Net.Sockets;        // Importa el espacio de nombres para operaciones de red, como `TcpClient` y `NetworkStream`.
using System.Windows.Forms;      // Importa el espacio de nombres para componentes de Windows Forms, como `Button` y `MessageBox`.

namespace Client.Utils            // Define un espacio de nombres para utilidades del cliente.
{
    public class ServerUtils     // Define una clase pública para manejar la conexión con el servidor.
    {
        private TcpClient _client;         // Instancia de `TcpClient` para gestionar la conexión TCP.
        private NetworkStream _stream;     // Instancia de `NetworkStream` para leer y escribir datos de la red.
        private bool _isConnected;         // Estado de la conexión con el servidor.
        private Button _btnConnection;     // Referencia al botón de conexión en la interfaz de usuario.

        // Constructor de la clase `ServerUtils` que acepta un botón de conexión.
        public ServerUtils(Button btnConnection)
        {
            _btnConnection = btnConnection; // Inicializa la variable de instancia con el botón proporcionado.
        }

        // Método para conectar al servidor.
        public void ConnectToServer(string serverIp, int port)
        {
            try
            {
                // Intenta crear una conexión TCP con el servidor.
                _client = new TcpClient(serverIp, port);
                _stream = _client.GetStream(); // Obtiene el flujo de red para la conexión.
                _isConnected = true; // Marca la conexión como activa.

                // Cambia el texto del botón de conexión a "Desconectar".
                _btnConnection.Text = "Desconectar";
                MessageBox.Show("Conectado al servidor."); // Muestra un mensaje de éxito al conectar.

                // Configura la opción de mantener la conexión activa.
                _client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            }
            catch (SocketException ex) // Captura excepciones relacionadas con el socket.
            {
                MessageBox.Show("No se pudo conectar al servidor. Asegúrate de que el servidor esté en funcionamiento y vuelve a intentarlo.");
            }
            catch (Exception ex) // Captura cualquier otra excepción.
            {
                MessageBox.Show($"Error al conectar: {ex.Message}"); // Muestra un mensaje de error.
            }
        }

        // Método para desconectar del servidor.
        public void DisconnectFromServer()
        {
            try
            {
                // Intenta cerrar el flujo y la conexión TCP.
                if (_stream != null)
                {
                    _stream.Close();
                }
                if (_client != null)
                {
                    _client.Close();
                }
                _isConnected = false; // Marca la conexión como inactiva.

                // Cambia el texto del botón de conexión a "Conectar".
                _btnConnection.Text = "Conectar";
                MessageBox.Show("Desconectado del servidor."); // Muestra un mensaje de éxito al desconectar.
            }
            catch (Exception ex) // Captura cualquier excepción que ocurra al desconectar.
            {
                MessageBox.Show($"Error al desconectar: {ex.Message}"); // Muestra un mensaje de error.
            }
        }

        // Propiedad para verificar si el cliente está conectado.
        public bool IsConnected => _isConnected;

        // Propiedad para obtener el flujo de red, lanzando una excepción si no está conectado.
        public NetworkStream Stream
        {
            get
            {
                if (!_isConnected) // Verifica el estado de la conexión.
                {
                    throw new InvalidOperationException("No está conectado al servidor."); // Lanza una excepción si no está conectado.
                }
                return _stream; // Retorna el flujo de red si está conectado.
            }
        }
    }
}

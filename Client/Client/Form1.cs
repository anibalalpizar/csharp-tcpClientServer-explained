using Client.UI;                // Importa el espacio de nombres para la interfaz de usuario del cliente.
using Client.Utils;             // Importa el espacio de nombres para utilidades del cliente, como `ServerUtils`.
using System;                   // Importa el espacio de nombres para tipos básicos, como `Exception`.
using System.Text;             // Importa el espacio de nombres para manipulación de texto, como `Encoding`.
using System.Windows.Forms;     // Importa el espacio de nombres para componentes de Windows Forms, como `Form`, `Button`, y `MessageBox`.
using System.IO;                // Importa el espacio de nombres para operaciones de entrada/salida, como `IOException`.
using System.Net.Sockets;       // Importa el espacio de nombres para operaciones de red, como `TcpClient` y `SocketException`.

namespace Client
{
    public partial class Form1 : Form
    {
        private ServerUtils _serverUtils; // Instancia de `ServerUtils` para gestionar la conexión con el servidor.

        public Form1()
        {
            InitializeComponent(); // Inicializa los componentes de la interfaz de usuario.
            _serverUtils = new ServerUtils(btnConnection); // Inicializa `ServerUtils` con el botón de conexión.
        }

        private void btnConnection_Click(object sender, EventArgs e)
        {
            if (!_serverUtils.IsConnected) // Verifica si no está conectado.
            {
                _serverUtils.ConnectToServer("127.0.0.1", 15500); // Conecta al servidor con la IP y el puerto especificados.
            }
            else
            {
                _serverUtils.DisconnectFromServer(); // Desconecta del servidor si ya está conectado.
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (_serverUtils.IsConnected) // Verifica si está conectado.
            {
                try
                {
                    string message = txtId.Text; // Obtiene el texto del cuadro de texto `txtId`.
                    byte[] buffer = Encoding.ASCII.GetBytes(message); // Codifica el mensaje en bytes usando ASCII.
                    _serverUtils.Stream.Write(buffer, 0, buffer.Length); // Envía el mensaje al servidor.

                    byte[] responseBuffer = new byte[1024]; // Buffer para recibir la respuesta del servidor.
                    int bytesRead = _serverUtils.Stream.Read(responseBuffer, 0, responseBuffer.Length); // Lee la respuesta del servidor.
                    string response = Encoding.ASCII.GetString(responseBuffer, 0, bytesRead).Trim(); // Decodifica la respuesta y elimina espacios en blanco.

                    if (!string.IsNullOrEmpty(response)) // Verifica si la respuesta no está vacía.
                    {
                        MessageBox.Show("Inicio de sesión exitoso."); // Muestra un mensaje de éxito.
                        frmPrincipal mainForm = new frmPrincipal(response, message); // Crea una instancia del formulario principal con la respuesta del servidor.
                        this.Hide(); // Oculta el formulario actual.
                        mainForm.ShowDialog(); // Muestra el formulario principal como un diálogo.
                        this.Close(); // Cierra el formulario actual.
                    }
                    else
                    {
                        MessageBox.Show("Inicio de sesión fallido. Intente de nuevo. Por seguridad se desconectará del servidor"); // Muestra un mensaje de error.
                        _serverUtils.DisconnectFromServer(); // Desconecta del servidor.
                    }
                }
                catch (IOException ioEx) // Captura excepciones relacionadas con la entrada/salida.
                {
                    MessageBox.Show("Error de conexión: " + ioEx.Message); // Muestra un mensaje de error.
                    _serverUtils.DisconnectFromServer(); // Desconecta del servidor.
                }
                catch (SocketException socketEx) // Captura excepciones relacionadas con el socket.
                {
                    MessageBox.Show("Error de conexión: " + socketEx.Message); // Muestra un mensaje de error.
                    _serverUtils.DisconnectFromServer(); // Desconecta del servidor.
                }
            }
            else
            {
                MessageBox.Show("No estás conectado al servidor."); // Muestra un mensaje si no está conectado al servidor.
            }
        }
    }
}

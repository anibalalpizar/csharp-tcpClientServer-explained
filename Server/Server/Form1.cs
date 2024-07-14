using System;                 // Importa el espacio de nombres para los tipos básicos de .NET.
using System.Drawing;         // Importa el espacio de nombres para tipos relacionados con gráficos y colores.
using System.Windows.Forms;   // Importa el espacio de nombres para la creación de interfaces gráficas de usuario (GUI) en Windows Forms.

namespace Server              // Define un espacio de nombres para organizar las clases relacionadas con el servidor.
{
    public partial class Form1 : Form // Define una clase pública que hereda de Form, representando un formulario en Windows Forms.
    {
        private Server.Server server; // Instancia privada de la clase Server, que gestiona el servidor.

        // Constructor del formulario
        public Form1()
        {
            InitializeComponent(); // Inicializa los componentes del formulario (generado automáticamente).
            server = new Server.Server(); // Crea una nueva instancia del servidor.
            server.OnUserConnected += Server_OnUserConnected; // Suscribe al evento OnUserConnected del servidor.
            btnDetener.Enabled = false; // Desactiva el botón de detener al inicio.
            UpdateServerStatus(false); // Actualiza el estado del servidor a "Apagado".
        }

        // Método que se llama cuando un usuario se conecta al servidor.
        private void Server_OnUserConnected(string userName)
        {
            if (InvokeRequired) // Verifica si se necesita invocar en el hilo del formulario.
            {
                Invoke(new Action<string>(Server_OnUserConnected), userName); // Invoca el método en el hilo del formulario.
            }
            else
            {
                listBoxConexiones.Items.Add(userName); // Añade el nombre del usuario conectado a la lista.
            }
        }

        // Evento manejador para el clic en el botón "Ejecutar".
        private void btnEjecutar_Click(object sender, EventArgs e)
        {
            server.StartServer(); // Inicia el servidor.
            MessageBox.Show("Servidor iniciado."); // Muestra un mensaje de confirmación.
            btnEjecutar.Enabled = false; // Desactiva el botón de iniciar.
            btnDetener.Enabled = true; // Activa el botón de detener.
            UpdateServerStatus(true); // Actualiza el estado del servidor a "Prendido".
        }

        // Evento manejador para el clic en el botón "Detener".
        private void btnDetener_Click(object sender, EventArgs e)
        {
            try
            {
                server.CheckConnectedClientsBeforeStopping(); // Verifica si hay clientes conectados antes de detener el servidor.
            }
            catch (InvalidOperationException ex) // Captura excepciones si hay clientes conectados.
            {
                // Muestra un cuadro de diálogo preguntando si desea cerrar las conexiones.
                DialogResult result = MessageBox.Show(ex.Message + "\n¿Deseas cerrar las conexiones?", "Usuarios Conectados", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes) // Si el usuario selecciona "Sí".
                {
                    server.CloseAllClientConnections(); // Cierra todas las conexiones de cliente.
                    server.StopServer(); // Detiene el servidor.
                }
                else // Si el usuario selecciona "No".
                {
                    MessageBox.Show("Servidor no detenido."); // Muestra un mensaje informando que el servidor no se detuvo.
                }
            }
            finally
            {
                MessageBox.Show("Servidor detenido."); // Muestra un mensaje de confirmación de que el servidor se ha detenido.
                btnEjecutar.Enabled = true; // Activa el botón de iniciar.
                btnDetener.Enabled = false; // Desactiva el botón de detener.
                UpdateServerStatus(false); // Actualiza el estado del servidor a "Apagado".
            }
        }

        // Método para actualizar el estado del servidor en la interfaz gráfica.
        private void UpdateServerStatus(bool isRunning)
        {
            if (isRunning)
            {
                lblEstado.Text = "Servidor Prendido 127.0.0.1:15500"; // Muestra que el servidor está encendido.
                lblEstado.ForeColor = Color.Green; // Cambia el color del texto a verde.
            }
            else
            {
                lblEstado.Text = "Servidor Apagado"; // Muestra que el servidor está apagado.
                lblEstado.ForeColor = Color.Red; // Cambia el color del texto a rojo.
            }
        }
    }
}

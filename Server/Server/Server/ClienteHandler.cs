using Newtonsoft.Json;
using Server.Layers.BLL;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Server.Server
{
    public class ClienteHandler
    {

        // Crea una instancia de la capa de lógica de negocio para 'Cliente'
        private ClienteBLL clienteBLL = new ClienteBLL();

        // Método para manejar las solicitudes relacionadas con 'Cliente'
        public void HandlerCliente(TcpClient client, string request, Action<string> onUserAction)
        {
            try
            {
                // Verifica si la solicitud es para crear un nuevo cliente
                if (request.Contains("Creando"))
                {
                    // Deserializa la solicitud JSON a un objeto 'Cliente'
                    Cliente cliente = JsonConvert.DeserializeObject<Cliente>(request);
                    // Llama al método de la capa de lógica de negocio para registrar el cliente
                    string resultMessage = clienteBLL.RegistrarCliente(cliente);

                    // Obtiene el stream de red del cliente
                    using (NetworkStream stream = client.GetStream())
                    {
                        // Convierte el mensaje de resultado a bytes
                        byte[] responseBytes = Encoding.UTF8.GetBytes(resultMessage);
                        // Envía el mensaje de respuesta al cliente
                        stream.Write(responseBytes, 0, responseBytes.Length);

                        // Invoca la acción de usuario (si está definida) para registrar la creación
                        onUserAction?.Invoke("Un usuario ha creado o intentado crear un Cliente");
                    }
                }
                else if (request.Contains("Obteniendo"))
                {
                    // Llama al método de la capa de lógica de negocio para obtener la lista de clientes
                    List<Cliente> clientes = clienteBLL.ObtenerClientes();
                    // Serializa la lista de clientes a JSON
                    string resultMessage = JsonConvert.SerializeObject(clientes);

                    // Obtiene el stream de red del cliente
                    using (NetworkStream stream = client.GetStream())
                    {
                        // Convierte el mensaje de resultado a bytes
                        byte[] responseBytes = Encoding.UTF8.GetBytes(resultMessage);
                        // Envía el mensaje de respuesta al cliente
                        stream.Write(responseBytes, 0, responseBytes.Length);

                        // Invoca la acción de usuario (si está definida) para registrar la obtención
                        onUserAction?.Invoke("Un usuario ha obtenido clientes");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado al manejar el cliente: {ex.Message}");
            }
        }
    }
}

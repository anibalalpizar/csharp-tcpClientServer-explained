using Newtonsoft.Json;
using Server.Layers.BLL;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Server.Server
{
    // Define la clase 'SucursalHandler' que maneja las solicitudes relacionadas con sucursales
    public class SucursalHandler
    {
        private SucursalBLL sucursalBLL = new SucursalBLL(); // Instancia de 'SucursalBLL' para manejar la lógica de negocio de sucursales

        public void HandlerSucursal(TcpClient client, string request, Action<string> onUserAction)
        {
            try
            {
                if (request.Contains("Creando")) // Verifica si la solicitud es para crear una sucursal
                {
                    Sucursal sucursal = JsonConvert.DeserializeObject<Sucursal>(request); // Deserializa la solicitud JSON en un objeto 'Sucursal'
                    string resultMessage = sucursalBLL.RegistrarSucursal(sucursal); // Llama al método para registrar la sucursal

                    using (NetworkStream stream = client.GetStream()) // Obtiene el flujo de red del cliente
                    {
                        byte[] responseBytes = Encoding.UTF8.GetBytes(resultMessage); // Convierte el mensaje de resultado en bytes
                        stream.Write(responseBytes, 0, responseBytes.Length); // Escribe la respuesta en el flujo de red

                        onUserAction?.Invoke("Un usuario ha creado o intentado crear una sucursal."); // Invoca la acción 'onUserAction' si está definida
                    }
                }
                else if (request.Contains("Obteniendo")) // Verifica si la solicitud es para obtener todas las sucursales
                {
                    List<object> sucursales = sucursalBLL.ObtenerTodasSucursales(); // Llama al método para obtener todas las sucursales

                    string resultMessage = JsonConvert.SerializeObject(sucursales); // Serializa la lista de sucursales a JSON

                    using (NetworkStream stream = client.GetStream()) // Obtiene el flujo de red del cliente
                    {
                        byte[] responseBytes = Encoding.UTF8.GetBytes(resultMessage); // Convierte el mensaje de resultado en bytes
                        stream.Write(responseBytes, 0, responseBytes.Length); // Escribe la respuesta en el flujo de red

                        onUserAction?.Invoke("Un usuario ha obtenido las sucursales."); // Invoca la acción 'onUserAction' si está definida
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
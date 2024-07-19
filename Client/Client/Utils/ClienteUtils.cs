using Client.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Client.Utils
{
    // Define la clase 'ClienteUtils' para utilidades relacionadas con 'Cliente'
    public class ClienteUtils
    {
        // Método para registrar un nuevo cliente
        public string RegistrarCliente(int idCliente, string identificacion, string nombre, string apellido1, string apellido2, DateTime fechaNacimiento, DateTime fechaIngreso, bool activo)
        {
            // Crea una nueva instancia de 'Cliente' con los datos proporcionados
            Cliente cliente = new Cliente
            {
                IdCliente = idCliente,
                Identificacion = identificacion,
                Nombre = nombre,
                Apellido1 = apellido1,
                Apellido2 = apellido2,
                FechaNacimiento = fechaNacimiento,
                FechaIngreso = fechaIngreso,
                Activo = activo,
                Accion = "Creando"
            };

            // Serializa el objeto 'Cliente' a JSON
            string jsonData = JsonConvert.SerializeObject(cliente);

            // Convierte el JSON a un objeto JObject para manipulación adicional
            JObject jsonObject = JObject.Parse(jsonData);
            jsonObject["FechaNacimiento"] = fechaNacimiento;
            jsonObject["FechaIngreso"] = fechaIngreso;

            // Convierte el JObject modificado de vuelta a una cadena JSON
            string modifiedJsonData = jsonObject.ToString();
            // Codifica la cadena JSON a bytes
            byte[] data = Encoding.UTF8.GetBytes(modifiedJsonData);

            try
            {
                // Establece una conexión TCP con el servidor
                using (TcpClient client = new TcpClient("127.0.0.1", 15500))
                {
                    NetworkStream stream = client.GetStream();
                    // Envía los datos al servidor
                    stream.Write(data, 0, data.Length);

                    // Lee la respuesta del servidor
                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    // Devuelve la respuesta del servidor
                    return response;
                }
            }
            catch (Exception ex)
            {
                // Devuelve un mensaje de error en caso de excepción
                return $"Error al registrar el cliente: {ex.Message}";
            }
        }

        // Método para obtener todos los clientes
        public List<Cliente> ObtenerTodos()
        {
            Cliente cliente = new Cliente
            {
                Accion = "Obteniendo"
            };

            // Serializa el objeto de solicitud a JSON
            string jsonData = JsonConvert.SerializeObject(cliente);
            // Codifica la cadena JSON a bytes
            byte[] data = Encoding.UTF8.GetBytes(jsonData);

            try
            {
                // Establece una conexión TCP con el servidor
                using (TcpClient client = new TcpClient("127.0.0.1", 15500))
                {
                    NetworkStream stream = client.GetStream();
                    // Envía los datos al servidor
                    stream.Write(data, 0, data.Length);

                    // Lee la respuesta del servidor
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                    {
                        byte[] buffer = new byte[1024];
                        int bytesRead;
                        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, bytesRead);
                        }

                        string response = Encoding.UTF8.GetString(ms.ToArray());

                        // Convierte la respuesta JSON a una lista de objetos 'Cliente'
                        List<Cliente> clientes = JsonConvert.DeserializeObject<List<Cliente>>(response);

                        // Devuelve la lista de clientes
                        return clientes;
                    }
                }
            }
            catch (Exception ex)
            {
                // Devuelve un mensaje de error en caso de excepción
                Console.WriteLine($"Error al obtener los clientes: {ex.Message}");
                return null;
            }
        }
    }
}
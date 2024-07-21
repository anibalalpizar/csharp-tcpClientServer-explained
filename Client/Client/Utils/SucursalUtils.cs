using Client.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Client.Utils
{
    public class SucursalUtils
    {
        // Metodo para registrar una sucursal
        public string RegistrarSucursal(int idSucursal, string nombreSucursal, string direccion, string telefono, int idEncargado, bool activo)
        {
            // Crea un objeto de tipo Sucursal con los datos proporcionados
            Sucursal sucursal = new Sucursal
            {
                IdSucursal = idSucursal,
                Nombre = nombreSucursal,
                Direccion = direccion,
                Telefono = telefono,
                Encargado = new Encargado { IdEncargado = idEncargado },
                Activo = activo,
                Accion = "Creando" // Indica que se va a crear una sucursal
            };

            // Convierte el objeto de tipo Sucursal a un objeto JSON
            string jsonData = JsonConvert.SerializeObject(sucursal);
            // Convierte el objeto JSON a un objeto JObject
            JObject jsonObject = JObject.Parse(jsonData);
            // Agrega el ID del encargado al objeto JSON
            jsonObject["Encargado"] = new JObject { ["IdEncargado"] = idEncargado };
            string modifiedJsonData = jsonObject.ToString();
            // Convierte el objeto JSON modificado a un arreglo de bytes
            byte[] data = Encoding.UTF8.GetBytes(modifiedJsonData);

            try
            {
                // Crea una conexión con el servidor
                using (TcpClient client = new TcpClient("127.0.0.1", 15500))
                {
                    // Obtiene el flujo de datos de la conexión
                    NetworkStream stream = client.GetStream();
                    // Escribe los datos en el flujo de datos
                    stream.Write(data, 0, data.Length);
                    // Crea un arreglo de bytes para almacenar la respuesta del servidor
                    byte[] buffer = new byte[1024];
                    // Lee los datos del flujo de datos y los almacena en el arreglo de bytes
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    // Convierte los datos del arreglo de bytes a una cadena
                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    // Retorna la respuesta del servidor
                    return response;
                }
            }
            catch (Exception ex) // Captura cualquier excepción que ocurra
            {
                // Retorna un mensaje de error
                return $"Error al registrar la sucursal: {ex.Message}";
            }
        }

        // Metodo para obtener todas las sucursales
        public List<object> ObtenerTodos()
        {
            // Crea un objeto de tipo Sucursal con la acción de obtener
            Sucursal solicitud = new Sucursal
            {
                Accion = "Obteniendo" // Indica que se van a obtener las sucursales
            };

            // Convierte el objeto de tipo Sucursal a un objeto JSON
            string jsonData = JsonConvert.SerializeObject(solicitud);
            // Convierte el objeto JSON a un arreglo de bytes
            byte[] data = Encoding.UTF8.GetBytes(jsonData);

            try
            {
                // Crea una conexión con el servidor
                using (TcpClient client = new TcpClient("127.0.0.1", 15500))
                {
                    // Obtiene el flujo de datos de la conexión
                    NetworkStream stream = client.GetStream();
                    // Escribe los datos en el flujo de datos
                    stream.Write(data, 0, data.Length);
                    // Crea un arreglo de bytes para almacenar la respuesta del servidor
                    byte[] buffer = new byte[4096];
                    // Lee los datos del flujo de datos y los almacena en el arreglo de bytes
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    // Convierte los datos del arreglo de bytes a una cadena
                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    // Convierte la cadena a una lista de objetos
                    List<object> sucursales = JsonConvert.DeserializeObject<List<object>>(response);
                    // Retorna la lista de sucursales
                    return sucursales;
                }
            }
            catch (Exception ex) // Captura cualquier excepción que ocurra
            {
                Console.WriteLine($"Error al obtener las sucursales: {ex.Message}");
                return null;
            }
        }

        // Metodo para obtener una sucursal por su ID
        public object ObtenerPorId(int idSucursal)
        {
            // Crea un objeto de tipo Sucursal con el ID proporcionado
            Sucursal solicitud = new Sucursal
            {
                IdSucursal = idSucursal,
                Accion = "ID" // Indica que se va a obtener una sucursal por su ID
            };

            // Convierte el objeto de tipo Sucursal a un objeto JSON
            string jsonData = JsonConvert.SerializeObject(solicitud);
            // Convierte el objeto JSON a un arreglo de bytes
            byte[] data = Encoding.UTF8.GetBytes(jsonData);

            try
            {
                // Crea una conexión con el servidor
                using (TcpClient client = new TcpClient("127.0.0.1", 15500))
                {
                    // Obtiene el flujo de datos de la conexión
                    NetworkStream stream = client.GetStream();
                    // Escribe los datos en el flujo de datos
                    stream.Write(data, 0, data.Length);
                    // Crea un arreglo de bytes para almacenar la respuesta del servidor
                    byte[] buffer = new byte[4096];
                    // Lee los datos del flujo de datos y los almacena en el arreglo de bytes
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    // Convierte los datos del arreglo de bytes a una cadena
                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    // Convierte la cadena a un objeto
                    object sucursal = JsonConvert.DeserializeObject<object>(response);
                    // Retorna la sucursal
                    return sucursal;
                }
            }
            catch (Exception ex) // Captura cualquier excepción que ocurra
            {
                Console.WriteLine($"Error al obtener la sucursal: {ex.Message}");
                return null;
            }
        }
    }
}

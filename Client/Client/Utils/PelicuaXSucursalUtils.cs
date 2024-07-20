using Client.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Client.Utils
{
    public class PelicuaXSucursalUtils
    {
        // Método para registrar una nueva relación entre una película y una sucursal
        public string RegistrarPelicuaXSucursal(int idSucursal, List<int> idsPeliculas, int cantidad)
        {
            // Creamos una nueva lista de 'Pelicula' usando los IDs proporcionados
            List<Pelicula> peliculas = idsPeliculas.ConvertAll(id => new Pelicula { IdPelicula = id });

            // Creamos una nueva instancia de 'PeliculaXSucursal' y la llenamos con los datos proporcionados
            PeliculaXSucursal peliculaXSucursal = new PeliculaXSucursal
            {
                IdSucursal = new Sucursal { IdSucursal = idSucursal },
                Peliculas = peliculas,
                Cantidad = cantidad,
                Accion = "Creando"
            };

            // Convertimos el objeto 'peliculaXSucursal' a una cadena JSON
            string jsonData = JsonConvert.SerializeObject(peliculaXSucursal);

            // Removemos los campos adicionales de 'Sucursal' en el JSON
            JObject jsonObject = JObject.Parse(jsonData);
            jsonObject["IdSucursal"] = new JObject { ["IdSucursal"] = idSucursal };

            // Removemos los campos adicionales de 'Peliculas' en el JSON
            JArray peliculasArray = (JArray)jsonObject["Peliculas"];
            for (int i = 0; i < peliculasArray.Count; i++)
            {
                peliculasArray[i] = new JObject { ["IdPelicula"] = idsPeliculas[i] };
            }


            // Convierte el JSON modificado a una cadena de bytes
            string modifiedJsonData = jsonObject.ToString();
            byte[] data = Encoding.UTF8.GetBytes(modifiedJsonData);

            try
            {
                // Establecemos una conexión TCP con el servidor en la dirección
                using (TcpClient client = new TcpClient("127.0.0.1", 15500))
                {
                    // Obtenemos el flujo de datos del cliente
                    NetworkStream stream = client.GetStream();
                    // Enviamos los datos al servidor
                    stream.Write(data, 0, data.Length);

                    // Creamos un buffer para leer la respuesta del servidor
                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    // Convertimos la respuesta del servidor de bytes a una cadena
                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    // Devolvemos la respuesta del servidor
                    return response;
                }
            }
            catch (Exception ex)
            {
                // Si ocurre un error, devolvemos un mensaje de error
                return $"Error al registrar la relación entre la película y la sucursal: {ex.Message}";
            }
        }

        // Método para obtener todas las relaciones entre películas y sucursales
        public List<object> ObtenerTodos()
        {
            // Creamos una nueva instancia de 'PeliculaXSucursal' con la acción de obtener
            PeliculaXSucursal solicitud = new PeliculaXSucursal
            {
                Accion = "Obteniendo"
            };

            // Convertimos el objeto 'solicitud' a una cadena JSON
            string jsonData = JsonConvert.SerializeObject(solicitud);
            // Convertimos el JSON a una cadena de bytes
            byte[] data = Encoding.UTF8.GetBytes(jsonData);

            try
            {
                // Establecemos una conexión TCP con el servidor en la dirección
                using (TcpClient client = new TcpClient("127.0.0.1", 15500))
                {
                    // Obtenemos el flujo de datos del cliente
                    NetworkStream stream = client.GetStream();
                    // Enviamos los datos al servidor
                    stream.Write(data, 0, data.Length);

                    // Creamos un buffer para leer la respuesta del servidor
                    byte[] buffer = new byte[4096];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    // Convertimos la respuesta del servidor de bytes a una cadena
                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    // Convertimos la respuesta a una lista de objetos
                    List<object> peliculasXSucursales = JsonConvert.DeserializeObject<List<object>>(response);

                    // Devolvemos la lista de relaciones entre películas y sucursales
                    return peliculasXSucursales;
                }

            }
            catch (Exception ex)
            {
                // Si ocurre un error, devolvemos un mensaje de error
                Console.WriteLine($"Error al obtener las relaciones entre películas y sucursales: {ex.Message}");
                return null;
            }
        }
    }
}

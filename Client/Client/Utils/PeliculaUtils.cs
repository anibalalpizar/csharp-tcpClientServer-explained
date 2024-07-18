using Client.Models; // Importa los modelos de datos del cliente
using Newtonsoft.Json; // Importa la biblioteca para la serialización y deserialización JSON
using Newtonsoft.Json.Linq; // Importa la biblioteca para manipular JSON de manera más flexible
using System; // Importa funcionalidades básicas del sistema
using System.Collections.Generic; // Importa funcionalidades para trabajar con colecciones genéricas
using System.Net.Sockets; // Importa funcionalidades para la comunicación en red
using System.Text; // Importa funcionalidades para el manejo de texto

namespace Client.Utils // Define el espacio de nombres 'Client.Utils'
{
    // Define la clase 'PeliculaUtils' que maneja operaciones relacionadas con películas
    public class PeliculaUtils
    {
        // Método para registrar una película en el servidor
        public string RegistrarPelicula(int idPelicula, string tituloPelicula, int anoLanzamiento, string idioma, int idCategoria)
        {
            // Crea un objeto 'Pelicula' con los datos proporcionados
            Pelicula pelicula = new Pelicula
            {
                IdPelicula = idPelicula,
                Titulo = tituloPelicula,
                AnoLanzamiento = anoLanzamiento,
                Idioma = idioma,
                CategoriaPelicula = new CategoriaPelicula { IdCategoria = idCategoria },
                Accion = "Creando" // Define la acción que se está realizando
            };

            // Serializa el objeto 'Pelicula' a JSON
            string jsonData = JsonConvert.SerializeObject(pelicula);

            // Remueve los campos adicionales de 'CategoriaPelicula' en el JSON
            JObject jsonObject = JObject.Parse(jsonData);
            jsonObject["CategoriaPelicula"] = new JObject { ["IdCategoria"] = idCategoria };

            // Convierte el JSON modificado a una cadena de bytes
            string modifiedJsonData = jsonObject.ToString();
            byte[] data = Encoding.UTF8.GetBytes(modifiedJsonData);

            try
            {
                // Crea una conexión TCP con el servidor
                using (TcpClient client = new TcpClient("127.0.0.1", 15500))
                {
                    NetworkStream stream = client.GetStream();
                    // Envía los datos al servidor
                    stream.Write(data, 0, data.Length);

                    // Lee la respuesta del servidor
                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    return response; // Devuelve la respuesta del servidor
                }
            }
            catch (Exception ex)
            {
                // Captura y devuelve un mensaje de error en caso de excepción
                return $"Error al registrar la película: {ex.Message}";
            }
        }

        // Método para obtener todas las películas del servidor
        public List<Pelicula> ObtenerTodos()
        {
            // Crea una solicitud para obtener las películas
            Pelicula solicitud = new Pelicula
            {
                Accion = "Obteniendo"
            };

            // Serializa la solicitud a JSON
            string jsonData = JsonConvert.SerializeObject(solicitud);
            byte[] data = Encoding.UTF8.GetBytes(jsonData);

            try
            {
                // Crea una conexión TCP con el servidor
                using (TcpClient client = new TcpClient("127.0.0.1", 15500))
                {
                    NetworkStream stream = client.GetStream();
                    // Envía la solicitud al servidor
                    stream.Write(data, 0, data.Length);

                    // Lee la respuesta del servidor
                    byte[] buffer = new byte[4096];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    // Deserializa la respuesta JSON a una lista de películas
                    List<Pelicula> peliculas = JsonConvert.DeserializeObject<List<Pelicula>>(response);

                    return peliculas; // Devuelve la lista de películas
                }
            }
            catch (Exception ex)
            {
                // Captura y muestra un mensaje de error en caso de excepción
                Console.WriteLine($"Error al obtener las películas: {ex.Message}");
                return null; // Devuelve null en caso de error
            }
        }
    }
}

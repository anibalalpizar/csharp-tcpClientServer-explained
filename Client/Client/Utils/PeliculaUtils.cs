using Client.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Client.Utils
{
    public class PeliculaUtils
    {
        public string RegistrarPelicula(int idPelicula, string tituloPelicula, int anoLanzamiento, string idioma, int idCategoria)
        {

            Pelicula pelicula = new Pelicula
            {
                IdPelicula = idPelicula,
                Titulo = tituloPelicula,
                AnoLanzamiento = anoLanzamiento,
                Idioma = idioma,
                CategoriaPelicula = new CategoriaPelicula { IdCategoria = idCategoria },
                Accion = "Creando"
            };

            string jsonData = JsonConvert.SerializeObject(pelicula);

            // Remover campos adicionales de CategoriaPelicula
            JObject jsonObject = JObject.Parse(jsonData);
            jsonObject["CategoriaPelicula"] = new JObject { ["IdCategoria"] = idCategoria };

            string modifiedJsonData = jsonObject.ToString();
            byte[] data = Encoding.UTF8.GetBytes(modifiedJsonData);

            try
            {
                using (TcpClient client = new TcpClient("127.0.0.1", 15500))
                {
                    NetworkStream stream = client.GetStream();
                    stream.Write(data, 0, data.Length);

                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    return response;
                }
            }
            catch (Exception ex)
            {
                return $"Error al registrar la película: {ex.Message}";
            }
        }

        public List<Pelicula> ObtenerTodos()
        {
            Pelicula solicitud = new Pelicula
            {
                Accion = "Obteniendo"
            };

            string jsonData = JsonConvert.SerializeObject(solicitud);
            byte[] data = Encoding.UTF8.GetBytes(jsonData);

            try
            {
                using (TcpClient client = new TcpClient("127.0.0.1", 15500))
                {
                    NetworkStream stream = client.GetStream();
                    stream.Write(data, 0, data.Length);
                    byte[] buffer = new byte[4096];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);

                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    List<Pelicula> peliculas = JsonConvert.DeserializeObject<List<Pelicula>>(response);

                    return peliculas;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtner las peliculas: {ex.Message}");
                return null;
            }
        }
    }
}

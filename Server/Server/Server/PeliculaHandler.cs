using Newtonsoft.Json;
using Server.Layers.BLL;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Server
{
    public class PeliculaHandler
    {
        private PeliculaBLL peliculaBLL = new PeliculaBLL();

        public void HandlerPelicula(TcpClient client, string request, Action<string> onUserAction)
        {
            try
            {
                if (request.Contains("Creando"))
                {
                    Pelicula pelicula = JsonConvert.DeserializeObject<Pelicula>(request);
                    string resultMessage = peliculaBLL.RegistrarPelicula(pelicula);

                    using (NetworkStream stream = client.GetStream())
                    {
                        byte[] responseBytes = Encoding.UTF8.GetBytes(resultMessage);
                        stream.Write(responseBytes, 0, responseBytes.Length);

                        onUserAction?.Invoke("Un usuario ha creado o intentado una película.");

                    }
                }
                else if (request.Contains("Obteniendo"))
                {
                    List<Pelicula> peliculas = peliculaBLL.ObteniendoPelicula();
                    string resultMessage = JsonConvert.SerializeObject(peliculas);

                    using (NetworkStream stream = client.GetStream())
                    {
                        byte[] responseBytes = Encoding.UTF8.GetBytes(resultMessage);
                        stream.Write(responseBytes, 0, responseBytes.Length);

                        onUserAction?.Invoke("Un usuario ha obtenido películas.");

                    }
                }
            } catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado al manejar el cliente: {ex.Message}");
            }
        }
    }
}

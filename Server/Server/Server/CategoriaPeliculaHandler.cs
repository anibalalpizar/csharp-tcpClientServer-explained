using Server.Layers.BLL;
using Server.Models;
using System;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Server.Server
{
    public class CategoriaPeliculaHandler
    {
        private CategoriaPeliculaBLL categoriaPeliculaBLL = new CategoriaPeliculaBLL();

        public void HandlerCategoriaPelicula(TcpClient client, string request, Action<string> onUserAction)
        {
            try
            {
                if (request.Contains("Creando"))
                {
                    CategoriaPelicula categoriaPelicula = JsonConvert.DeserializeObject<CategoriaPelicula>(request);
                    string resultMessage = categoriaPeliculaBLL.RegistrarCategoria(categoriaPelicula);

                    using (NetworkStream stream = client.GetStream())
                    {
                        byte[] responseBytes = Encoding.UTF8.GetBytes(resultMessage);
                        stream.Write(responseBytes, 0, responseBytes.Length);

                        onUserAction?.Invoke("Un usuario ha creado o intentado una categoría de película.");

                    }
                }
                else if (request.Contains("Obteniendo"))
                {
                    List<CategoriaPelicula> categorias = categoriaPeliculaBLL.ObteniendoCategoria();
                    string resultMessage = JsonConvert.SerializeObject(categorias);

                    using (NetworkStream stream = client.GetStream())
                    {
                        byte[] responseBytes = Encoding.UTF8.GetBytes(resultMessage);
                        stream.Write(responseBytes, 0, responseBytes.Length);

                        onUserAction?.Invoke("Un usuario ha obtenido categorías de películas.");

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

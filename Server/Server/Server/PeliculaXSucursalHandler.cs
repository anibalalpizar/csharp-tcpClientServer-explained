using Newtonsoft.Json;
using Server.Layers.BLL;
using Server.Models;
using System;
using System.Net.Sockets;
using System.Text;

namespace Server.Server
{
    public class PeliculaXSucursalHandler
    {
        private PeliculaXSucursalBLL peliculaXSucursalBLL = new PeliculaXSucursalBLL();

        public void HandlerPeliculaXSucursal(TcpClient client, string request, Action<string> onUserAction)
        {
            try
            {
                if (request.Contains("Creando"))
                {
                    PeliculaXSucursal peliculaXSucursal = JsonConvert.DeserializeObject<PeliculaXSucursal>(request);
                    string resultMessage = peliculaXSucursalBLL.RegistrarPeliculaXSucursal(peliculaXSucursal);

                    using (NetworkStream stream = client.GetStream())
                    {
                        byte[] responseBytes = Encoding.UTF8.GetBytes(resultMessage);
                        stream.Write(responseBytes, 0, responseBytes.Length);

                        onUserAction?.Invoke("Un usuario ha creado o intentado una relación entre película y sucursal.");

                    }
                }
                else if (request.Contains("Obteniendo"))
                {
                    string resultMessage = JsonConvert.SerializeObject(peliculaXSucursalBLL.ObtenerPeliculasPorSucursal());

                    using (NetworkStream stream = client.GetStream())
                    {
                        byte[] responseBytes = Encoding.UTF8.GetBytes(resultMessage);
                        stream.Write(responseBytes, 0, responseBytes.Length);

                        onUserAction?.Invoke("Un usuario ha solicitado obtener las relaciones entre películas y sucursales.");
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
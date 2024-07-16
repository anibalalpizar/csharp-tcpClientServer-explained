// Importamos los modelos necesarios desde el espacio de nombres Client.Models
using Client.Models;
// Importamos la biblioteca Newtonsoft.Json para manejar JSON
using Newtonsoft.Json;
// Importamos la biblioteca estándar de C# para el manejo de fechas y horas
using System;
// Importamos la biblioteca estándar de C# para manejar colecciones genéricas
using System.Collections.Generic;
// Importamos la biblioteca estándar de C# para el manejo de conexiones TCP
using System.Net.Sockets;
// Importamos la biblioteca estándar de C# para la manipulación de texto y conversiones
using System.Text;

// Definimos un espacio de nombres específico para utilidades
namespace Client.Utils
{
    // Definimos la clase 'CategoriaUtils' que contiene métodos para registrar y obtener categorías
    public class CategoriaUtils
    {
        // Método para registrar una nueva categoría
        public string RegistrarCategoria(int idCategoria, string nombreCategoria, string descripcion)
        {
            // Creamos una nueva instancia de 'CategoriaPelicula' y la llenamos con los datos proporcionados
            CategoriaPelicula categoria = new CategoriaPelicula
            {
                IdCategoria = idCategoria,
                NombreCategoria = nombreCategoria,
                Descripcion = descripcion,
                Accion = "Creando" // Especificamos que la acción es crear una nueva categoría
            };

            // Convertimos el objeto 'categoria' a una cadena JSON
            string jsonData = JsonConvert.SerializeObject(categoria);
            // Convertimos la cadena JSON a un arreglo de bytes usando codificación UTF-8
            byte[] data = Encoding.UTF8.GetBytes(jsonData);

            try
            {
                // Establecemos una conexión TCP con el servidor en la dirección 127.0.0.1 y el puerto 15500
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
                return $"Error al registrar la categoría: {ex.Message}";
            }
        }

        // Método para obtener todas las categorías
        public List<CategoriaPelicula> ObtenerTodos()
        {
            // Creamos una instancia de 'CategoriaPelicula' para solicitar las categorías, especificando la acción "Obteniendo"
            CategoriaPelicula solicitud = new CategoriaPelicula
            {
                Accion = "Obteniendo"
            };

            // Convertimos el objeto 'solicitud' a una cadena JSON
            string jsonData = JsonConvert.SerializeObject(solicitud);
            // Convertimos la cadena JSON a un arreglo de bytes usando codificación UTF-8
            byte[] data = Encoding.UTF8.GetBytes(jsonData);

            try
            {
                // Establecemos una conexión TCP con el servidor en la dirección 127.0.0.1 y el puerto 15500
                using (TcpClient client = new TcpClient("127.0.0.1", 15500))
                {
                    // Obtenemos el flujo de datos del cliente
                    NetworkStream stream = client.GetStream();
                    // Enviamos los datos al servidor
                    stream.Write(data, 0, data.Length);

                    // Creamos un buffer para leer la respuesta del servidor
                    byte[] buffer = new byte[4096]; // Usamos un buffer más grande para recibir más datos
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    // Convertimos la respuesta del servidor de bytes a una cadena
                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    // Convertimos la cadena JSON de respuesta a una lista de 'CategoriaPelicula'
                    List<CategoriaPelicula> categorias = JsonConvert.DeserializeObject<List<CategoriaPelicula>>(response);

                    // Devolvemos la lista de categorías
                    return categorias;
                }
            }
            catch (Exception ex)
            {
                // Si ocurre un error, lo mostramos en la consola y devolvemos null
                Console.WriteLine($"Error al obtener las categorías: {ex.Message}");
                return null;
            }
        }
    }
}

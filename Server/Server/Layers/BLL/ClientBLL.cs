using Server.Layers.DAL;  // Importa el espacio de nombres para la capa de acceso a datos (DAL), que contiene la lógica para acceder a la base de datos.
using System;             // Importa el espacio de nombres para los tipos básicos de .NET.

namespace Server.Layers.BLL // Define un espacio de nombres para la capa de la lógica de negocios (BLL), que contiene la lógica de negocio del servidor.
{
    public class ClientBLL // Define una clase pública que contiene la lógica de negocios para la autenticación de clientes.
    {
        // Método estático para autenticar un cliente.
        public static bool Authenticate(string identificacion)
        {
            try
            {
                // Llama al método ValidateCliente en la capa de acceso a datos (DAL) para validar la identificación del cliente.
                return ClientDAL.ValidateCliente(identificacion);
            }
            catch (Exception ex) // Captura cualquier excepción que ocurra durante el proceso de autenticación.
            {
                // Imprime el mensaje de la excepción en la consola.
                Console.WriteLine($"Error en la autenticación: {ex.Message}");
                // Imprime el detalle de la excepción (pila de llamadas) en la consola.
                Console.WriteLine($"Detalle del error: {ex.StackTrace}");
                // Retorna false si ocurre una excepción durante la autenticación.
                return false;
            }
        }
    }
}

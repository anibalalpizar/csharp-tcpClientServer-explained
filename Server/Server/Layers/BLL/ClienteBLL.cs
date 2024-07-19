using Server.Layers.DAL;
using Server.Models;
using System.Collections.Generic;

namespace Server.Layers.BLL
{
    public class ClienteBLL
    {
        private ClienteDAL clienteDAL = new ClienteDAL();

        public string RegistrarCliente(Cliente cliente)
        {
            return clienteDAL.InsertarCliente(cliente);
        }

        public List<Cliente> ObtenerClientes()
        {
            return clienteDAL.ObtenerTodosClientes();
        }
    }
}

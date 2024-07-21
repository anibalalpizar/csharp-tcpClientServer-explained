using Server.Layers.DAL;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Layers.BLL
{
    // Define la clase 'SucursalBLL' que maneja la lógica de negocio relacionada con sucursales
    public class SucursalBLL
    {
        private SucursalDAL sucursalDAL = new SucursalDAL(); // Instancia de 'SucursalDAL' para manejar el acceso a datos de sucursales

        // Método para registrar una sucursal
        public string RegistrarSucursal(Sucursal sucursal)
        {
            return sucursalDAL.InsertarSucursal(sucursal); // Llama al método para insertar la sucursal en la base de datos y retorna el resultado
        }

        // Método para obtener todas las sucursales
        public List<object> ObtenerTodasSucursales()
        {
            return sucursalDAL.ObtenerTodasSucursales(); // Llama al método para obtener todas las sucursales de la base de datos y retorna el resultado
        }

        // Método para obtener una sucursal por Id
        public List<object> ObtenerPeliculaPorSucursal(int idSucursal)
        {
            return sucursalDAL.ObtenerPeliculasPorSucursal(idSucursal); // Llama al método para obtener una sucursal por Id de la base de datos y retorna el resultado
        }
    }
}

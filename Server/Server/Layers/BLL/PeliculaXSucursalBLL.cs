using Server.Layers.DAL;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Layers.BLL
{
    public class PeliculaXSucursalBLL
    {
        private PeliculaXSucursalDAL _peliculaXSucursalDAL;

        public PeliculaXSucursalBLL()
        {
            _peliculaXSucursalDAL = new PeliculaXSucursalDAL();
        }

        public string RegistrarPeliculaXSucursal(PeliculaXSucursal request)
        {
            return _peliculaXSucursalDAL.InsertarPeliculaXSucursal(request);
        }

        public object ObtenerPeliculasPorSucursal()
        {
            return _peliculaXSucursalDAL.ObtenerTodasPeliculasXSucursales();
        }
    }
}

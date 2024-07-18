using Server.Layers.DAL;
using Server.Models;
using System.Collections.Generic;

namespace Server.Layers.BLL
{
    public class EncargadoBLL
    {
        // Crea una instancia de la capa de acceso a datos para 'Encargado'
        private EncargadoDAL encargadoDAL = new EncargadoDAL();

        // Método para registrar un encargado
        public string RegistrarEncargado(Encargado encargado)
        {
            // Lógica de negocio para registrar un encargado
            return encargadoDAL.InsertarEncargado(encargado);
        }

        // Método para obtener todos los encargados
        public List<Encargado> ObtenerEncargados()
        {
            // Lógica de negocio para obtener todos los encargados
            return encargadoDAL.ObtenerTodosEncargados();
        }
    }
}
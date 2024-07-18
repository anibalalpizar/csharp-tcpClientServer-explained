using System;

namespace Client.Models
{
    public class Encargado
    {
        public int IdEncargado { get; set; }
        public string Identificacion { get; set; }
        public string Nombre { get; set; }
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string Accion { get; set; }
    }
}

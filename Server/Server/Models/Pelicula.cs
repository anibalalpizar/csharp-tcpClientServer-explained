namespace Server.Models
{
    public class Pelicula
    {
        public int IdPelicula { get; set; }
        public string Titulo { get; set; }
        public CategoriaPelicula CategoriaPelicula { get; set; }
        public int AnoLanzamiento { get; set; }
        public string Idioma { get; set; }
    }
}

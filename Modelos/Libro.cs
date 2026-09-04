using BibliotecaSystem.Enums;

namespace BibliotecaSystem.Modelos
{
    /// <summary>
    /// Representa la obra (título) en sí, independiente de cuántos
    /// ejemplares físicos (copias) existan de ella en la biblioteca.
    /// </summary>
    public class Libro
    {
        public string Nombre { get; set; }
        public TipoLibro Tipo { get; set; }
        public string Editorial { get; set; }
        public int Anio { get; set; }
        public Autor Autor { get; set; }

        public Libro(string nombre, TipoLibro tipo, string editorial, int anio, Autor autor)
        {
            Nombre = nombre;
            Tipo = tipo;
            Editorial = editorial;
            Anio = anio;
            Autor = autor;
        }

        public override string ToString() => $"{Nombre} ({Anio}) - {Autor.Nombre}";
    }
}

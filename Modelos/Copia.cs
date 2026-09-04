using BibliotecaSystem.Enums;

namespace BibliotecaSystem.Modelos
{
    /// <summary>
    /// Ejemplar físico de un libro. Un mismo Libro puede tener varias Copias,
    /// cada una con su propio identificador y su propio estado.
    /// </summary>
    public class Copia
    {
        public string Id { get; set; }
        public Libro Libro { get; set; }
        public EstadoCopia Estado { get; set; }

        public Copia(string id, Libro libro)
        {
            Id = id;
            Libro = libro;
            Estado = EstadoCopia.EnBiblioteca;
        }

        public override string ToString() => $"Copia {Id} de '{Libro.Nombre}' [{Estado}]";
    }
}

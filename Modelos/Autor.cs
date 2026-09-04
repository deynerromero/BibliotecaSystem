using System;

namespace BibliotecaSystem.Modelos
{
    /// <summary>
    /// Autor de uno o varios libros del catálogo.
    /// </summary>
    public class Autor
    {
        public string Nombre { get; set; }
        public string Nacionalidad { get; set; }
        public DateTime FechaNacimiento { get; set; }

        public Autor(string nombre, string nacionalidad, DateTime fechaNacimiento)
        {
            Nombre = nombre;
            Nacionalidad = nacionalidad;
            FechaNacimiento = fechaNacimiento;
        }

        public override string ToString() => Nombre;
    }
}

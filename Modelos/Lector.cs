using System;
using System.Collections.Generic;
using System.Linq;

namespace BibliotecaSystem.Modelos
{
    /// <summary>
    /// Lector/usuario de la biblioteca. Mantiene su propio historial de
    /// préstamos y el control de la sanción por devoluciones tardías.
    /// </summary>
    public class Lector
    {
        public const int MaxPrestamosActivos = 3;

        public string Id { get; set; }
        public string Nombre { get; set; }

        /// <summary>Fecha hasta la cual el lector está sancionado (no puede prestar). Null = sin sanción.</summary>
        public DateTime? FechaFinSancion { get; private set; }

        private readonly List<Prestamo> _prestamos = new List<Prestamo>();
        public IReadOnlyList<Prestamo> Prestamos => _prestamos.AsReadOnly();

        public Lector(string id, string nombre)
        {
            Id = id;
            Nombre = nombre;
        }

        /// <summary>Préstamos que aún no han sido devueltos.</summary>
        public IEnumerable<Prestamo> PrestamosActivos => _prestamos.Where(p => p.FechaDevolucion == null);

        public bool EstaSancionado(DateTime fecha) =>
            FechaFinSancion.HasValue && fecha.Date < FechaFinSancion.Value.Date;

        public bool PuedeTomarPrestamo(DateTime fecha) =>
            !EstaSancionado(fecha) && PrestamosActivos.Count() < MaxPrestamosActivos;

        internal void RegistrarPrestamo(Prestamo prestamo) => _prestamos.Add(prestamo);

        /// <summary>
        /// Aplica una sanción de 2 días por cada día de retraso, acumulándola
        /// si el lector ya tenía una sanción vigente.
        /// </summary>
        internal void AplicarSancion(int diasRetraso, DateTime fechaDevolucion)
        {
            if (diasRetraso <= 0) return;

            int diasSancion = diasRetraso * 2;
            DateTime baseFecha = FechaFinSancion.HasValue && FechaFinSancion.Value > fechaDevolucion
                ? FechaFinSancion.Value
                : fechaDevolucion;

            FechaFinSancion = baseFecha.AddDays(diasSancion);
        }

        public override string ToString() => $"{Nombre} ({Id})";
    }
}

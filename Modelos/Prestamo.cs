using System;

namespace BibliotecaSystem.Modelos
{
    /// <summary>
    /// Representa el préstamo de una Copia concreta a un Lector concreto,
    /// con la ventana de 30 días permitida por la biblioteca.
    /// </summary>
    public class Prestamo
    {
        public const int DiasMaximoPrestamo = 30;

        public Copia Copia { get; }
        public Lector Lector { get; }
        public DateTime FechaPrestamo { get; }
        public DateTime FechaLimite { get; }
        public DateTime? FechaDevolucion { get; private set; }

        public Prestamo(Copia copia, Lector lector, DateTime fechaPrestamo)
        {
            Copia = copia;
            Lector = lector;
            FechaPrestamo = fechaPrestamo.Date;
            FechaLimite = FechaPrestamo.AddDays(DiasMaximoPrestamo);
        }

        public bool EstaDevuelto => FechaDevolucion.HasValue;

        /// <summary>Días de retraso a una fecha dada (0 si aún no vence o ya fue devuelto a tiempo).</summary>
        public int DiasRetraso(DateTime fechaReferencia)
        {
            DateTime fin = FechaDevolucion ?? fechaReferencia.Date;
            int dias = (fin - FechaLimite).Days;
            return dias > 0 ? dias : 0;
        }

        internal void MarcarDevuelto(DateTime fechaDevolucion)
        {
            FechaDevolucion = fechaDevolucion.Date;
        }

        public override string ToString() =>
            $"{Copia.Id} -> {Lector.Nombre} ({FechaPrestamo:yyyy-MM-dd} a {FechaLimite:yyyy-MM-dd})";
    }
}

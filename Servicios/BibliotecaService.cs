using System;
using System.Collections.Generic;
using System.Linq;
using BibliotecaSystem.Enums;
using BibliotecaSystem.Modelos;

namespace BibliotecaSystem.Servicios
{
    public class OperacionInvalidaException : Exception
    {
        public OperacionInvalidaException(string mensaje) : base(mensaje) { }
    }

    /// <summary>
    /// Orquesta las reglas de negocio: préstamos, devoluciones, sanciones
    /// y cambios de estado de las copias. Es el único punto de entrada
    /// para modificar el estado del sistema de forma consistente.
    /// </summary>
    public class BibliotecaService
    {
        private readonly List<Copia> _copias = new List<Copia>();
        private readonly List<Lector> _lectores = new List<Lector>();
        private readonly List<Prestamo> _prestamos = new List<Prestamo>();

        public void RegistrarCopia(Copia copia) => _copias.Add(copia);
        public void RegistrarLector(Lector lector) => _lectores.Add(lector);
        public IReadOnlyList<Prestamo> Prestamos => _prestamos.AsReadOnly();

        /// <summary>
        /// Presta una copia a un lector, validando disponibilidad de la copia,
        /// límite de 3 préstamos activos y ausencia de sanción vigente.
        /// </summary>
        public Prestamo PrestarLibro(Copia copia, Lector lector, DateTime? fecha = null)
        {
            DateTime hoy = (fecha ?? DateTime.Today).Date;

            if (copia.Estado != EstadoCopia.EnBiblioteca)
                throw new OperacionInvalidaException($"La copia {copia.Id} no está disponible ({copia.Estado}).");

            if (lector.EstaSancionado(hoy))
                throw new OperacionInvalidaException(
                    $"{lector.Nombre} está sancionado hasta {lector.FechaFinSancion:yyyy-MM-dd}.");

            if (lector.PrestamosActivos.Count() >= Lector.MaxPrestamosActivos)
                throw new OperacionInvalidaException(
                    $"{lector.Nombre} ya tiene {Lector.MaxPrestamosActivos} préstamos activos.");

            var prestamo = new Prestamo(copia, lector, hoy);
            copia.Estado = EstadoCopia.Prestada;
            lector.RegistrarPrestamo(prestamo);
            _prestamos.Add(prestamo);
            return prestamo;
        }

        /// <summary>
        /// Registra la devolución de un préstamo. Si hubo retraso, aplica la
        /// sanción de 2 días por cada día de retraso al lector.
        /// </summary>
        public void DevolverLibro(Prestamo prestamo, DateTime? fecha = null)
        {
            DateTime hoy = (fecha ?? DateTime.Today).Date;

            if (prestamo.EstaDevuelto)
                throw new OperacionInvalidaException("Este préstamo ya fue devuelto.");

            int diasRetraso = prestamo.DiasRetraso(hoy);
            prestamo.MarcarDevuelto(hoy);

            if (diasRetraso > 0)
                prestamo.Lector.AplicarSancion(diasRetraso, hoy);

            prestamo.Copia.Estado = EstadoCopia.EnBiblioteca;
        }

        /// <summary>Envía una copia a reparación (por ejemplo, tras daño físico).</summary>
        public void EnviarAReparacion(Copia copia)
        {
            if (copia.Estado == EstadoCopia.Prestada)
                throw new OperacionInvalidaException("No se puede reparar una copia actualmente prestada.");
            copia.Estado = EstadoCopia.EnReparacion;
        }

        public void FinalizarReparacion(Copia copia)
        {
            if (copia.Estado != EstadoCopia.EnReparacion)
                throw new OperacionInvalidaException("La copia no está en reparación.");
            copia.Estado = EstadoCopia.EnBiblioteca;
        }

        /// <summary>
        /// Recorre los préstamos activos y marca como "ConRetraso" las copias
        /// cuya fecha límite ya pasó sin devolución. Pensado para ejecutarse
        /// una vez al día (job/cron).
        /// </summary>
        public void ActualizarRetrasos(DateTime? fecha = null)
        {
            DateTime hoy = (fecha ?? DateTime.Today).Date;
            foreach (var prestamo in _prestamos.Where(p => !p.EstaDevuelto))
            {
                if (hoy > prestamo.FechaLimite)
                    prestamo.Copia.Estado = EstadoCopia.ConRetraso;
            }
        }
    }
}

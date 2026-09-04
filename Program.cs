using System;
using BibliotecaSystem.Enums;
using BibliotecaSystem.Modelos;
using BibliotecaSystem.Servicios;

var biblioteca = new BibliotecaService();

var autor = new Autor("Robert C. Martin", "Estadounidense", new DateTime(1952, 12, 5));
var libro = new Libro("Clean Code", TipoLibro.Informatica, "Prentice Hall", 2008, autor);
var copia = new Copia("C-001", libro);
biblioteca.RegistrarCopia(copia);

var lector = new Lector("L-001", "Deyner");
biblioteca.RegistrarLector(lector);

// Préstamo el 1 de enero
var prestamo = biblioteca.PrestarLibro(copia, lector, new DateTime(2026, 1, 1));
Console.WriteLine($"Prestado: {prestamo}");

// Devolución con 5 días de retraso (límite 31 de enero, devuelto 5 de febrero)
biblioteca.DevolverLibro(prestamo, new DateTime(2026, 2, 5));
Console.WriteLine($"Copia tras devolución: {copia.Estado}");
Console.WriteLine($"Sancionado hasta: {lector.FechaFinSancion:yyyy-MM-dd}"); // 5 días de retraso x 2 = 10 días

// Intento de nuevo préstamo durante la sanción
try
{
    biblioteca.PrestarLibro(copia, lector, new DateTime(2026, 2, 6));
}
catch (OperacionInvalidaException ex)
{
    Console.WriteLine($"Rechazado: {ex.Message}");
}

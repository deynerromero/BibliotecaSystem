# Diagrama de Clases - BibliotecaSystem

```mermaid
classDiagram
    namespace Modelos {
        class Autor {
            +string Nombre
            +string Nacionalidad
            +DateTime FechaNacimiento
            +Autor(string, string, DateTime)
            +ToString() string
        }

        class Libro {
            +string Nombre
            +TipoLibro Tipo
            +string Editorial
            +int Anio
            +Autor Autor
            +Libro(string, TipoLibro, string, int, Autor)
            +ToString() string
        }

        class Copia {
            +string Id
            +Libro Libro
            +EstadoCopia Estado
            +Copia(string, Libro)
            +ToString() string
        }

        class Lector {
            +const int MaxPrestamosActivos = 3
            +string Id
            +string Nombre
            -DateTime? FechaFinSancion
            -List~Prestamo~ _prestamos
            +IReadOnlyList~Prestamo~ Prestamos
            +Lector(string, string)
            +IEnumerable~Prestamo~ PrestamosActivos
            +bool EstaSancionado(DateTime) bool
            +bool PuedeTomarPrestamo(DateTime) bool
            +void RegistrarPrestamo(Prestamo)
            +void AplicarSancion(int, DateTime)
            +ToString() string
        }

        class Prestamo {
            +const int DiasMaximoPrestamo = 30
            +Copia Copia
            +Lector Lector
            +DateTime FechaPrestamo
            +DateTime FechaLimite
            +DateTime? FechaDevolucion
            +Prestamo(Copia, Lector, DateTime)
            +bool EstaDevuelto
            +int DiasRetraso(DateTime) int
            +void MarcarDevuelto(DateTime)
            +ToString() string
        }
    }

    namespace Enums {
        enum EstadoCopia {
            EnBiblioteca
            Prestada
            ConRetraso
            EnReparacion
        }

        enum TipoLibro {
            Ingenieria
            Literatura
            Informatica
            Historia
            Ciencia
            Arte
            Otro
        }
    }

    namespace Servicios {
        class OperacionInvalidaException {
            +OperacionInvalidaException(string)
        }

        class BibliotecaService {
            -List~Copia~ _copias
            -List~Lector~ _lectores
            -List~Prestamo~ _prestamos
            +IReadOnlyList~Prestamo~ Prestamos
            +void RegistrarCopia(Copia)
            +void RegistrarLector(Lector)
            +Prestamo PrestarLibro(Copia, Lector, DateTime?)
            +void DevolverLibro(Prestamo, DateTime?)
            +void EnviarAReparacion(Copia)
            +void FinalizarReparacion(Copia)
            +void ActualizarRetrasos(DateTime?)
        }
    }

    %% Relaciones
    Libro --> Autor : tiene
    Copia --> Libro : referencia a
    Copia --> EstadoCopia : usa
    Libro --> TipoLibro : clasificado como
    Lector --> Prestamo : tiene muchos
    Prestamo --> Copia : presta
    Prestamo --> Lector : prestado a
    BibliotecaService --> Copia : gestiona
    BibliotecaService --> Lector : gestiona
    BibliotecaService --> Prestamo : gestiona
    BibliotecaService --> OperacionInvalidaException : lanza
    Exception <|-- OperacionInvalidaException : hereda
```

## Descripción del Sistema

### Modelos (Modelos/)
- **Autor**: Información del autor de libros
- **Libro**: Obra con tipo y metadatos
- **Copia**: Ejemplar físico de un libro
- **Lector**: Usuario de la biblioteca
- **Préstamo**: Relación entre copia y lector

### Enumeraciones (Enums/)
- **EstadoCopia**: Estados posibles de una copia
- **TipoLibro**: Clasificación temática de libros

### Servicios (Servicios/)
- **BibliotecaService**: Orquestador de reglas de negocio
- **OperacionInvalidaException**: Excepción para operaciones inválidas

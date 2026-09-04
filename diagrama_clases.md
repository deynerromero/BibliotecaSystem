# Diagrama de Clases - BibliotecaSystem

```mermaid
classDiagram
    %% Enumeraciones
    class EstadoCopia {
        <<enumeration>>
        EnBiblioteca
        Prestada
        ConRetraso
        EnReparacion
    }

    class TipoLibro {
        <<enumeration>>
        Ingenieria
        Literatura
        Informatica
        Historia
        Ciencia
        Arte
        Otro
    }

    %% Modelos
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
        +bool EstaSancionado(DateTime)
        +bool PuedeTomarPrestamo(DateTime)
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
        +int DiasRetraso(DateTime)
        +void MarcarDevuelto(DateTime)
        +ToString() string
    }

    %% Servicios
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

### 📚 Modelos (Carpeta: `Modelos/`)
| Clase | Descripción |
|-------|-------------|
| **Autor** | Información del autor (nombre, nacionalidad, fecha nacimiento) |
| **Libro** | Obra con tipo, editorial, año y autor |
| **Copia** | Ejemplar físico de un libro con estado |
| **Lector** | Usuario de la biblioteca con historial de préstamos |
| **Préstamo** | Asociación copia-lector con período de 30 días |

### 📋 Enumeraciones (Carpeta: `Enums/`)
| Enum | Valores |
|------|---------|
| **EstadoCopia** | EnBiblioteca, Prestada, ConRetraso, EnReparacion |
| **TipoLibro** | Ingenieria, Literatura, Informatica, Historia, Ciencia, Arte, Otro |

### ⚙️ Servicios (Carpeta: `Servicios/`)
| Clase | Responsabilidad |
|-------|-----------------|
| **BibliotecaService** | Orquesta las reglas de negocio: préstamos, devoluciones, sanciones |
| **OperacionInvalidaException** | Excepción para operaciones inválidas |

### 🔗 Relaciones Principales
- Un **Libro** tiene un **Autor**
- Una **Copia** referencia a un **Libro** y tiene un **EstadoCopia**
- Un **Lector** puede tener múltiples **Préstamos**
- Un **Préstamo** vincula una **Copia** con un **Lector**
- El **BibliotecaService** gestiona todas las entidades

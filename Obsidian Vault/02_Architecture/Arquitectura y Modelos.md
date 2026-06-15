---
tags:
  - architecture
  - database
  - documentation
---
# Arquitectura y Modelos - Caporale Barbería

Este documento define la estructura de datos, las relaciones del modelo y cómo se conectan los componentes de la aplicación.

## 🗄️ Modelo de Base de Datos (SQLite + EF Core)

El sistema utiliza SQLite para el almacenamiento de datos persistentes. A continuación se detallan las entidades clave y las restricciones críticas:

### 1. Entidades Principales

#### `User` (Usuarios)
- `Id` (Guid, PK)
- `Name` (string)
- `Email` (string)
- `PasswordHash` (string)
- `Role` (string: Cliente, Barbero, Administrador)
- `IsBanned` (bool)

#### `Hairdresser` (Peluqueros / Barberos)
- `Id` (Guid, PK)
- `Name` (string)
- `Specialty` (string)

#### `Appointment` (Turnos / Reservas)
- `Id` (Guid, PK)
- `HairdresserId` (Guid, FK)
- `ClientId` (Guid, FK)
- `Date` (DateTime)
- `IsCanceled` (bool) — *Utilizado para Soft Delete y reciclado*
- `IsRecurrent` (bool)
- `RecurrenceType` (int: 0 = Ninguna, 1 = Semanal, 2 = Quincenal)

#### `BannedHardwareProfile` (Perfiles de Hardware Baneados)
- `Id` (Guid, PK)
- `HardwareHash` (string, Unique) — *Combinación de WebGL + AudioContext + CPU*
- `DateBanned` (DateTime)
- `Reason` (string)

> [!IMPORTANT]
> **Restricción de SQLite Crítica:**
> Existe un índice único compuesto: `UNIQUE(HairdresserId, Date)`. Para evitar excepciones al reactivar turnos previamente cancelados, la inserción lógica debe verificar si ya existe un registro con `IsCanceled = true` y actualizarlo en lugar de crear un registro duplicado.

---

## 🔌 Conexión de Componentes y Flujo de Datos

El flujo sigue el patrón MVC / API clásico en ASP.NET Core:

```mermaid
graph TD
    %% Bloque de diagrama de flujo vacío para completarlo según sea necesario.
    %% (Dibuja aquí las interacciones entre los componentes)
```

1. **Frontend (TypeScript modular):** Captura las interacciones y genera el Fingerprint del hardware mediante `@fingerprintjs/fingerprintjs`.
2. **Model Binding & Controllers:** Los controllers (`AppointmentsController`, `AdminController`) reciben DTOs fuertemente tipados en formato JSON (camelCase).
3. **Servicios (C#):** Inyección de dependencias para lógica de negocio (`AppointmentService`, `SecurityService`).
4. **Persistencia (EF Core):** Consultas controladas y prevenciones contra nulos (`FirstOrDefaultAsync`).

---

## 🔗 Enlaces de Interés (Obsidian)
- [[MVP y Estado Actual]]
- [[Background Services y Procesos]]
- [[Tablero Kanban de Cierre]]

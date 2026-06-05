Actúa como un Ingeniero de Software Senior y Mentor de Programación con más de 10 años de experiencia. Tu objetivo es guiarme para que yo resuelva los problemas de código por mí mismo, asegurando que yo desarrolle el pensamiento lógico y crítico.

Sigue estrictamente estas reglas en todas tus respuestas:

Prohibido el código crudo: No me des bloques de código listos para copiar y pegar. No escribas funciones ni scripts completos por mí.

Usa el método socrático: Si te planteo un problema o un bug, hazme preguntas guía que me ayuden a identificar el error o la solución por mi cuenta.

Explica con pseudocódigo o conceptos: Si necesito entender la estructura, utiliza pseudocódigo de alto nivel, diagramas de flujo conceptuales o analogías.

Divide el problema: Ayúdame a descomponer problemas complejos en tareas o pasos lógicos muy pequeños que yo pueda codificar individualmente.

Valida mi lógica: Revisa el código que yo te comparta. Dime qué partes están bien y señala conceptualmente dónde están los fallos o qué casos de borde (edge cases) olvidé considerar.

Si has entendido perfectamente que no debes darme código directo, sino guiar mi razonamiento, responde únicamente con: "Entendido. Estoy listo para ser tu mentor. ¿Qué problema o funcionalidad vamos a analizar hoy?" y espera mi mensaje.


# Contexto del Proyecto: Caporale Barbería
## Planificación de Sprint (Scrum - Duración: 2 Semanas)

Este documento contiene el estado actual del desarrollo, la arquitectura técnica de base y el **Sprint Backlog** estructurado para las próximas dos semanas. Su objetivo es alinear el desarrollo de las nuevas funcionalidades (turnos recurrentes, dashboards, facturación y seguridad avanzada).

---

## 1. Stack Tecnológico y Estado Actual
* **Backend:** ASP.NET Core MVC / Web API (C#), Entity Framework Core.
* **Base de Datos:** SQLite (Maneja un índice único compuesto: `UNIQUE(HairdresserId, Date)`).
* **Frontend:** TypeScript (TS) modular en el cliente y vistas dinámicas con **Razor (.cshtml)**.
* **Estado del Módulo de Turnos:** Ya está implementada la lógica de cancelación asrincrónica (*Soft Delete* mediante `IsCanceled = true`) y el **patrón de reciclado de registros** en el método `InsertAppointmentAsync` para reutilizar filas existentes y no romper la restricción `UNIQUE` de SQLite.

---

## 2. Objetivo del Sprint (Sprint Goal)
Consolidar el módulo de reservas con turnos recurrentes, desarrollar los paneles de control (Peluqueros y Administrador), implementar el flujo de caja/facturación y desplegar un sistema de baneo dual (Usuario y Hardware Fingerprinting) inter-navegador.

---

## 3. Sprint Backlog (Dividido por Épicas y Tareas)

### Épica 1: Gestión de Turnos Avanzada y Recurrentes
* **Tarea 1.1: Cierre del módulo de reserva actual.** Asegurar el flujo feliz de reserva y cancelación con actualización dinámica de la UI en la grilla de turnos.
* **Tarea 1.2: Implementación de Turnos Fijos (Recurrentes).** * Modificar el DTO de reserva y la tabla `Appointments` para soportar recurrencia: *Sin recurrencia*, *Cada 1 semana*, *Cada 2 semanas*.
    * Desarrollar la lógica en el backend para que, al reservar un turno recurrente, se generen automáticamente los bloqueos de agenda a futuro en los slots del peluquero (respetando el patrón de reciclado si ya existen filas canceladas en esas fechas).

### Épica 2: Dashboard y Vista del Peluquero
* **Tarea 2.1: Panel de Agenda Interno.** Crear una vista privada para los barberos donde puedan loguearse y visualizar su grilla de turnos diaria y semanal.
* **Tarea 2.2: Filtros de Visualización.** Filtros por estado del turno (Confirmados, Cancelados) y ordenamiento cronológico.

### Épica 3: Dashboard de Administrador, Facturación y Configuración
* **Tarea 3.1: Módulo de Facturación y Caja.**
    * Desarrollar reportes de ingresos en el panel de administrador.
    * Filtrar facturación total por rangos de fecha (Día, Semana, Mes) y por rendimiento individual de cada peluquero.
* **Tarea 3.2: Gestión Dinámica de Precios.**
    * Crear interfaz de administración para modificar los precios de los servicios de corte y barbería en tiempo real sin tocar código.
* **Tarea 3.3: Panel de Control de Usuarios.** Vista global para ver el listado de clientes registrados y sus historiales de asistencia.

### Épica 4: Sistema de Seguridad Perimetral y Manejo de Baneos
* **Tarea 4.1: Baneo Lógico por Cuenta (`UserId`).**
    * Agregar la bandera `IsBanned` a la tabla de usuarios. Si está activa, impedir el login y la reserva de turnos desde el backend.
* **Tarea 4.2: Baneo por Hardware Avanzado (*Cross-Browser Fingerprinting*).**
    * **Frontend (TypeScript):** Integrar la librería `@fingerprintjs/fingerprintjs` (versión Open Source). Extraer de forma aislada los componentes físicos del dispositivo: hash de renderizado **WebGL (GPU)**, hash de **AudioContext** y métricas de **Hardware Concurrency** (hilos de CPU) y memoria RAM.
    * Generar un perfil de hardware único combinando estos datos para identificar el dispositivo incluso si el usuario cambia de navegador (de Chrome a Firefox) o usa el modo incógnito.
    * **Backend (ASP.NET Core):** Crear la tabla `BannedHardwareProfiles` en SQLite. Implementar una validación (*Guard Clause*) en el servicio de inserción de turnos que cruce los hashes de la máquina del cliente y bloquee la transacción con un HTTP `403 Forbid` si coincide con un perfil listado.

---

## 4. Criterios de Aceptación Técnicos para el Desarrollo
1.  **Model Binding Prolijo:** Todos los nuevos endpoints de los controladores deben mapear parámetros tipados de forma exacta con los requests del frontend (utilizando objetos anónimos en formato camelCase para las respuestas JSON hacia TypeScript).
2.  **Validación de Modelos:** Uso obligatorio de `ModelState.IsValid` antes de invocar cualquier lógica de los servicios en las nuevas acciones de los dashboards.
3.  **Prevención de Nulos:** Mantener el blindaje del backend interceptando consultas vacías con `FirstOrDefaultAsync` antes de alterar estados de entidades.

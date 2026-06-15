---
tags:
  - specs
  - mvp
  - phase/stabilization
---
# MVP y Estado Actual - Caporale Barbería

Este documento resume el alcance del MVP, las características implementadas y los pendientes de estabilización y cierre antes de dar por terminado el proyecto.

## 🚀 Resumen del Sistema
**Caporale Barbería** es una plataforma web para la gestión de turnos de barbería. Permite a los clientes reservar citas de forma ágil, a los barberos gestionar su agenda diaria y semanal, y a los administradores visualizar reportes de facturación, gestionar precios en tiempo real y aplicar un sistema avanzado de baneo perimetral para proteger la integridad de la plataforma.


---

## ✅ Funcionalidades 100% Listas (Producción)

### 1. Núcleo de Reservas (Turnos Simples)
- [x] Lógica de reserva y actualización dinámica de la UI en la grilla de turnos.
- [x] Cancelación de turnos mediante **Soft Delete** (`IsCanceled = true`).
- [x] **Patrón de Reciclado de Registros** en el método `InsertAppointmentAsync` para reutilizar filas canceladas y no violar la restricción `UNIQUE(HairdresserId, Date)` de SQLite.
- [x] Stack base configurado: ASP.NET Core MVC / Web API y Entity Framework Core.
- [x] Frontend modular con TypeScript en el cliente y vistas dinámicas en Razor (`.cshtml`).
	
### 2. Autenticación y Cuentas
- [x] Registro y login de clientes.
- [x] Control de roles básico (Cliente, Barbero, Administrador).

---

## ⏳ Detalles de Última Hora (Pendientes de Cierre)

Antes de finalizar la fase de estabilización y dar por concluido el proyecto, se deben resolver las siguientes tareas críticas del sprint:

### 1. Gestión de Turnos Recurrentes
- [ ] Implementar la recurrencia (*Sin recurrencia*, *Cada 1 semana*, *Cada 2 semanas*) en la tabla `Appointments` y actualizar la lógica de reserva para bloquear slots futuros aplicando el patrón de reciclado.

### 2. Dashboards de Control
- [ ] **Vista del Peluquero:** Filtrado y ordenamiento de agenda para los barberos.
- [ ] **Facturación e Ingresos:** Panel de administración con reportes de caja, filtros de rango de fechas y desempeño por barbero.
- [ ] **Configuración Dinámica:** Panel para ajustar precios de corte y barbería sin alterar código.

### 3. Seguridad Avanzada y Baneo Perimetral
- [ ] **Baneo Lógico:** Bloqueo de cuentas mediante flag `IsBanned` en la tabla de usuarios.
- [ ] **Baneo de Hardware (Cross-Browser Fingerprinting):**
  - [ ] Extracción y hasheo en frontend (TypeScript con `@fingerprintjs/fingerprintjs`) de WebGL (GPU), AudioContext, Hardware Concurrency y memoria RAM.
  - [ ] Validación en el backend cruzando datos contra la tabla `BannedHardwareProfiles` de SQLite (HTTP 403 Forbid en caso de coincidencia).

---

## 🔗 Enlaces de Interés (Obsidian)
- [[Arquitectura y Modelos]]
- [[Background Services y Procesos]]
- [[Tablero Kanban de Cierre]]

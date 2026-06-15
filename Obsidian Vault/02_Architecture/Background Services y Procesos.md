---
tags:
  - architecture
  - background-services
  - logs
---
# Background Services y Procesos - Caporale Barbería

Documentación del ciclo de vida y diseño técnico de las tareas y servicios en segundo plano del sistema.

## ⚙️ Servicio de Gestión y Reciclado de Turnos

### 🎯 Disparador (Trigger)
- **Tipo:** `IHostedService` / `BackgroundService` en ASP.NET Core.
- **Frecuencia:** Ejecución diaria a la medianoche (`00:00:00`) o a través de un scheduler interno.

### 🔄 Flujo de Trabajo (Workflow)
1. **Inicialización:** Levanta el contexto de base de datos a través de una fábrica de scopes (`IServiceScopeFactory`).
2. **Lectura de Recurrencias:** Consulta la base de datos buscando citas que tengan habilitado `IsRecurrent = true` y que su fecha próxima caiga en el rango de planificación.
3. **Validación de Conflictos:**
   - Comprueba si el slot del peluquero para la fecha futura ya está ocupado por un turno activo (`IsCanceled == false`).
   - Si existe un turno cancelado (`IsCanceled == true`) en ese slot, se **recicla** el registro modificando el `ClientId`, `IsCanceled = false` y actualizando el estado.
   - Si no existe ningún registro, se genera una nueva reserva.
4. **Persistencia:** Guarda los cambios en bloque mediante `SaveChangesAsync()`.

### 🚨 Manejo de Errores y Excepciones
- **Transaccionalidad:** Cada lote de procesamiento se ejecuta dentro de una transacción de Entity Framework Core para evitar estados inconsistentes (agenda a medias).
- **Control de Excepciones:** Captura de `DbUpdateConcurrencyException` y fallos de conexión de SQLite.
- **Logs:** Registro de eventos en la salida de logs configurada para producción con niveles de criticidad (Info/Warning/Error).

---

## 🔗 Enlaces de Interés (Obsidian)
- [[MVP y Estado Actual]]
- [[Arquitectura y Modelos]]
- [[Tablero Kanban de Cierre]]

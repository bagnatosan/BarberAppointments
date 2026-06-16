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
- **Frecuencia:** Ejecución diaria  (`19:00:00`).
- Remover el `await Task.Delay` y calcular la hora para que se ejecute a la hora deseada.

### 🔄 Flujo de Trabajo (Workflow)
1. **Inicialización:** Levanta el contexto de base de datos a través de una fábrica de scopes (`IServiceScopeFactory`).
2. **Lectura de Recurrencias:** Consulta la base de datos buscando citas que tengan habilitado `IsRecurrent = true` y que su fecha próxima caiga en el rango de planificación. También se debe buscar en appointment la que tenga el mismo id de recurrencia, y la ultima haciendo un ordenamiento y eligiendo la primera
3. **Bucles:** Se recorre con un foreach todos los turnos recurrentes y se hace un for (i = 0; i < 30; i++) en el cual se va sumando dias.
4. **Calculo:** 
- **Calcular la diferencia en días**: Restamos las dos fechas para saber cuántos días de separación hay: `diferenciaDias = fechaCandidata - fechaBase`
- **Convertir a semanas**: Dividimos esa cantidad de días por 7: `diferenciaSemanas = diferenciaDias / 7`
- **Comprobar la cadencia**: Usamos el operador `%` con el intervalo (`IntervalWeeks` que puede ser 1 o 2): `¿Corresponde? = (diferenciaSemanas % IntervalWeeks) == 0`.  
5. **Persistencia:** Guarda los cambios en bloque mediante `SaveChangesAsync()`.

>[!warning] 
>Si cancelo un turno recurrente (un solo dia) y quiere hacer una insersion despues hace falta chequear que si cumple todo lo demas pero el turno esta cancelado, que no haga nada.

>[!tip]
>Al comparar la fecha base con la de 15 dias, hacerlo con .date para comparar mes, dia y no la horas.

>[!warning] Warning
>Hay que cambiar que cuando se cree una recurrent appointment se ponga el corte de pelo. El cambio debe impactar en appointmentService y appointmentGenerator
>
### 🚨 Manejo de Errores y Excepciones
- **Transaccionalidad:** Cada lote de procesamiento se ejecuta dentro de una transacción de Entity Framework Core para evitar estados inconsistentes (agenda a medias).
- **Control de Excepciones:** Captura de `DbUpdateConcurrencyException` y fallos de conexión de SQLite.
- **Logs:** Registro de eventos en la salida de logs configurada para producción con niveles de criticidad (Info/Warning/Error).
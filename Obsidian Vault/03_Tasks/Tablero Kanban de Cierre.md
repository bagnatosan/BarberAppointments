---

kanban-plugin: board

---

## To Do

- [ ] Configurar logs de producción en `Program.cs` y `appsettings.json` para capturar excepciones en Background Services.
- [ ] Revisar inyección de dependencias en Background Services (asegurar el uso correcto de `IServiceScopeFactory` al resolver contextos efímeros como `BarberContext`).
- [ ] Escribir pruebas unitarias críticas para el flujo de reciclado en `InsertAppointmentAsync` y prevención de conflictos de índice único en SQLite.


## In Progress



## Done





%% kanban:settings
```
{"kanban-plugin":"board","list-collapse":[null,false]}
```
%%
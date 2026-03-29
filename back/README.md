# Prueba Eventos

Guía rápida para compilar y ejecutar la solución `prueba-eventos`.

Prerequisitos
- .NET 10 SDK instalado
- Credenciales de AWS configuradas (si va a usar SQS/Dynamo). Puede usar variables de entorno `AWS_REGION`, `AWS_ACCESS_KEY_ID`, `AWS_SECRET_ACCESS_KEY` o un perfil de AWS.
- Redis disponible si desea habilitar caché distribuida (configurar `Redis:Configuration` en `appsettings.json`).

Configuración importante
- `HS.Eventos.WebApi/appsettings.json` contiene claves relevantes:
  - `AWS:Region` y `AWS:Profile` — configuración de AWS
  - `Colas:Eventos` — nombre de la cola SQS usada por el proyecto
  - `Redis:Configuration` — cadena de conexión para Redis
  - `Cache:Enabled` — activar/desactivar caching (por defecto `false`)

Compilar
1. Desde la raíz del repositorio:
```
dotnet build
```

Ejecutar API Web
1. Desde la raíz del repositorio puede ejecutar la API:
```
dotnet run --project HS.Eventos.WebApi
```
2. Por defecto Kestrel imprimirá la(s) URL(s) en la consola. También puede fijar la URL con la variable de entorno:
```
ASPNETCORE_URLS="http://localhost:5000" dotnet run --project HS.Eventos.WebApi
```

Ejecutar el consumidor de colas (Jobs)
1. Para ejecutar el worker que consume mensajes:
```
dotnet run --project HS.Eventos.Jobs
```

Variables de entorno útiles
- `ASPNETCORE_ENVIRONMENT` — por ejemplo `Development`
- `AWS_REGION`, `AWS_ACCESS_KEY_ID`, `AWS_SECRET_ACCESS_KEY` — para acceso a AWS
- `Cache__Enabled` — `true`/`false` para activar caché (sintaxis de variables de entorno para `Cache:Enabled`)
- `Redis__Configuration` — cadena de conexión para Redis

Ejemplos de uso
- Crear un evento (POST):
```
curl -X POST http://localhost:5000/api/evento \
  -H "Content-Type: application/json" \
  -d '{"Nombre":"Concierto","Fecha":"2026-06-01","Lugar":"Auditorio","Zonas":[]}'
```
- Obtener lista de eventos (GET):
```
curl http://localhost:5000/api/evento
```
- Obtener evento por id (GET):
```
curl http://localhost:5000/api/evento/{id}
```

Notas
- Asegúrese de configurar las credenciales y la región de AWS si utiliza SQS/Dynamo.
- Para desarrollo local de caché, puede ejecutar un contenedor Redis y ajustar `Redis:Configuration` a `localhost:6379`.

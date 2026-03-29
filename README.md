# prueba-eventos

Este repositorio esta organizado en tres carpetas principales:

- `back/`: solucion backend en .NET con dominio, aplicacion, acceso a datos, mensajeria y APIs.
- `front/`: aplicacion frontend (Next.js).
- `diseño/`: recursos de diseno y diagramas.

## Estructura principal

### back/

Contiene la solucion `HS.Eventos.slnx` y los proyectos del backend:

- `HS.Core/`: utilidades y contratos base compartidos (entidades, errores, paginacion, interfaces de lectura/escritura).
- `HS.Eventos.Aplicacion/`: capa de aplicacion (casos de uso, comandos, handlers y eventos).
- `HS.Eventos.Dominio/`: reglas de dominio, entidades, repositorios y servicios de negocio.
- `HS.Eventos.DataAccess.Dynamo/`: implementacion de acceso a datos con DynamoDB (mappers, modelos, repositorios).
- `HS.Eventos.Messaging.SQS/`: integracion de mensajeria con AWS SQS.
- `HS.Eventos.Models/`: modelos compartidos (DTOs, requests y responses).
- `HS.Eventos.WebApi/`: API HTTP (controladores, configuracion y arranque).
- `HS.Eventos.Jobs/`: procesos de background/consumo de cola (job runner y consumer).
- `HS.Mediator/`: implementacion de patron mediator y behaviors.

Tambien incluye `README.md` propio dentro de `back/`.

### front/

Incluye el proyecto:

- `eventos-app/`: aplicacion frontend construida con Next.js.
  - `app/`: rutas y estructura principal de la aplicacion.
  - `components/`: componentes reutilizables de UI.
  - `public/`: recursos estaticos.
  - `certificates/`: certificados usados por el entorno.
  - Archivos de configuracion como `package.json`, `next.config.ts`, `tsconfig.json` y `eslint.config.mjs`.

### diseño/

Contiene material de apoyo funcional/visual:

- `Eventos.drawio`: diagrama editable.
- `Eventos.drawio.svg`: version exportada.
- `readme.md`: notas adicionales de la carpeta.

## Notas

- Varias carpetas incluyen `bin/`, `obj/`, `.next/`, `node_modules/` y otros artefactos de compilacion/desarrollo.
- Para informacion mas especifica por modulo, revisar los `README.md` internos donde existan.
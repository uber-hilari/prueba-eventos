# Arquitectura Plataforma de Eventos (draw.io)

![Arquitectura Plataforma](Eventos.drawio.svg)

## 1. Frontend
- **Aplicación Web**: desarrollada con React / NextJS y desplegada en **AWS Amplify**.
- Los usuarios acceden a la plataforma desde el navegador.

## 2. API & Autenticación
- **API Gateway**: punto de entrada para las llamadas REST/CRUD.
- **WAF**: protección contra ataques y filtrado de tráfico.
- **Elastic Beanstalk (API CRUD)**: despliegue de servicios .NET Core para lógica de negocio.
- Autenticación y autorización basada en **JWT / OIDC / OAuth 2.0**.

## 3. Gestión de Eventos
- **Elastic Beanstalk (API CRUD)**: expone operaciones de creación y administración de eventos.
- **Lambda (Background Task)**: tareas en segundo plano.
- **DynamoDB (NoSQL)**: almacenamiento flexible de datos de eventos.

## 4. Gestión de Tickets
- **Elastic Beanstalk (Registro Tickets & Reservas)**: maneja reservas y emisión de tickets.
- **Aurora (SQL)**: base de datos relacional para consistencia y transacciones.
- **Lambda (Gen PDF / Mail)**: generación de tickets en PDF y envío por correo.
- **S3 (PDF/QR)**: almacenamiento y distribución de tickets digitales.

## 5. Usuario & Perfiles
- **Elastic Beanstalk (API CRUD)**: gestión de perfiles y roles.
- **Aurora (SQL)**: persistencia de usuarios y permisos.
- **Lambda (Background Task)**: procesos auxiliares relacionados con usuarios.

## 6. Pagos & Reembolsos
- **Elastic Beanstalk (Pagos & Reembolsos)**: lógica de integración con PSP.
- **Aurora (SQL)**: almacenamiento de transacciones financieras.
- **PSP externo**: proveedor de pagos conectado.
- Comunicación asincrónica con **SQS / RabbitMQ** para resiliencia.

## 7. Notificaciones
- **Lambda (Envio de Mails)**: envío de correos electrónicos.
- Integración con colas/mensajería para desacoplar procesos.

## 8. Monitoreo & Logs
- **CloudWatch**: métricas y logs centralizados.
- **Grafana**: visualización de métricas y dashboards.
- Integración con colas/eventos para trazabilidad.

## 9. Comunicación Asincrónica
- **SQS / RabbitMQ**: broker de mensajería para desacoplar microservicios.
- Conexiones desde gestión de eventos, tickets, usuarios y pagos hacia la cola.


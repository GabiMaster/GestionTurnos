<<<<<<< HEAD
# ReservIt! - Sistema de Gestión de Turnos

## Descripción
ReservIt! es una solución web para la gestión y reserva de turnos para servicios profesionales. Permite a los usuarios registrarse, iniciar sesión, visualizar servicios disponibles, agendar turnos y gestionar sus reservas. El sistema incluye una SPA (Single Page Application) que consume la API en tiempo real y una aplicación Razor Pages tradicional.

## Herramientas utilizadas
- .NET 8 y .NET 9
- ASP.NET Core Razor Pages
- SPA con Alpine.js y Bootstrap 5
- Entity Framework Core (SQL Server)
- JWT para autenticación
- ZXing.Net para generación de QR
- Visual Studio / VS Code

## Estructura del sistema
- **BackEnd**: API RESTful en ASP.NET Core, maneja usuarios, servicios, turnos y autenticación.
- **FrontEnd Web**: Aplicación Razor Pages para gestión tradicional.
- **FrontEnd SPA**: Aplicación SPA con Alpine.js para experiencia reactiva y consumo en tiempo real de la API.
- **Shared.DTO**: Modelos y DTOs compartidos entre backend y frontend.

## Instrucciones de uso
1. Clona el repositorio y abre la solución en Visual Studio.
2. Configura la cadena de conexión a SQL Server en `appsettings.json`.
3. Ejecuta las migraciones para crear la base de datos.
4. Inicia el proyecto `GestionTurnos.BackEnd.API` (API) y `GestionTurnos.FrontEnd.Web` (Razor Pages).
5. Accede a la SPA en `GestionTurnos.FrontEnd.SPA/index.html` (puede requerir servidor local o Live Server).
6. El backend debe estar corriendo en `https://localhost:7298` para que la SPA funcione correctamente.
7. Regístrate, inicia sesión y comienza a gestionar tus turnos.

## Escenario de uso
El sistema está pensado para profesionales y clientes que necesitan organizar y reservar turnos de manera eficiente. Permite la gestión de servicios, profesionales, turnos y confirmación mediante QR. La SPA permite interacción en tiempo real y la web tradicional cubre escenarios administrativos y de gestión avanzada.

## Notas
- El backend debe estar encendido para que la SPA funcione y consulte datos en tiempo real.
- El sistema soporta autenticación JWT y roles (Administrador, Cliente).
- La generación y validación de QR permite confirmar asistencia a los turnos.

---
=======
# GestionTurnos
>>>>>>> 839875854f8ce62244c3f0e7ca3f7e06568e83d4

<<<<<<< HEAD
# ReservIt! - Sistema de Gesti�n de Turnos

## Descripci�n
ReservIt! es una soluci�n web para la gesti�n y reserva de turnos para servicios profesionales. Permite a los usuarios registrarse, iniciar sesi�n, visualizar servicios disponibles, agendar turnos y gestionar sus reservas. El sistema incluye una SPA (Single Page Application) que consume la API en tiempo real y una aplicaci�n Razor Pages tradicional.

## Herramientas utilizadas
- .NET 8 y .NET 9
- ASP.NET Core Razor Pages
- SPA con Alpine.js y Bootstrap 5
- Entity Framework Core (SQL Server)
- JWT para autenticaci�n
- ZXing.Net para generaci�n de QR
- Visual Studio / VS Code

## Estructura del sistema
- **BackEnd**: API RESTful en ASP.NET Core, maneja usuarios, servicios, turnos y autenticaci�n.
- **FrontEnd Web**: Aplicaci�n Razor Pages para gesti�n tradicional.
- **FrontEnd SPA**: Aplicaci�n SPA con Alpine.js para experiencia reactiva y consumo en tiempo real de la API.
- **Shared.DTO**: Modelos y DTOs compartidos entre backend y frontend.

## Instrucciones de uso
1. Clona el repositorio y abre la soluci�n en Visual Studio.
2. Configura la cadena de conexi�n a SQL Server en `appsettings.json`.
3. Ejecuta las migraciones para crear la base de datos.
4. Inicia el proyecto `GestionTurnos.BackEnd.API` (API) y `GestionTurnos.FrontEnd.Web` (Razor Pages).
5. Accede a la SPA en `GestionTurnos.FrontEnd.SPA/index.html` (puede requerir servidor local o Live Server).
6. El backend debe estar corriendo en `https://localhost:7298` para que la SPA funcione correctamente.
7. Reg�strate, inicia sesi�n y comienza a gestionar tus turnos.

## Escenario de uso
El sistema est� pensado para profesionales y clientes que necesitan organizar y reservar turnos de manera eficiente. Permite la gesti�n de servicios, profesionales, turnos y confirmaci�n mediante QR. La SPA permite interacci�n en tiempo real y la web tradicional cubre escenarios administrativos y de gesti�n avanzada.

## Notas
- El backend debe estar encendido para que la SPA funcione y consulte datos en tiempo real.
- El sistema soporta autenticaci�n JWT y roles (Administrador, Cliente).
- La generaci�n y validaci�n de QR permite confirmar asistencia a los turnos.

---
=======
# GestionTurnos
>>>>>>> 839875854f8ce62244c3f0e7ca3f7e06568e83d4

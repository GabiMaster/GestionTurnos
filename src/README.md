# ReservIt! - Sistema de Gesti�n de Turnos

## Descripci�n
ReservIt! es un sistema web para la gesti�n y reserva de turnos para servicios profesionales. Permite a los usuarios registrarse, reservar turnos, recibir confirmaciones y gestionar sus reservas. Los administradores pueden gestionar servicios y profesionales desde una interfaz protegida.

## Escenario
- Usuarios pueden registrarse, iniciar sesi�n y reservar turnos para distintos servicios.
- Los turnos pueden ser agendados tanto desde la interfaz tradicional (Razor Pages) como desde el SPA (Single Page Application) con Alpine.js.
- Los administradores pueden crear, editar y eliminar servicios y profesionales.
- El sistema env�a correos para recuperaci�n de contrase�a con enlaces seguros.

## Herramientas y Tecnolog�as Utilizadas
- **.NET 8 / .NET 9**: Backend y Frontend Razor Pages
- **ASP.NET Core Razor Pages**: Interfaz tradicional
- **Alpine.js**: SPA para agendamiento y administraci�n r�pida
- **Bootstrap 5**: Estilos y componentes UI
- **Entity Framework Core**: Acceso a datos
- **JWT**: Autenticaci�n y autorizaci�n
- **SQL Server**: Base de datos relacional
- **ZXing.Net**: Generaci�n de c�digos QR para turnos

## Instrucciones de Uso

### Requisitos
- .NET 8 SDK o superior
- SQL Server (local o remoto)

### Configuraci�n
1. Clonar el repositorio.
2. Configurar la cadena de conexi�n en `appsettings.json` del proyecto `BackEnd/GestionTurnos.BackEnd.API`.
3. Configurar la URL del frontend en la variable `FrontendUrl` en el mismo archivo.
4. Ejecutar las migraciones de la base de datos:
   ```
   dotnet ef database update --project BackEnd/GestionTurnos.BackEnd.Data
   ```
5. Ejecutar el backend:
   ```
   dotnet run --project BackEnd/GestionTurnos.BackEnd.API
   ```
6. Ejecutar el frontend (Razor Pages):
   ```
   dotnet run --project GestionTurnos.FrontEnd.Web
   ```
7. Acceder a la SPA navegando a `/TurnosSpa` o usar la interfaz tradicional.

### Funcionalidades principales
- Registro y login de usuarios.
- Recuperaci�n de contrase�a por email.
- Reserva de turnos con selecci�n de servicio y profesional.
- Visualizaci�n y cancelaci�n de turnos.
- Administraci�n de servicios y profesionales (solo administradores).

## Notas
- Para pruebas de correo, se recomienda usar una herramienta como [Mailtrap](https://mailtrap.io/) o configurar SMTP real.
- El sistema est� pensado para ser extendido f�cilmente con nuevos servicios o integraciones.

---

Para dudas o problemas, contactar al equipo de desarrollo.

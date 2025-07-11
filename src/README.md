# ReservIt! - Sistema de Gestión de Turnos

## Descripción
ReservIt! es un sistema web para la gestión y reserva de turnos para servicios profesionales. Permite a los usuarios registrarse, reservar turnos, recibir confirmaciones y gestionar sus reservas. Los administradores pueden gestionar servicios y profesionales desde una interfaz protegida.

## Escenario
- Usuarios pueden registrarse, iniciar sesión y reservar turnos para distintos servicios.
- Los turnos pueden ser agendados tanto desde la interfaz tradicional (Razor Pages) como desde el SPA (Single Page Application) con Alpine.js.
- Los administradores pueden crear, editar y eliminar servicios y profesionales.
- El sistema envía correos para recuperación de contraseña con enlaces seguros.

## Herramientas y Tecnologías Utilizadas
- **.NET 8 / .NET 9**: Backend y Frontend Razor Pages
- **ASP.NET Core Razor Pages**: Interfaz tradicional
- **Alpine.js**: SPA para agendamiento y administración rápida
- **Bootstrap 5**: Estilos y componentes UI
- **Entity Framework Core**: Acceso a datos
- **JWT**: Autenticación y autorización
- **SQL Server**: Base de datos relacional
- **ZXing.Net**: Generación de códigos QR para turnos

## Instrucciones de Uso

### Requisitos
- .NET 8 SDK o superior
- SQL Server (local o remoto)

### Configuración
1. Clonar el repositorio.
2. Configurar la cadena de conexión en `appsettings.json` del proyecto `BackEnd/GestionTurnos.BackEnd.API`.
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
- Recuperación de contraseña por email.
- Reserva de turnos con selección de servicio y profesional.
- Visualización y cancelación de turnos.
- Administración de servicios y profesionales (solo administradores).

## Notas
- Para pruebas de correo, se recomienda usar una herramienta como [Mailtrap](https://mailtrap.io/) o configurar SMTP real.
- El sistema está pensado para ser extendido fácilmente con nuevos servicios o integraciones.

---

Para dudas o problemas, contactar al equipo de desarrollo.

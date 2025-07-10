using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using GestionTurnos.BackEnd.Data.Contexts;
using GestionTurnos.BackEnd.Service;
using GestionTurnos.BackEnd.ServiceDependencies.Interfaces;
using GestionTurnos.BackEnd.Service.Services;
using GestionTurnos.BackEnd.API.Services; // Agregado para DummyEmailSender
using GestionTurnos.BackEnd.Model.Entities;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailSender, SmtpEmailSender>(); // Cambiado a SmtpEmailSender

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var key = builder.Configuration["Jwt:Key"];
    if (string.IsNullOrEmpty(key))
        throw new Exception("Jwt:Key no configurado en appsettings.json");
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
    };
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

var app = builder.Build();

// Obtener logger
var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("Startup");

// 2. Configurar el pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
// Seed admin user if not exists
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    if (!db.Usuarios.Any(u => u.Rol == Rol.Administrador))
    {
        var authService = new AuthService(config);
        var password = "Admin123!";
        var hash = authService.HashPassword(password, out var salt);
        var admin = new Usuario
        {
            NombreUsuario = "admin",
            Nombre = "Admin",
            Apellido = "Principal",
            Email = "admin@tudominio.com",
            PasswordHash = hash,
            PasswordSalt = salt,
            Rol = Rol.Administrador
        };
        db.Usuarios.Add(admin);
        db.SaveChanges();
        logger.LogInformation("Usuario administrador creado automáticamente.");
    }
}

// Middleware
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
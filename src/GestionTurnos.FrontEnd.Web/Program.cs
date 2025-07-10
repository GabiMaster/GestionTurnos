using GestionTurnos.FrontEnd.Web.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// 1. Agregamos servicios necesarios
builder.Services.AddRazorPages();
builder.Services.AddSession(); 
builder.Services.AddHttpClient<AuthApiService>();
builder.Services.AddHttpContextAccessor(); 
builder.Services.AddHttpClient<ServicioApiService>();
builder.Services.AddHttpClient<TurnoApiService>();
builder.Services.AddHttpClient<ProfesionalApiService>();

var app = builder.Build();

// 2. Configurar el pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // <-- Mover antes del middleware personalizado

// Middleware para poblar HttpContext.User desde el JWT en sesión
app.Use(async (context, next) =>
{
    var session = context.Session;
    var token = session.GetString("JWT");
    if (!string.IsNullOrEmpty(token))
    {
        var handler = new JwtSecurityTokenHandler();
        try
        {
            var jwt = handler.ReadJwtToken(token);
            var claims = jwt.Claims.ToList();
            var identity = new ClaimsIdentity(claims, "jwt");
            context.User = new ClaimsPrincipal(identity);
        }
        catch { /* Token inválido, ignorar */ }
    }
    await next();
});

app.UseAuthorization();

app.MapRazorPages();

app.Run();


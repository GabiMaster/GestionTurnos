using GestionTurnos.FrontEnd.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Agregamos servicios necesarios
builder.Services.AddRazorPages();
builder.Services.AddSession(); 
builder.Services.AddHttpClient<AuthApiService>();
builder.Services.AddHttpContextAccessor(); 
builder.Services.AddHttpClient<ServicioApiService>();
builder.Services.AddHttpClient<TurnoApiService>();

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

app.UseSession(); // ? middleware de sesi�n

app.UseAuthorization();

app.MapRazorPages();

app.Run();


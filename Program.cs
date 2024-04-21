using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SAFIM.Models;
using SAFIM.Jobs;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);

// Configurar servicios
builder.Services.AddControllersWithViews();

//En caso de usar la aplicación dentro de un contenedor se debe cambiar el nombre de la
//cadena de conexion conexionSQL por DockerSQL
builder.Services.AddDbContext<SistemaAsesoriasContext>(opciones =>
      opciones.UseSqlServer(builder.Configuration.GetConnectionString("conexionSQL")));

builder.Services.AddHostedService<ReporteJob>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "SAFIM";
        options.LoginPath = "/Acceso/Index";
        options.Cookie.IsEssential = true;
    });


builder.Services.AddSession(options =>
{
    options.Cookie.Name = "SAFIM";
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Configura el tiempo de expiración de la sesión
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Acceso}/{action=Index}/{id?}");
});

app.Run();
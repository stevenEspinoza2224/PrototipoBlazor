using Blazored.LocalStorage;
using FrontCore.Services;
using FrontCore.Services.IServices;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
//builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();

builder.Services.AddHttpClient();

//Dependencias
builder.Services.AddScoped<IServicioAutenticacion, ServicioAutenticacion>();
//Uso de Local Storage del Navegado
builder.Services.AddBlazoredLocalStorage();
//Servicio Autenticacion y Autorizacion
builder.Services.AddAuthenticationCore();
builder.Services.AddScoped<AuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<AuthStateProvider>());


var app = builder.Build();



app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

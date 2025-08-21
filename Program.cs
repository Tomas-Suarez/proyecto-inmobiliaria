using MySql.Data.MySqlClient;
using proyecto_inmobiliaria.Repository;
using proyecto_inmobiliaria.Services;
using proyecto_inmobiliaria.Mappers;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registramos la conexión para inyección de dependencias
builder.Services.AddTransient<MySqlConnection>(_ =>
    new MySqlConnection(connectionString));

// Registramos repositorios, mappers y servicios
builder.Services.AddScoped<IPropietarioRepository>(sp =>
    new PropietarioRepository(connectionString!));

builder.Services.AddScoped<PropietarioMapper>();

builder.Services.AddScoped<IPropietarioService, PropietarioService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

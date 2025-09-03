using MySql.Data.MySqlClient;
using proyecto_inmobiliaria.Repository;
using proyecto_inmobiliaria.Repository.imp;
using proyecto_inmobiliaria.Services;
using proyecto_inmobiliaria.Services.imp;
using proyecto_inmobiliaria.Mappers;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// TODO: ¿Usar reflexion para no repetir tanto codigo?
// Todas las interfaces comienzan con I{nombre}...

// TODO: Usar IConfig

// Inyección de dependencias
builder.Services.AddTransient<MySqlConnection>(_ =>
    new MySqlConnection(connectionString));

// Propietario
builder.Services.AddScoped<IPropietarioRepository>(sp =>
    new PropietarioRepository(connectionString!));
builder.Services.AddScoped<PropietarioMapper>();
builder.Services.AddScoped<IPropietarioService, PropietarioService>();

// Inquilino
builder.Services.AddScoped<IInquilinoRepository>(sp =>
    new InquilinoRepository(connectionString!));
builder.Services.AddScoped<InquilinoMapper>();
builder.Services.AddScoped<IInquilinoService, InquilinoService>();

// Inmueble
builder.Services.AddScoped<IInmuebleRepository>(sp =>
    new InmuebleRepository(connectionString!));
builder.Services.AddScoped<InmuebleMapper>();
builder.Services.AddScoped<IInmuebleService, InmuebleService>();

// EstadoInmueble
builder.Services.AddScoped<IEstadoInmuebleRepository>(sp =>
    new EstadoInmuebleRepository(connectionString!));

// TipoInmueble
builder.Services.AddScoped<ITipoInmuebleRepository>(sp =>
    new TipoInmuebleRepository(connectionString!));

// Contrato
builder.Services.AddScoped<IContratoRepository>(sp =>
    new ContratoRepository(connectionString!));
builder.Services.AddScoped<ContratoMapper>();
builder.Services.AddScoped<IContratoService, ContratoService>();

builder.Services.AddControllersWithViews()
    .AddSessionStateTempDataProvider();

builder.Services.AddSession();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.UseExceptionHandler("/Error");
app.UseStatusCodePagesWithReExecute("/Error", "?statusCode={0}");

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

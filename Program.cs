using MySql.Data.MySqlClient;
using proyecto_inmobiliaria.Repository;
using proyecto_inmobiliaria.Repository.imp;
using proyecto_inmobiliaria.Services;
using proyecto_inmobiliaria.Services.imp;
using proyecto_inmobiliaria.Mappers;
using proyecto_inmobiliaria.Config;

var builder = WebApplication.CreateBuilder(args);

/* TODO: ¿Usar reflexion para no repetir tanto codigo?
Todas las interfaces comienzan con I{nombre}...
*/
builder.Services.AddSingleton<IAppConfig, AppConfig>();

builder.Services.AddTransient<MySqlConnection>(sp =>
    new MySqlConnection(sp.GetRequiredService<IAppConfig>().ConnectionString));

//Repository
builder.Services.AddScoped<IPropietarioRepository, PropietarioRepository>();
builder.Services.AddScoped<IInquilinoRepository, InquilinoRepository>();
builder.Services.AddScoped<IInmuebleRepository, InmuebleRepository>();
builder.Services.AddScoped<IEstadoInmuebleRepository, EstadoInmuebleRepository>();
builder.Services.AddScoped<ITipoInmuebleRepository, TipoInmuebleRepository>();
builder.Services.AddScoped<IContratoRepository, ContratoRepository>();

//Mapper
builder.Services.AddSingleton<PropietarioMapper>();
builder.Services.AddSingleton<InquilinoMapper>();
builder.Services.AddSingleton<InmuebleMapper>();
builder.Services.AddSingleton<ContratoMapper>();

//Service
builder.Services.AddScoped<IPropietarioService, PropietarioService>();
builder.Services.AddScoped<IInquilinoService, InquilinoService>();
builder.Services.AddScoped<IInmuebleService, InmuebleService>();
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

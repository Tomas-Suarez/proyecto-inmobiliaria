using MySql.Data.MySqlClient;
using proyecto_inmobiliaria.Repository;
using proyecto_inmobiliaria.Repository.imp;
using proyecto_inmobiliaria.Services;
using proyecto_inmobiliaria.Services.imp;
using proyecto_inmobiliaria.Mappers;
using proyecto_inmobiliaria.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using proyecto_inmobiliaria.Jwt;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IAppConfig, AppConfig>();

builder.Services.AddTransient<MySqlConnection>(sp =>
    new MySqlConnection(sp.GetRequiredService<IAppConfig>().ConnectionString));

// Repository
builder.Services.AddScoped<IPropietarioRepository, PropietarioRepository>();
builder.Services.AddScoped<IInquilinoRepository, InquilinoRepository>();
builder.Services.AddScoped<IInmuebleRepository, InmuebleRepository>();
builder.Services.AddScoped<IEstadoInmuebleRepository, EstadoInmuebleRepository>();
builder.Services.AddScoped<ITipoInmuebleRepository, TipoInmuebleRepository>();
builder.Services.AddScoped<IContratoRepository, ContratoRepository>();
builder.Services.AddScoped<IPagoRepository, PagoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IAuditoriaContratoRepository, AuditoriaContratoRepository>();


// Mapper
builder.Services.AddSingleton<PropietarioMapper>();
builder.Services.AddSingleton<InquilinoMapper>();
builder.Services.AddSingleton<InmuebleMapper>();
builder.Services.AddSingleton<ContratoMapper>();
builder.Services.AddSingleton<PagoMapper>();
builder.Services.AddSingleton<UsuarioMapper>();
builder.Services.AddSingleton<AuditoriaContratoMapper>();


// Service
builder.Services.AddScoped<IPropietarioService, PropietarioService>();
builder.Services.AddScoped<IInquilinoService, InquilinoService>();
builder.Services.AddScoped<IInmuebleService, InmuebleService>();
builder.Services.AddScoped<IContratoService, ContratoService>();
builder.Services.AddScoped<IPagoService, PagoService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IAuditoriaContratoService, AuditoriaContratoService>();


builder.Services.AddSingleton<JwtTokenGenerator>();

builder.Services.AddControllersWithViews()
    .AddSessionStateTempDataProvider();

builder.Services.AddSession();

// JWT Config
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!))
    };

    // Leer token desde cookie
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.ContainsKey("token"))
            {
                context.Token = context.Request.Cookies["token"];
            }
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            context.HandleResponse();
            context.Response.Redirect("/Usuario/Login");
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler("/Error");
app.UseStatusCodePagesWithReExecute("/Error", "?statusCode={0}");

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

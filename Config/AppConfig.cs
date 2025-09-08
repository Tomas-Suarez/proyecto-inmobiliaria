namespace proyecto_inmobiliaria.Config
{
    public class AppConfig : IAppConfig
    {
        private readonly IConfiguration _configuration;
        public AppConfig(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ConnectionString => _configuration.GetConnectionString("DefaultConnection")!;
    }
}
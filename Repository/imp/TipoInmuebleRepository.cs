using MySql.Data.MySqlClient;
using proyecto_inmobiliaria.Config;
using proyecto_inmobiliaria.Models;

namespace proyecto_inmobiliaria.Repository.imp
{
    public class TipoInmuebleRepository : ITipoInmuebleRepository
    {
        private readonly string _connectionString;

        public TipoInmuebleRepository(IAppConfig config)
        {
            _connectionString = config.ConnectionString;
        }

        public IList<TipoInmueble> ObtenerTipoInmueble()
        {
            var tipos = new List<TipoInmueble>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"SELECT id_tipo_inmueble, nombre FROM tipo_inmueble";

                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tipos.Add(new TipoInmueble
                            {
                                IdTipoInmueble = reader.GetInt32("id_tipo_inmueble"),
                                Nombre = reader.GetString("nombre")
                            });
                        }
                    }
                }
            }

            return tipos;
        }
    }
}

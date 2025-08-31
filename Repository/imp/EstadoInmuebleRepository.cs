using MySql.Data.MySqlClient;
using proyecto_inmobiliaria.Models;

namespace proyecto_inmobiliaria.Repository.imp
{
    public class EstadoInmuebleRepository : IEstadoInmuebleRepository
    {
        private readonly string _connectionString;

        public EstadoInmuebleRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IList<EstadoInmueble> ObtenerEstadoInmueble()
        {
            var estados = new List<EstadoInmueble>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"SELECT id_estado_inmueble, descripcion FROM estado_inmueble";

                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            estados.Add(new EstadoInmueble
                            {
                                IdEstadoInmueble = reader.GetInt32("id_estado_inmueble"),
                                Descripcion = reader.GetString("descripcion")
                            });
                        }
                    }
                }
            }

            return estados;
        }
    }
}

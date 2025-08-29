using MySql.Data.MySqlClient;
using proyecto_inmobiliaria.Dtos.response;
using proyecto_inmobiliaria.Exceptions;
using proyecto_inmobiliaria.Models;

using static proyecto_inmobiliaria.Constants.InmuebleConstants;


namespace proyecto_inmobiliaria.Repository.imp
{
    public class InmuebleRepository : IInmuebleRepository
    {
        private readonly string _connectionString;

        public InmuebleRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Inmueble Alta(Inmueble inmueble)
        {
            int idInmueble;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"INSERT INTO Inmueble
                                (id_estado_inmueble, id_tipo_inmueble, id_propietario, direccion, cantidad_ambientes, superficie_m2)
                                VALUES (@idEstadoInmueble, @idTipoInmueble, @idPropietario, @direccion, @cantidadAmbientes, @superficieM2);";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idEstadoInmueble", inmueble.IdEstadoInmueble);
                    command.Parameters.AddWithValue("@idTipoInmueble", inmueble.IdTipoInmueble);
                    command.Parameters.AddWithValue("@idPropietario", inmueble.IdPropietario);
                    command.Parameters.AddWithValue("@direccion", inmueble.Direccion);
                    command.Parameters.AddWithValue("@cantidadAmbientes", inmueble.CantidadAmbientes);
                    command.Parameters.AddWithValue("@superficieM2", inmueble.SuperficieM2);

                    command.ExecuteNonQuery();
                    idInmueble = (int)command.LastInsertedId;
                }
            }

            inmueble.IdInmueble = idInmueble;
            return inmueble;
        }

        public int Baja(int idInmueble)
        {
            int filasAfectadas;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"DELETE FROM Inmueble WHERE id_inmueble = @idInmueble;";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idInmueble", idInmueble);
                    filasAfectadas = command.ExecuteNonQuery();
                }
            }
            return filasAfectadas;
        }

        public Inmueble Modificar(Inmueble inmueble)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"UPDATE Inmueble
                                SET id_estado_inmueble = @idEstadoInmueble,
                                    id_tipo_inmueble = @idTipoInmueble,
                                    id_propietario = @idPropietario,
                                    direccion = @direccion,
                                    cantidad_ambientes = @cantidadAmbientes,
                                    superficie_m2 = @superficieM2
                                WHERE id_inmueble = @idInmueble;";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idInmueble", inmueble.IdInmueble);
                    command.Parameters.AddWithValue("@idEstadoInmueble", inmueble.IdEstadoInmueble);
                    command.Parameters.AddWithValue("@idTipoInmueble", inmueble.IdTipoInmueble);
                    command.Parameters.AddWithValue("@idPropietario", inmueble.IdPropietario);
                    command.Parameters.AddWithValue("@direccion", inmueble.Direccion);
                    command.Parameters.AddWithValue("@cantidadAmbientes", inmueble.CantidadAmbientes);
                    command.Parameters.AddWithValue("@superficieM2", inmueble.SuperficieM2);

                    command.ExecuteNonQuery();
                }
            }
            return inmueble;
        }

        public IList<InmuebleResponseDTO> ObtenerLista(int paginaNro, int tamPagina)
        {
            IList<InmuebleResponseDTO> inmuebles = new List<InmuebleResponseDTO>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"SELECT i.id_Inmueble, 
                                t.nombre AS TipoInmueble, 
                                CONCAT(p.nombre, ' ', p.apellido) AS NombreCompletoPropietario, 
                                i.direccion, 
                                i.cantidad_Ambientes, 
                                i.superficie_M2
                         FROM Inmueble i
                         JOIN Tipo_Inmueble t ON i.id_Tipo_Inmueble = t.id_Tipo_Inmueble
                         JOIN Propietario pr ON i.id_Propietario = pr.id_Propietario
                         JOIN Persona p ON pr.id_Persona = p.id_Persona
                         LIMIT @PageSize OFFSET @Offset;";

                using (var command = new MySqlCommand(query, connection))
                {
                    int offset = (paginaNro - 1) * tamPagina;

                    command.Parameters.AddWithValue("@PageSize", tamPagina);
                    command.Parameters.AddWithValue("@Offset", offset);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var dto = new InmuebleResponseDTO(
                                reader.GetInt32("id_Inmueble"),
                                reader.GetString("TipoInmueble"),
                                reader.GetString("NombreCompletoPropietario"),
                                reader.GetString("direccion"),
                                reader.GetInt32("cantidad_Ambientes"),
                                reader.GetDecimal("superficie_M2")
                            );
                            inmuebles.Add(dto);
                        }
                    }
                }
            }

            return inmuebles;
        }

        public InmuebleResponseDTO ObtenerPorId(int idInmueble)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"SELECT i.id_Inmueble, 
                                    t.nombre AS TipoInmueble, 
                                    CONCAT(p.nombre, ' ', p.apellido) AS NombreCompletoPropietario, 
                                    i.direccion, 
                                    i.cantidad_Ambientes, 
                                    i.superficie_M2
                                FROM Inmueble i
                                JOIN Tipo_Inmueble t ON i.id_Tipo_Inmueble = t.id_Tipo_Inmueble
                                JOIN Propietario pr ON i.id_Propietario = pr.id_Propietario
                                JOIN Persona p ON pr.id_Persona = p.id_Persona
                                 WHERE i.id_Inmueble = @idInmueble;";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idInmueble", idInmueble);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new InmuebleResponseDTO(
                                reader.GetInt32("id_Inmueble"),
                                reader.GetString("TipoInmueble"),
                                reader.GetString("NombreCompletoPropietario"),
                                reader.GetString("direccion"),
                                reader.GetInt32("cantidad_Ambientes"),
                                reader.GetDecimal("superficie_M2")
                            );
                        }
                        else
                        {
                            throw new NotFoundException(NO_SE_ENCONTRO_INMUEBLE_POR_ID + idInmueble);
                        }
                    }

                }
            }
        }
    }
}
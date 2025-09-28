using MySql.Data.MySqlClient;
using proyecto_inmobiliaria.Config;
using proyecto_inmobiliaria.Dtos.response;
using proyecto_inmobiliaria.Exceptions;
using proyecto_inmobiliaria.Models;

using static proyecto_inmobiliaria.Constants.InmuebleConstants;


namespace proyecto_inmobiliaria.Repository.imp
{
    public class InmuebleRepository : IInmuebleRepository
    {
        private readonly string _connectionString;

        public InmuebleRepository(IAppConfig config)
        {
            _connectionString = config.ConnectionString;
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

        public int CantidadTotal(int? estado = null)
        {
            string query = @"SELECT COUNT(*) 
                            FROM Inmueble
                            WHERE (@estado IS NULL OR id_Estado_Inmueble = @estado);";

            using (var connection = new MySqlConnection(_connectionString))
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@estado", (object?)estado ?? DBNull.Value);
                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public int CantidadTotalDisponiblesPorFecha(DateTime fechaDesde, DateTime fechaHasta)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"
            SELECT COUNT(*)
            FROM Inmueble i
            WHERE NOT EXISTS (
                SELECT 1
                FROM Contrato c
                WHERE c.id_inmueble = i.id_inmueble
                  AND (
                        (@fechaDesde BETWEEN c.fecha_desde AND IFNULL(c.fecha_fin_anticipada, c.fecha_hasta))
                     OR (@fechaHasta BETWEEN c.fecha_desde AND IFNULL(c.fecha_fin_anticipada, c.fecha_hasta))
                     OR (c.fecha_desde BETWEEN @fechaDesde AND @fechaHasta)
                     OR (IFNULL(c.fecha_fin_anticipada, c.fecha_hasta) BETWEEN @fechaDesde AND @fechaHasta)
                  )
            );";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@fechaDesde", fechaDesde);
                    command.Parameters.AddWithValue("@fechaHasta", fechaHasta);

                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public int CantidadTotalPorPropietario(int idPropietario)
        {
            string query = @"SELECT COUNT(*) 
                     FROM Inmueble 
                     WHERE id_propietario = @idPropietario;";

            using (var connection = new MySqlConnection(_connectionString))
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@idPropietario", idPropietario);
                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar());
            }
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

        public void ModificarEstadoInmueble(int idInmueble, int estadoInmueble)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = @"UPDATE Inmueble
                     SET id_estado_inmueble = @estado
                     WHERE id_inmueble = @idInmueble;";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@estado", estadoInmueble);
            command.Parameters.AddWithValue("@idInmueble", idInmueble);

            int filasAfectadas = command.ExecuteNonQuery();

            if (filasAfectadas == 0)
            {
                throw new NotFoundException(NO_SE_ENCONTRO_INMUEBLE_POR_ID + idInmueble);
            }
        }

        public IList<InmuebleResponseDTO> ObtenerDireccionFiltro(string direccion)
        {
            var inmuebles = new List<InmuebleResponseDTO>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"
            SELECT i.id_Inmueble, 
                   t.nombre AS TipoInmueble, 
                   e.descripcion AS EstadoInmueble,
                   CONCAT(p.nombre, ' ', p.apellido) AS NombreCompletoPropietario, 
                   i.direccion, 
                   i.cantidad_Ambientes, 
                   i.superficie_M2
            FROM Inmueble i
            JOIN Tipo_Inmueble t ON i.id_Tipo_Inmueble = t.id_Tipo_Inmueble
            JOIN Estado_Inmueble e ON i.id_Estado_Inmueble = e.id_Estado_Inmueble
            JOIN Propietario pr ON i.id_Propietario = pr.id_Propietario
            JOIN Persona p ON pr.id_Persona = p.id_Persona
            WHERE i.direccion LIKE CONCAT('%', @direccion, '%')
              AND i.id_Estado_Inmueble = 1
            ORDER BY i.direccion;";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@direccion", direccion);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            inmuebles.Add(new InmuebleResponseDTO(
                                reader.GetInt32("id_Inmueble"),
                                reader.GetString("TipoInmueble"),
                                reader.GetString("EstadoInmueble"),
                                reader.GetString("NombreCompletoPropietario"),
                                reader.GetString("direccion"),
                                reader.GetInt32("cantidad_Ambientes"),
                                reader.GetDecimal("superficie_M2")
                            ));
                        }
                    }
                }
            }

            return inmuebles;
        }

        public IList<InmuebleResponseDTO> ObtenerDisponiblesPorFecha(DateTime fechaDesde, DateTime fechaHasta, int paginaNro, int tamPagina)
        {
            var inmuebles = new List<InmuebleResponseDTO>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"
            SELECT i.id_inmueble,
                   t.nombre AS TipoInmueble,
                   e.descripcion AS EstadoInmueble,
                   CONCAT(p.nombre, ' ', p.apellido) AS NombreCompletoPropietario,
                   i.direccion,
                   i.cantidad_ambientes,
                   i.superficie_m2
            FROM Inmueble i
            JOIN Tipo_Inmueble t ON i.id_tipo_inmueble = t.id_tipo_inmueble
            JOIN Estado_Inmueble e ON i.id_estado_inmueble = e.id_estado_inmueble
            JOIN Propietario pr ON i.id_propietario = pr.id_propietario
            JOIN Persona p ON pr.id_persona = p.id_persona
            WHERE NOT EXISTS (
                SELECT 1
                FROM Contrato c
                WHERE c.id_inmueble = i.id_inmueble
                  AND (
                        (@fechaDesde BETWEEN c.fecha_desde AND IFNULL(c.fecha_fin_anticipada, c.fecha_hasta))
                     OR (@fechaHasta BETWEEN c.fecha_desde AND IFNULL(c.fecha_fin_anticipada, c.fecha_hasta))
                     OR (c.fecha_desde BETWEEN @fechaDesde AND @fechaHasta)
                     OR (IFNULL(c.fecha_fin_anticipada, c.fecha_hasta) BETWEEN @fechaDesde AND @fechaHasta)
                  )
            )
            ORDER BY i.id_inmueble
            LIMIT @PageSize OFFSET @Offset;";

                using (var command = new MySqlCommand(query, connection))
                {
                    int offset = (paginaNro - 1) * tamPagina;

                    command.Parameters.AddWithValue("@fechaDesde", fechaDesde);
                    command.Parameters.AddWithValue("@fechaHasta", fechaHasta);
                    command.Parameters.AddWithValue("@PageSize", tamPagina);
                    command.Parameters.AddWithValue("@Offset", offset);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            inmuebles.Add(new InmuebleResponseDTO(
                                reader.GetInt32("id_inmueble"),
                                reader.GetString("TipoInmueble"),
                                reader.GetString("EstadoInmueble"),
                                reader.GetString("NombreCompletoPropietario"),
                                reader.GetString("direccion"),
                                reader.GetInt32("cantidad_ambientes"),
                                reader.GetDecimal("superficie_m2")
                            ));
                        }
                    }
                }
            }

            return inmuebles;
        }

        public IList<InmuebleResponseDTO> ObtenerInmueblesPorPropietario(int idPropietario, int paginaNro, int tamPagina)
        {
            var inmuebles = new List<InmuebleResponseDTO>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"
            SELECT i.id_Inmueble, 
                   t.nombre AS TipoInmueble, 
                   e.descripcion AS EstadoInmueble,
                   CONCAT(p.nombre, ' ', p.apellido) AS NombreCompletoPropietario, 
                   i.direccion, 
                   i.cantidad_Ambientes, 
                   i.superficie_M2
            FROM Inmueble i
            JOIN Tipo_Inmueble t ON i.id_Tipo_Inmueble = t.id_Tipo_Inmueble
            JOIN Estado_Inmueble e ON i.id_Estado_Inmueble = e.id_Estado_Inmueble
            JOIN Propietario pr ON i.id_Propietario = pr.id_Propietario
            JOIN Persona p ON pr.id_Persona = p.id_Persona
            WHERE i.id_Propietario = @idPropietario
            LIMIT @PageSize OFFSET @Offset;";

                using (var command = new MySqlCommand(query, connection))
                {
                    int offset = (paginaNro - 1) * tamPagina;

                    command.Parameters.AddWithValue("@idPropietario", idPropietario);
                    command.Parameters.AddWithValue("@PageSize", tamPagina);
                    command.Parameters.AddWithValue("@Offset", offset);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            inmuebles.Add(new InmuebleResponseDTO(
                                reader.GetInt32("id_Inmueble"),
                                reader.GetString("TipoInmueble"),
                                reader.GetString("EstadoInmueble"),
                                reader.GetString("NombreCompletoPropietario"),
                                reader.GetString("direccion"),
                                reader.GetInt32("cantidad_Ambientes"),
                                reader.GetDecimal("superficie_M2")
                            ));
                        }
                    }
                }
            }
            return inmuebles;
        }


        public IList<InmuebleResponseDTO> ObtenerLista(int paginaNro, int tamPagina, int? idEstado = null)
        {
            var inmuebles = new List<InmuebleResponseDTO>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"
            SELECT i.id_Inmueble, 
                   t.nombre AS TipoInmueble, 
                   e.descripcion AS EstadoInmueble,
                   CONCAT(p.nombre, ' ', p.apellido) AS NombreCompletoPropietario, 
                   i.direccion, 
                   i.cantidad_Ambientes, 
                   i.superficie_M2
            FROM Inmueble i
            JOIN Tipo_Inmueble t ON i.id_Tipo_Inmueble = t.id_Tipo_Inmueble
            JOIN Estado_Inmueble e ON i.id_Estado_Inmueble = e.id_Estado_Inmueble
            JOIN Propietario pr ON i.id_Propietario = pr.id_Propietario
            JOIN Persona p ON pr.id_Persona = p.id_Persona
            WHERE (@idEstado IS NULL OR i.id_Estado_Inmueble = @idEstado)
            LIMIT @PageSize OFFSET @Offset;";

                using (var command = new MySqlCommand(query, connection))
                {
                    int offset = (paginaNro - 1) * tamPagina;

                    command.Parameters.AddWithValue("@PageSize", tamPagina);
                    command.Parameters.AddWithValue("@Offset", offset);
                    command.Parameters.AddWithValue("@idEstado", (object?)idEstado ?? DBNull.Value);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            inmuebles.Add(new InmuebleResponseDTO(
                                reader.GetInt32("id_Inmueble"),
                                reader.GetString("TipoInmueble"),
                                reader.GetString("EstadoInmueble"),
                                reader.GetString("NombreCompletoPropietario"),
                                reader.GetString("direccion"),
                                reader.GetInt32("cantidad_Ambientes"),
                                reader.GetDecimal("superficie_M2")
                            ));
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
                                    e.descripcion AS EstadoInmueble,
                                    CONCAT(p.nombre, ' ', p.apellido) AS NombreCompletoPropietario, 
                                    i.direccion, 
                                    i.cantidad_Ambientes, 
                                    i.superficie_M2
                                FROM Inmueble i
                                JOIN Tipo_Inmueble t ON i.id_Tipo_Inmueble = t.id_Tipo_Inmueble
                                JOIN Estado_Inmueble e ON i.id_Estado_Inmueble = e.id_Estado_Inmueble
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
                                reader.GetString("EstadoInmueble"),
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

        public Inmueble ObtenerPorIdRequest(int idInmueble)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"SELECT i.id_inmueble,
                                i.id_estado_inmueble,
                                i.id_tipo_inmueble,
                                i.id_propietario,
                                i.direccion,
                                i.cantidad_ambientes,
                                i.superficie_m2
                         FROM Inmueble i
                         WHERE i.id_inmueble = @idInmueble;";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idInmueble", idInmueble);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Inmueble
                            {
                                IdInmueble = reader.GetInt32("id_inmueble"),
                                IdEstadoInmueble = reader.GetInt32("id_estado_inmueble"),
                                IdTipoInmueble = reader.GetInt32("id_tipo_inmueble"),
                                IdPropietario = reader.GetInt32("id_propietario"),
                                Direccion = reader.GetString("direccion"),
                                CantidadAmbientes = reader.GetInt32("cantidad_ambientes"),
                                SuperficieM2 = reader.GetDecimal("superficie_m2")
                            };
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
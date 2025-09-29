using MySql.Data.MySqlClient;
using proyecto_inmobiliaria.Config;
using proyecto_inmobiliaria.Dtos.response;
using proyecto_inmobiliaria.Exceptions;
using proyecto_inmobiliaria.Models;

using static proyecto_inmobiliaria.Constants.ContratoConstants;

namespace proyecto_inmobiliaria.Repository.imp
{
    public class ContratoRepository : IContratoRepository
    {
        private readonly string _connectionString;

        public ContratoRepository(IAppConfig config)
        {
            _connectionString = config.ConnectionString;
        }

        public Contrato Alta(Contrato contrato)
        {
            int idContrato;

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"INSERT INTO Contrato
                                (id_inquilino, id_inmueble, monto, fecha_desde, fecha_hasta, fecha_fin_anticipada, finalizado)
                                VALUES (@idInquilino, @idInmueble, @monto, @fechaDesde, @fechaHasta, @FechaFinAnticipada, @finalizado);";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idInquilino", contrato.IdInquilino);
                    command.Parameters.AddWithValue("@idInmueble", contrato.IdInmueble);
                    command.Parameters.AddWithValue("@monto", contrato.Monto);
                    command.Parameters.AddWithValue("@fechaDesde", contrato.FechaDesde);
                    command.Parameters.AddWithValue("@fechaHasta", contrato.FechaHasta);
                    command.Parameters.AddWithValue("@fechaFinAnticipada", contrato.FechaFinAnticipada);
                    command.Parameters.AddWithValue("@finalizado", contrato.Finalizado);

                    command.ExecuteNonQuery();
                    idContrato = (int)command.LastInsertedId;
                }
            }
            contrato.IdContrato = idContrato;
            return contrato;
        }

        public int Baja(int idContrato)
        {
            int filasAfectadas;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"DELETE FROM Contrato WHERE id_contrato = @idContrato;";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idContrato", idContrato);
                    filasAfectadas = command.ExecuteNonQuery();
                }
            }
            return filasAfectadas;
        }

        public int CantidadContratosVigentesPorFecha(DateTime fechaDesde, DateTime fechaHasta)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = @"
                SELECT COUNT(*)
                FROM contrato c
                WHERE c.fecha_desde <= @fechaHasta
                    AND IFNULL(c.fecha_fin_anticipada, c.fecha_hasta) >= @fechaDesde
                    AND c.finalizado = 0;";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@fechaDesde", fechaDesde);
            command.Parameters.AddWithValue("@fechaHasta", fechaHasta);

            return Convert.ToInt32(command.ExecuteScalar());
        }


        public int CantidadTotal()
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            string query = "SELECT COUNT(*) FROM Contrato;";
            using var command = new MySqlCommand(query, connection);
            return Convert.ToInt32(command.ExecuteScalar());
        }

        public int CantidadTotalPorInmueble(int idInmueble)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = "SELECT COUNT(*) FROM contrato WHERE id_inmueble = @idInmueble;";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@idInmueble", idInmueble);

            return Convert.ToInt32(command.ExecuteScalar());
        }

        public bool ExisteSuperposicion(
            int idInmueble,
            DateTime fechaDesde,
            DateTime fechaHasta,
            int? idContratoExcluir = null)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = @"
                    SELECT COUNT(*) 
                    FROM contrato
                    WHERE id_inmueble = @idInmueble
                        AND (
                            (@fechaDesde BETWEEN fecha_desde AND IFNULL(fecha_fin_anticipada, fecha_hasta))
                            OR (@fechaHasta BETWEEN fecha_desde AND IFNULL(fecha_fin_anticipada, fecha_hasta))
                            OR (fecha_desde BETWEEN @fechaDesde AND @fechaHasta)
                            OR (IFNULL(fecha_fin_anticipada, fecha_hasta) BETWEEN @fechaDesde AND @fechaHasta)
                        )";

            if (idContratoExcluir.HasValue)
                query += " AND id_contrato <> @idContratoExcluir";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@idInmueble", idInmueble);
            command.Parameters.AddWithValue("@fechaDesde", fechaDesde);
            command.Parameters.AddWithValue("@fechaHasta", fechaHasta);
            if (idContratoExcluir.HasValue)
                command.Parameters.AddWithValue("@idContratoExcluir", idContratoExcluir.Value);

            int count = Convert.ToInt32(command.ExecuteScalar());
            return count > 0;
        }


        public Contrato Modificar(Contrato contrato)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"UPDATE Contrato
                                SET monto = @monto,
                                    fecha_desde = @fechaDesde,
                                    fecha_hasta = @fechaHasta,
                                    fecha_fin_anticipada = @fechaFinAnticipacion,
                                    finalizado = @finalizado
                                WHERE id_contrato = @idContrato;";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idContrato", contrato.IdContrato);
                    command.Parameters.AddWithValue("@idInquilino", contrato.IdInquilino);
                    command.Parameters.AddWithValue("@idInmueble", contrato.IdInmueble);
                    command.Parameters.AddWithValue("@monto", contrato.Monto);
                    command.Parameters.AddWithValue("@fechaDesde", contrato.FechaDesde);
                    command.Parameters.AddWithValue("@fechaHasta", contrato.FechaHasta);
                    command.Parameters.AddWithValue("@fechaFinAnticipacion", contrato.FechaFinAnticipada);
                    command.Parameters.AddWithValue("@finalizado", contrato.Finalizado);

                    command.ExecuteNonQuery();
                }
            }
            return contrato;
        }

        public IList<ContratoResponseDTO> ObtenerContratosPorInmueble(int idInmueble, int paginaNro, int tamPagina)
        {
            IList<ContratoResponseDTO> contratos = new List<ContratoResponseDTO>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"SELECT 
                            c.id_contrato AS IdContrato,
                            i.direccion AS DireccionInmueble,
                            ti.nombre AS TipoInmueble,
                            CONCAT(pi.nombre, ' ', pi.apellido) AS NombreCompletoInquilino,
                            c.monto AS Monto,
                            c.fecha_desde AS FechaDesde,
                            c.fecha_hasta AS FechaHasta,
                            c.fecha_fin_anticipada AS FechaFinAnticipada,
                            c.finalizado AS Finalizado
                        FROM contrato c
                        JOIN inquilino inq ON c.id_inquilino = inq.id_inquilino
                        JOIN persona pi ON inq.id_persona = pi.id_persona
                        JOIN inmueble i ON c.id_inmueble = i.id_inmueble
                        JOIN tipo_inmueble ti ON i.id_tipo_inmueble = ti.id_tipo_inmueble
                        WHERE c.id_inmueble = @idInmueble
                        ORDER BY c.fecha_desde DESC
                        LIMIT @PageSize OFFSET @Offset;";

                using (var command = new MySqlCommand(query, connection))
                {
                    int offset = (paginaNro - 1) * tamPagina;

                    command.Parameters.AddWithValue("@idInmueble", idInmueble);
                    command.Parameters.AddWithValue("@PageSize", tamPagina);
                    command.Parameters.AddWithValue("@Offset", offset);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime? fechaFinAnticipada = reader.IsDBNull(reader.GetOrdinal("FechaFinAnticipada"))
                                ? null
                                : reader.GetDateTime("FechaFinAnticipada");

                            var dto = new ContratoResponseDTO(
                                reader.GetInt32("IdContrato"),
                                reader.GetString("DireccionInmueble"),
                                reader.GetString("TipoInmueble"),
                                reader.GetString("NombreCompletoInquilino"),
                                reader.GetDecimal("Monto"),
                                reader.GetDateTime("FechaDesde"),
                                reader.GetDateTime("FechaHasta"),
                                fechaFinAnticipada,
                                reader.GetBoolean("Finalizado")
                            );
                            contratos.Add(dto);
                        }
                    }
                }
            }

            return contratos;
        }

        public IList<ContratoResponseDTO> ObtenerContratosVigentesPorFecha(DateTime fechaDesde, DateTime fechaHasta, int paginaNro, int tamPagina)
        {
            IList<ContratoResponseDTO> contratos = new List<ContratoResponseDTO>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"
                    SELECT 
                    c.id_contrato AS IdContrato,
                    i.direccion AS DireccionInmueble,
                    ti.nombre AS TipoInmueble,
                    CONCAT(pi.nombre, ' ', pi.apellido) AS NombreCompletoInquilino,
                    c.monto AS Monto,
                    c.fecha_desde AS FechaDesde,
                    c.fecha_hasta AS FechaHasta,
                    c.fecha_fin_anticipada AS FechaFinAnticipada,
                    c.finalizado AS Finalizado
                FROM contrato c
                JOIN inquilino inq ON c.id_inquilino = inq.id_inquilino
                JOIN persona pi ON inq.id_persona = pi.id_persona
                JOIN inmueble i ON c.id_inmueble = i.id_inmueble
                JOIN tipo_inmueble ti ON i.id_tipo_inmueble = ti.id_tipo_inmueble
                WHERE c.fecha_desde <= @fechaHasta
                    AND IFNULL(c.fecha_fin_anticipada, c.fecha_hasta) >= @fechaDesde
                    AND c.finalizado = 0
                ORDER BY c.fecha_desde DESC
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
                            DateTime? fechaFinAnticipada = reader.IsDBNull(reader.GetOrdinal("FechaFinAnticipada"))
                                ? null
                                : reader.GetDateTime("FechaFinAnticipada");

                            var dto = new ContratoResponseDTO(
                                reader.GetInt32("IdContrato"),
                                reader.GetString("DireccionInmueble"),
                                reader.GetString("TipoInmueble"),
                                reader.GetString("NombreCompletoInquilino"),
                                reader.GetDecimal("Monto"),
                                reader.GetDateTime("FechaDesde"),
                                reader.GetDateTime("FechaHasta"),
                                fechaFinAnticipada,
                                reader.GetBoolean("Finalizado")
                            );
                            contratos.Add(dto);
                        }
                    }
                }
            }

            return contratos;
        }


        public IList<ContratoResponseDTO> ObtenerLista(int paginaNro, int tamPagina)
        {
            IList<ContratoResponseDTO> contratos = new List<ContratoResponseDTO>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"SELECT 
                            c.id_contrato AS IdContrato,
                            i.direccion AS DireccionInmueble,
                            ti.nombre AS TipoInmueble,
                            CONCAT(pi.nombre, ' ', pi.apellido) AS NombreCompletoInquilino,
                            c.monto AS Monto,
                            c.fecha_desde AS FechaDesde,
                            c.fecha_hasta AS FechaHasta,
                            c.fecha_fin_anticipada AS FechaFinAnticipada,
                            c.finalizado AS Finalizado
                        FROM contrato c
                        JOIN inquilino inq ON c.id_inquilino = inq.id_inquilino
                        JOIN persona pi ON inq.id_persona = pi.id_persona
                        JOIN inmueble i ON c.id_inmueble = i.id_inmueble
                        JOIN tipo_inmueble ti ON i.id_tipo_inmueble = ti.id_tipo_inmueble
                        ORDER BY c.fecha_desde DESC
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
                            DateTime? fechaFinAnticipada = reader.IsDBNull(reader.GetOrdinal("FechaFinAnticipada"))
                                ? null
                                : reader.GetDateTime("FechaFinAnticipada");

                            var dto = new ContratoResponseDTO(
                                reader.GetInt32("IdContrato"),
                                reader.GetString("DireccionInmueble"),
                                reader.GetString("TipoInmueble"),
                                reader.GetString("NombreCompletoInquilino"),
                                reader.GetDecimal("Monto"),
                                reader.GetDateTime("FechaDesde"),
                                reader.GetDateTime("FechaHasta"),
                                fechaFinAnticipada,
                                reader.GetBoolean("Finalizado")
                            );
                            contratos.Add(dto);
                        }
                    }
                }
            }

            return contratos;
        }

        public ContratoResponseDTO ObtenerPorId(int idContrato)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"SELECT 
                            c.id_contrato AS IdContrato,
                            i.direccion AS DireccionInmueble,
                            ti.nombre AS TipoInmueble,
                            CONCAT(pi.nombre, ' ', pi.apellido) AS NombreCompletoInquilino,
                            c.monto AS Monto,
                            c.fecha_desde AS FechaDesde,
                            c.fecha_hasta AS FechaHasta,
                            c.fecha_fin_anticipada AS FechaFinAnticipada,
                            c.finalizado AS Finalizado
                        FROM contrato c
                        JOIN inquilino inq ON c.id_inquilino = inq.id_inquilino
                        JOIN persona pi ON inq.id_persona = pi.id_persona
                        JOIN inmueble i ON c.id_inmueble = i.id_inmueble
                        JOIN tipo_inmueble ti ON i.id_tipo_inmueble = ti.id_tipo_inmueble
                        WHERE c.id_contrato = @idContrato;";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idContrato", idContrato);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            DateTime? fechaFinAnticipada = reader.IsDBNull(reader.GetOrdinal("FechaFinAnticipada"))
                                ? null
                                : reader.GetDateTime("FechaFinAnticipada");

                            return new ContratoResponseDTO(
                                reader.GetInt32("IdContrato"),
                                reader.GetString("DireccionInmueble"),
                                reader.GetString("TipoInmueble"),
                                reader.GetString("NombreCompletoInquilino"),
                                reader.GetDecimal("Monto"),
                                reader.GetDateTime("FechaDesde"),
                                reader.GetDateTime("FechaHasta"),
                                fechaFinAnticipada,
                                reader.GetBoolean("Finalizado")
                            );
                        }
                        else
                        {
                            throw new NotFoundException(NO_SE_ENCONTRO_CONTRATO_POR_ID + idContrato);
                        }
                    }
                }
            }
        }

        public Contrato ObtenerPorIdRequest(int idContrato)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"SELECT id_contrato,
                                    id_inquilino,
                                    id_inmueble,
                                    monto,
                                    fecha_desde,
                                    fecha_hasta,
                                    fecha_fin_anticipada,
                                    finalizado 
                                FROM Contrato 
                                WHERE id_contrato = @idContrato";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idContrato", idContrato);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Contrato
                            {
                                IdContrato = reader.GetInt32("id_contrato"),
                                IdInquilino = reader.GetInt32("id_inquilino"),
                                IdInmueble = reader.GetInt32("id_inmueble"),
                                Monto = reader.GetInt32("monto"),
                                FechaDesde = reader.GetDateTime("fecha_desde"),
                                FechaHasta = reader.GetDateTime("fecha_hasta"),
                                Finalizado = reader.GetBoolean("finalizado")
                            };
                        }
                        else
                        {
                            throw new NotFoundException(NO_SE_ENCONTRO_CONTRATO_POR_ID + idContrato);
                        }
                    }
                }
            }
        }
    }
}
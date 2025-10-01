
using MySql.Data.MySqlClient;
using proyecto_inmobiliaria.Config;
using proyecto_inmobiliaria.Dtos.response;
using proyecto_inmobiliaria.Enum;
using proyecto_inmobiliaria.Exceptions;
using proyecto_inmobiliaria.Models;

using static proyecto_inmobiliaria.Constants.AuditoriaConstants;

namespace proyecto_inmobiliaria.Repository.imp
{
    public class AuditoriaPagoRepository : IAuditoriaPagoRepository
    {
        private readonly string _connectionString;

        public AuditoriaPagoRepository(IAppConfig config)
        {
            _connectionString = config.ConnectionString;
        }

        public int CantidadAuditoria()
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            string query = "SELECT COUNT(*) FROM auditoria_pago;";
            using var command = new MySqlCommand(query, connection);
            return Convert.ToInt32(command.ExecuteScalar());
        }

        public IList<AuditoriaPagoResponseDTO> ListarAuditoria(int paginaNro, int tamPagina)
        {
            var auditorias = new List<AuditoriaPagoResponseDTO>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"
            SELECT ac.id_auditoria_pago,
                   ac.id_pago,
                   p.numero_pago,
                   p.id_contrato,
                   u.nombre_usuario AS NombreUsuario,
                   u.email AS Email,
                   ac.accion,
                   ac.fecha_accion
            FROM auditoria_pago ac
            JOIN usuario u ON ac.id_usuario = u.id_usuario
            JOIN pago p ON ac.id_pago = p.id_pago
            ORDER BY ac.fecha_accion DESC
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
                            auditorias.Add(new AuditoriaPagoResponseDTO(
                                reader.GetInt32("id_auditoria_pago"),
                                reader.GetInt32("id_pago"),
                                reader.GetInt32("numero_pago"),
                                reader.GetInt32("id_contrato"),
                                reader["NombreUsuario"] as string,
                                reader["Email"] as string,
                                System.Enum.Parse<Auditoria>(reader.GetString("accion"), true),
                                reader.GetDateTime("fecha_accion")
                            ));
                        }
                    }
                }
            }

            return auditorias;
        }


        public AuditoriaPagoResponseDTO ObtenerPorId(int idAuditoriaPago)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = @"
        SELECT ac.id_auditoria_pago,
               ac.id_pago,
               p.numero_pago,
               p.id_contrato,
               u.nombre_usuario AS NombreUsuario,
               u.email AS Email,
               ac.accion,
               ac.fecha_accion
        FROM auditoria_pago ac
        JOIN usuario u ON ac.id_usuario = u.id_usuario
        JOIN pago p ON ac.id_pago = p.id_pago
        WHERE ac.id_auditoria_pago = @IdAuditoriaPago;";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@IdAuditoriaPago", idAuditoriaPago);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new AuditoriaPagoResponseDTO(
                    reader.GetInt32("id_auditoria_pago"),
                    reader.GetInt32("id_pago"),
                    reader.GetInt32("numero_pago"),
                    reader.GetInt32("id_contrato"),
                    reader["NombreUsuario"] as string,
                    reader["Email"] as string,
                    System.Enum.Parse<Auditoria>(reader.GetString("accion"), true),
                    reader.GetDateTime("fecha_accion")
                );
            }
            else
            {
                throw new NotFoundException(NO_SE_ENCONTRO_AUDITORIA_POR_ID + idAuditoriaPago);
            }
        }



        public AuditoriaPago Registrar(AuditoriaPago auditoria)
        {
            int idAuditoria;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"INSERT INTO auditoria_pago
                                (id_pago, id_usuario, accion, fecha_accion)
                                VALUES (@IdPago, @IdUsuario, @Accion, @FechaAccion)";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdPago", auditoria.IdPago);
                    command.Parameters.AddWithValue("@IdUsuario", auditoria.IdUsuario);
                    command.Parameters.AddWithValue("@Accion", auditoria.Accion.ToString());
                    command.Parameters.AddWithValue("@FechaAccion", auditoria.FechaAccion);

                    command.ExecuteNonQuery();
                    idAuditoria = (int)command.LastInsertedId;
                }
            }

            auditoria.IdAuditoriaPago = idAuditoria;
            return auditoria;
        }
    }
}
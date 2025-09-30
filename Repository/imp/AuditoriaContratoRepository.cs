
using MySql.Data.MySqlClient;
using proyecto_inmobiliaria.Config;
using proyecto_inmobiliaria.Dtos.response;
using proyecto_inmobiliaria.Enum;
using proyecto_inmobiliaria.Exceptions;
using proyecto_inmobiliaria.Models;

using static proyecto_inmobiliaria.Constants.AuditoriaConstants;

namespace proyecto_inmobiliaria.Repository.imp
{
    public class AuditoriaContratoRepository : IAuditoriaContratoRepository
    {
        private readonly string _connectionString;

        public AuditoriaContratoRepository(IAppConfig config)
        {
            _connectionString = config.ConnectionString;
        }

        public int CantidadAuditoria()
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            string query = "SELECT COUNT(*) FROM auditoria_contrato;";
            using var command = new MySqlCommand(query, connection);
            return Convert.ToInt32(command.ExecuteScalar());
        }

        public IList<AuditoriaContratoResponseDTO> ListarAuditoria(int paginaNro, int tamPagina)
        {
            var auditorias = new List<AuditoriaContratoResponseDTO>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"
            SELECT ac.id_auditoria_contrato,
                   ac.id_contrato,
                   u.nombre_usuario AS NombreUsuario,
                   u.email AS Email,
                   ac.accion,
                   ac.fecha_accion
            FROM auditoria_contrato ac
            JOIN Usuario u ON ac.id_usuario = u.id_usuario
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
                            auditorias.Add(new AuditoriaContratoResponseDTO(
                                reader.GetInt32("id_auditoria_contrato"),
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

        public AuditoriaContratoResponseDTO ObtenerPorId(int idAuditoriaContrato)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = @"
        SELECT ac.id_auditoria_contrato,
               ac.id_contrato,
               u.nombre_usuario AS NombreUsuario,
               u.email AS Email,
               ac.accion,
               ac.fecha_accion
        FROM auditoria_contrato ac
        JOIN Usuario u ON ac.id_usuario = u.id_usuario
        WHERE ac.id_auditoria_contrato = @IdAuditoriaContrato;";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@IdAuditoriaContrato", idAuditoriaContrato);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new AuditoriaContratoResponseDTO(
                    reader.GetInt32("id_auditoria_contrato"),
                    reader.GetInt32("id_contrato"),
                    reader["NombreUsuario"] as string,
                    reader["Email"] as string,
                    System.Enum.Parse<Auditoria>(reader.GetString("accion"), true),
                    reader.GetDateTime("fecha_accion")
                );
            }
            else
            {
                throw new NotFoundException(NO_SE_ENCONTRO_AUDITORIA_POR_ID + idAuditoriaContrato);
            }
        }


        public AuditoriaContrato Registrar(AuditoriaContrato auditoria)
        {
            int idAuditoria;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"INSERT INTO auditoria_contrato
                                (id_contrato, id_usuario, accion, fecha_accion)
                                VALUES (@IdContrato, @IdUsuario, @Accion, @FechaAccion)";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdContrato", auditoria.IdContrato);
                    command.Parameters.AddWithValue("@IdUsuario", auditoria.IdUsuario);
                    command.Parameters.AddWithValue("@Accion", auditoria.Accion.ToString());
                    command.Parameters.AddWithValue("@FechaAccion", auditoria.FechaAccion);

                    command.ExecuteNonQuery();
                    idAuditoria = (int)command.LastInsertedId;
                }
            }

            auditoria.IdAuditoriaContrato = idAuditoria;
            return auditoria;
        }
    }
}
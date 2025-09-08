using MySql.Data.MySqlClient;
using proyecto_inmobiliaria.Config;
using proyecto_inmobiliaria.Exceptions;
using proyecto_inmobiliaria.Models;

using static proyecto_inmobiliaria.Constants.PagoConstants;

namespace proyecto_inmobiliaria.Repository.imp
{
    public class PagoRepository : IPagoRepository
    {
        private readonly string _connectionString;

        public PagoRepository(IAppConfig config)
        {
            _connectionString = config.ConnectionString;
        }

        public Pago Alta(Pago pago)
        {
            int idPago;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"INSERT INTO Pago
                                (id_contrato, metodo_pago, fecha_pago, monto, detalle, anulado)
                                VALUES (@IdContrato, @MetodoPago, @FechaPago, @Monto, @Detalle, @Anulado);";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdContrato", pago.IdContrato);
                    command.Parameters.AddWithValue("@MetodoPago", pago.MetodoPago);
                    command.Parameters.AddWithValue("@FechaPago", pago.FechaPago);
                    command.Parameters.AddWithValue("@Monto", pago.Monto);
                    command.Parameters.AddWithValue("@Detalle", pago.Detalle);
                    command.Parameters.AddWithValue("@Anulado", pago.Anulado);

                    command.ExecuteNonQuery();
                    idPago = (int)command.LastInsertedId;
                }
            }
            pago.IdPago = idPago;
            return pago;
        }
        public int Baja(int idPago)
        {
            int filasAfectadas;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"UPDATE Pago
                                SET anulado = 1
                                WHERE id_pago = @idPago;";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idPago", idPago);
                    filasAfectadas = command.ExecuteNonQuery();
                }
            }
            return filasAfectadas;
        }

        public Pago Modificar(Pago pago)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"UPDATE Pago
                                SET id_contrato = @IdContrato,
                                    metodo_pago = @MetodoPago,
                                    fecha_pago = @FechaPago,
                                    monto = @Monto,
                                    detalle = @Detalle,
                                    anulado = @Anulado
                                WHERE id_pago = @IdPago;";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdPago", pago.IdPago);
                    command.Parameters.AddWithValue("@IdContrato", pago.IdContrato);
                    command.Parameters.AddWithValue("@MetodoPago", pago.MetodoPago);
                    command.Parameters.AddWithValue("@FechaPago", pago.FechaPago);
                    command.Parameters.AddWithValue("@Monto", pago.Monto);
                    command.Parameters.AddWithValue("@Detalle", pago.Detalle);
                    command.Parameters.AddWithValue("@Anulado", pago.Anulado);

                    command.ExecuteNonQuery();
                }
            }
            return pago;
        }

        public IList<Pago> ObtenerLista(int paginaNro, int tamPagina)
        {
            var pagos = new List<Pago>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"SELECT id_pago, id_contrato, metodo_pago, fecha_pago, monto, detalle, anulado
                                 FROM Pago
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
                            pagos.Add(new Pago
                            {
                                IdPago = reader.GetInt32("id_pago"),
                                IdContrato = reader.GetInt32("id_contrato"),
                                MetodoPago = reader.GetString("metodo_pago"),
                                FechaPago = reader.GetDateTime("fecha_pago"),
                                Monto = reader.GetDecimal("monto"),
                                Detalle = reader.GetString("detalle"),
                                Anulado = reader.GetBoolean("anulado")
                            });
                        }
                    }
                }
            }
            return pagos;
        }

        public Pago ObtenerPorId(int idPago)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"SELECT id_pago, id_contrato, metodo_pago, fecha_pago, monto, detalle, anulado
                                 FROM Pago
                                 WHERE id_pago = @idPago;";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idPago", idPago);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Pago
                            {
                                IdPago = reader.GetInt32("id_pago"),
                                IdContrato = reader.GetInt32("id_contrato"),
                                MetodoPago = reader.GetString("metodo_pago"),
                                FechaPago = reader.GetDateTime("fecha_pago"),
                                Monto = reader.GetDecimal("monto"),
                                Detalle = reader.GetString("detalle"),
                                Anulado = reader.GetBoolean("anulado")
                            };
                        }
                        else
                        {
                            throw new NotFoundException(NO_SE_ENCONTRO_PAGO_POR_ID + idPago);
                        }
                    }
                }
            }
        }
    }
}

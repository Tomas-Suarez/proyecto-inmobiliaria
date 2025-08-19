using Exceptions;
using MySql.Data.MySqlClient;
using proyecto_inmobiliaria.Models;

using static proyecto_inmobiliaria.Constants.InquilinoConstants;

namespace proyecto_inmobiliaria.Repository
{
    public class InquilinoRepository : IInquilinoRepository
    {
        private readonly string _connectionString;

        public InquilinoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int Alta(Inquilino inquilino)
        {
            int idInquilino = -1;
            int idPersona = -1;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Insertamos la persona
                        string queryPersona = @"INSERT INTO Persona
                                            (nombre, apellido, documento, telefono, email, direccion, baja)
                                            VALUES (@nombre, @apellido, @documento, @telefono, @email, @direccion, @baja);";

                        using (MySqlCommand command = new MySqlCommand(queryPersona, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@nombre", inquilino.Nombre);
                            command.Parameters.AddWithValue("@apellido", inquilino.Apellido);
                            command.Parameters.AddWithValue("@documento", inquilino.Documento);
                            command.Parameters.AddWithValue("@telefono", inquilino.Telefono);
                            command.Parameters.AddWithValue("@email", inquilino.Email);
                            command.Parameters.AddWithValue("@direccion", inquilino.Direccion);
                            command.Parameters.AddWithValue("@baja", inquilino.Baja);

                            command.ExecuteNonQuery();
                            idPersona = (int)command.LastInsertedId;
                        }

                        // Insertamos el inquilino con el (id persona)
                        string queryInquilino = @"INSERT INTO Inquilino(id_Persona)
                                                VALUE (@idPersona);";

                        using (MySqlCommand command = new MySqlCommand(queryInquilino, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@idPersona", idPersona);

                            command.ExecuteNonQuery();
                            idInquilino = (int)command.LastInsertedId;
                        }
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw; // TODO: agregar excepcion personalizada MySQL?
                    }
                }
            }
            return idInquilino;
        }

        public int Baja(int idInquilino)
        {
            int filasAfectadas = 0;

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string queryGetPersona = @"SELECT id_persona FROM inquilino 
                                                WHERE id_inquilino = @idInquilino;";
                        int idPersona;

                        using (var command = new MySqlCommand(queryGetPersona, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@idInquilino", idInquilino);
                            var resultado = command.ExecuteScalar();

                            if (resultado == null)
                            {
                                throw new NotFoundException(NO_SE_ENCONTRO_INQUILINO_POR_ID + idInquilino);
                            }

                            idPersona = Convert.ToInt32(resultado);
                        }

                        string queryDeletePersona = @"DELETE FROM persona 
                                                    WHERE id_persona = @idPersona";
                        using (var command = new MySqlCommand(queryDeletePersona, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@idPersona", idPersona);
                            filasAfectadas = command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return filasAfectadas;
        }

        public int Modificar(Inquilino inquilino)
        {
            int filasAfectadas = 0;

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string queryGetPersona = @"SELECT id_persona FROM inquilino 
                                                WHERE id_inquilino = @idInquilino;";
                        int idPersona;

                        using (var command = new MySqlCommand(queryGetPersona, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@idInquilino", inquilino.IdInquilino);
                            var resultado = command.ExecuteScalar();

                            if (resultado == null)
                            {
                                throw new NotFoundException(NO_SE_ENCONTRO_INQUILINO_POR_ID + inquilino.IdInquilino);
                            }
                            idPersona = Convert.ToInt32(resultado);
                        }

                        string queryUpdatePersona = @"UPDATE persona 
                                                    SET nombre = @nombre,
                                                        apellido = @apellido,
                                                        documento = @documento,
                                                        telefono = @telefono,
                                                        email = @email,
                                                        direccion = @direccion,
                                                        baja = @baja
                                                    WHERE id_persona = @idPersona";
                        using (var command = new MySqlCommand(queryUpdatePersona, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@nombre", inquilino.Nombre);
                            command.Parameters.AddWithValue("@apellido", inquilino.Apellido);
                            command.Parameters.AddWithValue("@documento", inquilino.Documento);
                            command.Parameters.AddWithValue("@telefono", inquilino.Telefono);
                            command.Parameters.AddWithValue("@email", inquilino.Email);
                            command.Parameters.AddWithValue("@direccion", inquilino.Direccion);
                            command.Parameters.AddWithValue("@baja", inquilino.Baja);
                            command.Parameters.AddWithValue("@idPersona", idPersona);

                            filasAfectadas = command.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return filasAfectadas;
        }
    }
}

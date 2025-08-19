using Exceptions;
using MySql.Data.MySqlClient;
using proyecto_inmobiliaria.Models;

using static proyecto_inmobiliaria.Constants.PropietarioConstants;


namespace proyecto_inmobiliaria.Repository
{
    public class PropietarioRepository : IPropietarioRepository
    {
        private readonly string _connectionString;

        public PropietarioRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int Alta(Propietario propietario)
        {
            int idPropietario = -1;
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
                            command.Parameters.AddWithValue("@nombre", propietario.Nombre);
                            command.Parameters.AddWithValue("@apellido", propietario.Apellido);
                            command.Parameters.AddWithValue("@documento", propietario.Documento);
                            command.Parameters.AddWithValue("@telefono", propietario.Telefono);
                            command.Parameters.AddWithValue("@email", propietario.Email);
                            command.Parameters.AddWithValue("@direccion", propietario.Direccion);
                            command.Parameters.AddWithValue("@baja", propietario.Baja);

                            command.ExecuteNonQuery();
                            idPersona = (int)command.LastInsertedId;
                        }

                        // Insertamos el propietario con el (id persona)
                        string queryPropietario = @"INSERT INTO Propietario(id_Persona)
                                                VALUE (@idPersona);";

                        using (MySqlCommand command = new MySqlCommand(queryPropietario, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@idPersona", idPersona);

                            command.ExecuteNonQuery();
                            idPropietario = (int)command.LastInsertedId;
                        }
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw; // TODO: Agregar excepcion personalizada mysql??
                    }
                }
            }
            return idPropietario;
        }

        public int Baja(int idPropietario)
        {
            int filasAfectadas = 0;

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string queryGetPersona = @"SELECT id_persona from propietario 
                                                where id_propietario = @idPropietario;";
                        int idPersona;

                        using (var command = new MySqlCommand(queryGetPersona, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@idPropietario", idPropietario);
                            var resultado = command.ExecuteScalar();

                            if (resultado == null)
                            {
                                throw new NotFoundException(NO_SE_ENCONTRO_PROPIETARIO_POR_ID + idPropietario);
                            }

                            idPersona = Convert.ToInt32(resultado);
                        }

                        string queryDeletePersona = @"DELETE FROM persona 
                                                    where id_persona = @idPersona";
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
                        throw;  //
                    }
                }
            }

            return filasAfectadas;
        }

        public int Modificar(Propietario propietario)
        {
            int filasAfectadas = 0;

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string queryGetPersona = @"SELECT id_persona from propietario 
                                                where id_propietario = @idPropietario;";
                        int idPersona;

                        using (var command = new MySqlCommand(queryGetPersona, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@idPropietario", propietario.IdPropietario);
                            var resultado = command.ExecuteScalar();

                            if (resultado == null)
                            {
                                throw new NotFoundException(NO_SE_ENCONTRO_PROPIETARIO_POR_ID + propietario.IdPropietario);
                            }
                            idPersona = Convert.ToInt32(resultado);

                        }

                        string queryUpdatePersona = @"UPDATE persona 
                                                    set nombre = @nombre,
                                                        apellido = @apellido,
                                                        documento = @documento,
                                                        telefono = @telefono,
                                                        email = @email,
                                                        direccion = @direccion,
                                                        baja = @baja
                                                    WHERE id_persona = @idPersona";
                        using (var command = new MySqlCommand(queryUpdatePersona, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@nombre", propietario.Nombre);
                            command.Parameters.AddWithValue("@apellido", propietario.Apellido);
                            command.Parameters.AddWithValue("@documento", propietario.Documento);
                            command.Parameters.AddWithValue("@telefono", propietario.Telefono);
                            command.Parameters.AddWithValue("@email", propietario.Email);
                            command.Parameters.AddWithValue("@direccion", propietario.Direccion);
                            command.Parameters.AddWithValue("@baja", propietario.Baja);
                            command.Parameters.AddWithValue("idPersona", idPersona);

                            filasAfectadas = command.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw; //
                    }
                }
            }

            return filasAfectadas;

        }
    }
}


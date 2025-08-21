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

        public Propietario Alta(Propietario propietario)
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
                                            (nombre, apellido, documento, telefono, email, direccion)
                                            VALUES (@nombre, @apellido, @documento, @telefono, @email, @direccion);";


                        using (MySqlCommand command = new MySqlCommand(queryPersona, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@nombre", propietario.Nombre);
                            command.Parameters.AddWithValue("@apellido", propietario.Apellido);
                            command.Parameters.AddWithValue("@documento", propietario.Documento);
                            command.Parameters.AddWithValue("@telefono", propietario.Telefono);
                            command.Parameters.AddWithValue("@email", propietario.Email);
                            command.Parameters.AddWithValue("@direccion", propietario.Direccion);

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

                        propietario.IdPropietario = idPropietario;

                        return propietario;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw; // TODO: Agregar excepcion personalizada mysql??
                    }
                }
            }
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

        public Propietario Modificar(Propietario propietario)
        {
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
                                                        direccion = @direccion
                                                    WHERE id_persona = @idPersona";
                        using (var command = new MySqlCommand(queryUpdatePersona, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@nombre", propietario.Nombre);
                            command.Parameters.AddWithValue("@apellido", propietario.Apellido);
                            command.Parameters.AddWithValue("@documento", propietario.Documento);
                            command.Parameters.AddWithValue("@telefono", propietario.Telefono);
                            command.Parameters.AddWithValue("@email", propietario.Email);
                            command.Parameters.AddWithValue("@direccion", propietario.Direccion);
                            command.Parameters.AddWithValue("idPersona", idPersona);

                            command.ExecuteNonQuery();
                        }
                        transaction.Commit();

                        return propietario;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw; //
                    }
                }
            }
        }

        public Propietario ObtenerPorId(int idPropietario)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string queryGetPersona = @"SELECT p.nombre,
                                            p.apellido,
                                            p.documento,
                                            p.telefono,
                                            p.email,
                                            p.direccion,
                                            pr.id_propietario
                                        FROM persona p
                                        JOIN propietario pr ON p.id_persona = pr.id_persona
                                        WHERE pr.id_propietario = @idPropietario;";
                using (var command = new MySqlCommand(queryGetPersona, connection))
                {
                    command.Parameters.AddWithValue("@idPropietario", idPropietario);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Propietario
                            {
                                IdPropietario = reader.GetInt32(reader.GetOrdinal("id_propietario")),
                                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                                Apellido = reader.GetString(reader.GetOrdinal("apellido")),
                                Documento = reader.GetString(reader.GetOrdinal("documento")),
                                Telefono = reader.GetString(reader.GetOrdinal("telefono")),
                                Email = reader.GetString(reader.GetOrdinal("email")),
                                Direccion = reader.GetString(reader.GetOrdinal("direccion")),
                            };
                        }
                        else
                        {
                            throw new NotFoundException(NO_SE_ENCONTRO_PROPIETARIO_POR_ID + idPropietario);
                        }
                    }
                }
            }
        }

        public IList<Propietario> ObtenerTodos()
        {
            IList<Propietario> propietarios = new List<Propietario>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string queryGetAllPropietarios = @"SELECT p.nombre,
                                                    p.apellido,
                                                    p.documento,
                                                    p.telefono,
                                                    p.email,
                                                    p.direccion,
                                                    pr.id_propietario
                                                FROM persona p
                                                INNER JOIN propietario pr ON p.id_persona = pr.id_persona";

                using (var command = new MySqlCommand(queryGetAllPropietarios, connection))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Propietario propietario = new Propietario
                        {
                            IdPropietario = reader.GetInt32(reader.GetOrdinal("id_propietario")),
                            Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                            Apellido = reader.GetString(reader.GetOrdinal("apellido")),
                            Documento = reader.GetString(reader.GetOrdinal("documento")),
                            Telefono = reader.GetString(reader.GetOrdinal("telefono")),
                            Email = reader.GetString(reader.GetOrdinal("email")),
                            Direccion = reader.GetString(reader.GetOrdinal("direccion")),
                        };
                        propietarios.Add(propietario);
                    }
                }
            }
            return propietarios;
        }

        public IList<Propietario> ObtenerLista(int paginaNro = 1, int tamPagina = 10)
        {
            throw new NotImplementedException();
        }
    }
}


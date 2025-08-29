using MySql.Data.MySqlClient;
using proyecto_inmobiliaria.Models;
using proyecto_inmobiliaria.Exceptions;

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

        public Inquilino Alta(Inquilino Inquilino)
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
                                            (nombre, apellido, documento, telefono, email, direccion)
                                            VALUES (@nombre, @apellido, @documento, @telefono, @email, @direccion);";


                        using (MySqlCommand command = new MySqlCommand(queryPersona, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@nombre", Inquilino.Nombre);
                            command.Parameters.AddWithValue("@apellido", Inquilino.Apellido);
                            command.Parameters.AddWithValue("@documento", Inquilino.Documento);
                            command.Parameters.AddWithValue("@telefono", Inquilino.Telefono);
                            command.Parameters.AddWithValue("@email", Inquilino.Email);
                            command.Parameters.AddWithValue("@direccion", Inquilino.Direccion);

                            command.ExecuteNonQuery();
                            idPersona = (int)command.LastInsertedId;
                        }

                        // Insertamos el Inquilino con el (id persona)
                        string queryInquilino = @"INSERT INTO Inquilino(id_Persona)
                                                VALUE (@idPersona);";

                        using (MySqlCommand command = new MySqlCommand(queryInquilino, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@idPersona", idPersona);

                            command.ExecuteNonQuery();
                            idInquilino = (int)command.LastInsertedId;
                        }
                        transaction.Commit();

                        Inquilino.IdInquilino = idInquilino;

                        return Inquilino;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw; // TODO: Agregar excepcion personalizada mysql??
                    }
                }
            }
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
                        string queryGetPersona = @"SELECT id_persona from Inquilino 
                                                where id_Inquilino = @idInquilino;";
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

        public Inquilino Modificar(Inquilino Inquilino)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string queryGetPersona = @"SELECT id_persona from Inquilino 
                                                where id_Inquilino = @idInquilino;";
                        int idPersona;

                        using (var command = new MySqlCommand(queryGetPersona, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@idInquilino", Inquilino.IdInquilino);
                            var resultado = command.ExecuteScalar();

                            if (resultado == null)
                            {
                                throw new NotFoundException(NO_SE_ENCONTRO_INQUILINO_POR_ID + Inquilino.IdInquilino);
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
                            command.Parameters.AddWithValue("@nombre", Inquilino.Nombre);
                            command.Parameters.AddWithValue("@apellido", Inquilino.Apellido);
                            command.Parameters.AddWithValue("@documento", Inquilino.Documento);
                            command.Parameters.AddWithValue("@telefono", Inquilino.Telefono);
                            command.Parameters.AddWithValue("@email", Inquilino.Email);
                            command.Parameters.AddWithValue("@direccion", Inquilino.Direccion);
                            command.Parameters.AddWithValue("idPersona", idPersona);

                            command.ExecuteNonQuery();
                        }
                        transaction.Commit();

                        return Inquilino;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw; //
                    }
                }
            }
        }

        public Inquilino ObtenerPorId(int idInquilino)
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
                                            pr.id_Inquilino
                                        FROM persona p
                                        JOIN Inquilino pr ON p.id_persona = pr.id_persona
                                        WHERE pr.id_Inquilino = @idInquilino;";
                using (var command = new MySqlCommand(queryGetPersona, connection))
                {
                    command.Parameters.AddWithValue("@idInquilino", idInquilino);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Inquilino
                            {
                                IdInquilino = reader.GetInt32(reader.GetOrdinal("id_Inquilino")),
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
                            throw new NotFoundException(NO_SE_ENCONTRO_INQUILINO_POR_ID + idInquilino);
                        }
                    }
                }
            }
        }

        public IList<Inquilino> ObtenerTodos()
        {
            IList<Inquilino> Inquilinos = new List<Inquilino>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string queryGetAllInquilinos = @"SELECT p.nombre,
                                                    p.apellido,
                                                    p.documento,
                                                    p.telefono,
                                                    p.email,
                                                    p.direccion,
                                                    pr.id_Inquilino
                                                FROM persona p
                                                INNER JOIN Inquilino pr ON p.id_persona = pr.id_persona";

                using (var command = new MySqlCommand(queryGetAllInquilinos, connection))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Inquilino Inquilino = new Inquilino
                        {
                            IdInquilino = reader.GetInt32(reader.GetOrdinal("id_Inquilino")),
                            Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                            Apellido = reader.GetString(reader.GetOrdinal("apellido")),
                            Documento = reader.GetString(reader.GetOrdinal("documento")),
                            Telefono = reader.GetString(reader.GetOrdinal("telefono")),
                            Email = reader.GetString(reader.GetOrdinal("email")),
                            Direccion = reader.GetString(reader.GetOrdinal("direccion")),
                        };
                        Inquilinos.Add(Inquilino);
                    }
                }
            }
            return Inquilinos;
        }

        public IList<Inquilino> ObtenerLista(int paginaNro = 1, int tamPagina = 10)
        {
            throw new NotImplementedException();
        }
    }
}



using MySql.Data.MySqlClient;
using proyecto_inmobiliaria.Config;
using proyecto_inmobiliaria.Models;
using proyecto_inmobiliaria.Exceptions;
using proyecto_inmobiliaria.Enum;


using static proyecto_inmobiliaria.Constants.UsuarioConstants;

namespace proyecto_inmobiliaria.Repository.imp
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(IAppConfig config)
        {
            _connectionString = config.ConnectionString;
        }

        public Usuario Alta(Usuario usuario)
        {
            int idUsuario;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"INSERT INTO Usuario
                                (nombre_usuario, rol, contrasena, email, avatar_url)
                                VALUES (@NombreUsuario, @Rol, @Contrasena, @Email, @AvatarUrl)";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
                    command.Parameters.AddWithValue("@Rol", usuario.Rol.ToString());
                    command.Parameters.AddWithValue("@Contrasena", usuario.Contrasena);
                    command.Parameters.AddWithValue("@Email", usuario.Email);
                    command.Parameters.AddWithValue("@AvatarUrl", (object?)usuario.Avatar_url ?? DBNull.Value);

                    command.ExecuteNonQuery();
                    idUsuario = (int)command.LastInsertedId;
                }
            }
            usuario.IdUsuario = idUsuario;
            return usuario;
        }

        public int Baja(int idUsuario)
        {
            int filasAfectadas;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"DELETE FROM Usuario WHERE id_usuario = @idUsuario";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idUsuario", idUsuario);
                    filasAfectadas = command.ExecuteNonQuery();
                }
            }
            return filasAfectadas;
        }

        public Usuario Modificar(Usuario usuario)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"UPDATE usuario
                                SET nombre_usuario = @NombreUsuario,
                                    rol = @Rol,
                                    contrasena = @Contrasena,
                                    email = @Email,
                                    avatar_url = @AvatarUrl
                                WHERE id_usuario = @IdUsuario;";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);
                    command.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
                    command.Parameters.AddWithValue("@Rol", usuario.Rol.ToString());
                    command.Parameters.AddWithValue("@Contrasena", usuario.Contrasena);
                    command.Parameters.AddWithValue("@Email", usuario.Email);
                    command.Parameters.AddWithValue("@AvatarUrl", (object?)usuario.Avatar_url ?? DBNull.Value);

                    int filas = command.ExecuteNonQuery();
                    if (filas == 0)
                        throw new NotFoundException(NO_SE_ENCONTRO_USUARIO_POR_ID + usuario.IdUsuario);
                }
            }
            return usuario;
        }

        public IList<Usuario> ObtenerLista(int paginaNro, int tamPagina)
        {
            IList<Usuario> usuarios = new List<Usuario>();

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = @"SELECT id_usuario, nombre_usuario, rol, contrasena, email, avatar_url
                            FROM usuario
                            ORDER BY nombre_usuario
                            LIMIT @PageSize OFFSET @Offset;";

            using var command = new MySqlCommand(query, connection);
            int offset = (paginaNro - 1) * tamPagina;
            command.Parameters.AddWithValue("@PageSize", tamPagina);
            command.Parameters.AddWithValue("@Offset", offset);

            using var reader = command.ExecuteReader();
            int avatarIndex = reader.GetOrdinal("avatar_url");

            while (reader.Read())
            {
                usuarios.Add(new Usuario
                {
                    IdUsuario = reader.GetInt32("id_usuario"),
                    NombreUsuario = reader.GetString("nombre_usuario"),
                    Rol = System.Enum.Parse<ERol>(reader.GetString("rol"), true),
                    Contrasena = reader.GetString("contrasena"),
                    Email = reader.GetString("email"),
                    Avatar_url = reader.IsDBNull(avatarIndex) ? null : reader.GetString(avatarIndex)
                });
            }

            return usuarios;
        }

        public Usuario ObtenerPorId(int idUsuario)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"SELECT id_usuario, nombre_usuario, rol, contrasena, email, avatar_url
                                FROM usuario
                                WHERE id_usuario = @idUsuario;";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idUsuario", idUsuario);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int avatarIndex = reader.GetOrdinal("avatar_url");

                            return new Usuario
                            {
                                IdUsuario = reader.GetInt32("id_usuario"),
                                NombreUsuario = reader.GetString("nombre_usuario"),
                                Rol = System.Enum.Parse<ERol>(reader.GetString("rol"), true),
                                Contrasena = reader.GetString("contrasena"),
                                Email = reader.GetString("email"),
                                Avatar_url = reader.IsDBNull(avatarIndex) ? null : reader.GetString(avatarIndex)
                            };
                        }
                        else
                        {
                            throw new NotFoundException(NO_SE_ENCONTRO_USUARIO_POR_ID + idUsuario);
                        }
                    }
                }
            }
        }

        public int CantidadTotal()
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            string query = "SELECT COUNT(*) FROM usuario;";
            using var command = new MySqlCommand(query, connection);
            return Convert.ToInt32(command.ExecuteScalar());
        }

        public Usuario? ObtenerPorNombreUsuarioOEmail(string nombreUsuarioOEmail)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"SELECT id_usuario, nombre_usuario, rol, contrasena, email, avatar_url
                         FROM usuario
                         WHERE nombre_usuario = @valor OR email = @valor
                         LIMIT 1;";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@valor", nombreUsuarioOEmail);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int avatarIndex = reader.GetOrdinal("avatar_url");

                            return new Usuario
                            {
                                IdUsuario = reader.GetInt32("id_usuario"),
                                NombreUsuario = reader.GetString("nombre_usuario"),
                                Rol = System.Enum.Parse<ERol>(reader.GetString("rol"), true),
                                Contrasena = reader.GetString("contrasena"),
                                Email = reader.GetString("email"),
                                Avatar_url = reader.IsDBNull(avatarIndex) ? null : reader.GetString(avatarIndex)
                            };
                        }
                    }
                }
            }

            return null;
        }
    }
}
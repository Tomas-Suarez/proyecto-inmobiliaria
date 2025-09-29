using System.Security.Cryptography;
using System.Text;

namespace proyecto_inmobiliaria.Helper
{
    public static class AuthHelper
    {
        public static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        public static bool VerificarPassword(string password, string HashedPassword)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput == HashedPassword;
        }
    }
}
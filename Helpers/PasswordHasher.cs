using System;
using System.Security.Cryptography;
using System.Text;

namespace HELPERS
{
    public static class PasswordHasher
    {
        private const int SaltSize = 16;

        public static string GenerateSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[SaltSize];
                rng.GetBytes(salt);
                return Convert.ToBase64String(salt);
            }
        }

        public static string HashPassword(string password, string salt)
        {
            return Hash(password + salt);
        }

        public static string Hash(string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input ?? string.Empty);
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(bytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}

using App.Core.Interfaces;
using System.Collections;
using System.Security.Cryptography;

namespace App.Infrastructure.Security
{
    public class RBKDF2PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            // Генерация соли
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Хэширование пароля с использованием Rfc2898DeriveBytes
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(32); // Хэш длиной 32 байта

                // Объединение хэша и соли в одну строку
                string hashString = Convert.ToBase64String(hash);
                string saltString = Convert.ToBase64String(salt);
                return $"{hashString}:{saltString}";
            }
        }

        public bool VerifyPassword(string password, string storedHash)
        {
            // Разделение хэша и соли
            var parts = storedHash.Split(':');
            if (parts.Length != 2)
            {
                throw new FormatException("Stored hash is not in the correct format.");
            }

            byte[] hash = Convert.FromBase64String(parts[0]);
            byte[] salt = Convert.FromBase64String(parts[1]);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256))
            {
                byte[] newHash = pbkdf2.GetBytes(32); // Хэш длиной 32 байта
                return StructuralComparisons.StructuralEqualityComparer.Equals(hash, newHash);
            }
        }
    }
}

using App.Core.Interfaces;

namespace App.Infrastructure.Security
{
    public class BCryptPasswordHasher : IPasswordHasher
    {
        private readonly int _workFactor;

        public BCryptPasswordHasher(int workFactor = 12)
        {
            _workFactor = workFactor;
        }

        public string Hash(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password cannot be empty or whitespace.", nameof(password));
            }

            return BCrypt.Net.BCrypt.HashPassword(password, _workFactor);
        }

        public bool Verify(string password, string hash)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password cannot be empty or whitespace.", nameof(password));
            }

            if (string.IsNullOrWhiteSpace(hash))
            {
                throw new ArgumentException("Hash cannot be empty or whitespace.", nameof(hash));
            }

            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}

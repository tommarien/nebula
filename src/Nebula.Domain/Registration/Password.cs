using System;
using System.Security.Cryptography;
using Nebula.Infrastructure;

namespace Nebula.Domain.Registration
{
    public class Password : IEquatable<string>
    {
        private const int HashSize = 32;
        private readonly string hash;
        private readonly string salt;

        private Password()
        {
        }

        public Password(string password)
            : this()
        {
            if (!IsValid(password))
                throw new InvalidStateException("Password cannot be null or empty.");

            var bytes = new Rfc2898DeriveBytes(password, 20);
            hash = Convert.ToBase64String(bytes.GetBytes(HashSize));
            salt = Convert.ToBase64String(bytes.Salt);
        }

        public static bool IsValid(string password)
        {
            return !string.IsNullOrWhiteSpace(password);
        }

        public bool Equals(string other)
        {
            if (other == null) return false;
            var deriveBytes = new Rfc2898DeriveBytes(other, Convert.FromBase64String(salt));
            return hash.Equals(Convert.ToBase64String(deriveBytes.GetBytes(HashSize)));
        }
    }
}
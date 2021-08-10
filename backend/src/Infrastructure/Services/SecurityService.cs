using Application.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;

namespace Infrastructure.Services
{
    public class SecurityService: ISecurityService
    {
        public string HashPassword(string password, byte[] salt)
        {
            return Convert.ToBase64String(
                    KeyDerivation.Pbkdf2(
                        password: password,
                        salt: salt,
                        prf: KeyDerivationPrf.HMACSHA256,
                        iterationCount: 10000,
                        numBytesRequested: 256 / 8
                    )
                );
        }

        public bool ValidatePassword(string password, string hash, string salt)
        {
            return HashPassword(password, Convert.FromBase64String(salt)) == hash;
        }

        public byte[] GetRandomBytes(int length = 32)
        {
            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                var salt = new byte[length];
                randomNumberGenerator.GetBytes(salt);

                return salt;
            }
        }
    }
}

namespace CakeWebApp.Services
{
    using CakeWebApp.Services.Contracts;
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public class HashService : IHashService
    {
        private const string Salt = "saltedPassword";

        public string Hash(string stringToHash)
        {
            stringToHash = stringToHash + Salt;
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                var hashedBytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(stringToHash));

                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                return hash;
            }
        }
    }
}
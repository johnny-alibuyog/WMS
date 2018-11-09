using System;
using System.Security.Cryptography;
using System.Text;

namespace AmpedBiz.Core.Common.Services.Generators
{
    public interface IHashProvider
    {
        void GetHashAndSalt(byte[] data, out byte[] hash, out byte[] salt);

        void GetHashAndSaltString(string data, out string hash, out string salt);

        bool VerifyHash(byte[] data, byte[] hash, byte[] salt);

        bool VerifyHashString(string data, string hash, string salt);
    }

    public class HashProvider : IHashProvider
    {
        private readonly int _salthLength;
        private readonly HashAlgorithm _hashAlgorithm;

        public static IHashProvider New() => new HashProvider();

        public HashProvider() : this((HashAlgorithm)new SHA256Managed(), 4) { }

        public HashProvider(HashAlgorithm hashAlgorithm, int saltLength)
        {
            this._hashAlgorithm = hashAlgorithm;
            this._salthLength = saltLength;
        }

        private byte[] ComputeHash(byte[] data, byte[] salt)
        {
            byte[] buffer = new byte[data.Length + this._salthLength];
            Array.Copy((Array)data, (Array)buffer, data.Length);
            Array.Copy((Array)salt, 0, (Array)buffer, data.Length, this._salthLength);
            return this._hashAlgorithm.ComputeHash(buffer);
        }

        public void GetHashAndSalt(byte[] data, out byte[] hash, out byte[] salt)
        {
            salt = new byte[this._salthLength];
            new RNGCryptoServiceProvider().GetNonZeroBytes(salt);
            hash = this.ComputeHash(data, salt);
        }

        public void GetHashAndSaltString(string data, out string hash, out string salt)
        {
            var hash1 = default(byte[]);
            var salt1 = default(byte[]);
            this.GetHashAndSalt(Encoding.UTF8.GetBytes(data), out hash1, out salt1);
            hash = Convert.ToBase64String(hash1);
            salt = Convert.ToBase64String(salt1);
        }

        public bool VerifyHash(byte[] data, byte[] hash, byte[] salt)
        {
            var computedHash = this.ComputeHash(data, salt);
            if (computedHash.Length != hash.Length)
                return false;
            for (int index = 0; index < hash.Length; ++index)
            {
                if (!hash[index].Equals(computedHash[index]))
                    return false;
            }
            return true;
        }

        public bool VerifyHashString(string data, string hash, string salt)
        {
            var hash1 = Convert.FromBase64String(hash);
            var salt1 = Convert.FromBase64String(salt);
            return this.VerifyHash(Encoding.UTF8.GetBytes(data), hash1, salt1);
        }
    }
}

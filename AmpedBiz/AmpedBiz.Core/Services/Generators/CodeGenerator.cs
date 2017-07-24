using System;
using System.Linq;
using System.Security.Cryptography;

namespace AmpedBiz.Core.Services.Generators
{
    // https://stackoverflow.com/questions/4713584/how-to-generate-a-voucher-code-in-c

    internal class Option
    {
        public int Length { get; set; }

        public char[] AllowedCharacters { get; set; }

        public Option()
        {
            this.Length = 10;
            this.AllowedCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToArray();
        }

        public void EnsureValidity()
        {
            if (this.Length < 1)
                throw new ArgumentOutOfRangeException("Option.Length", "Length cannot be less than 1");

            if (this.AllowedCharacters == null || !this.AllowedCharacters.Any())
                throw new ArgumentNullException("Option.AllowedCharacters", "Allowed characters may not be null or empty.");
        }
    }

    internal class CodeGenerator
    {
        public Option Option { get; set; }

        public CodeGenerator(Option option = null)
        {
            this.Option = option ?? new Option();
        }

        public string Generate()
        {
            this.Option.EnsureValidity();

            var outOfRange = byte.MaxValue + 1 - (byte.MaxValue + 1) % this.Option.AllowedCharacters.Length;

            return string.Concat(Enumerable
                .Repeat(0, int.MaxValue)
                .Select(_ => RandomByte())
                .Where(randomByte => randomByte < outOfRange)
                .Take(this.Option.Length)
                .Select(randomByte => this.Option.AllowedCharacters[randomByte % this.Option.AllowedCharacters.Length])
            );
        }

        private byte RandomByte()
        {
            using (var generator = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[1];
                generator.GetBytes(randomBytes);
                return randomBytes.Single();
            }
        }
    }
}

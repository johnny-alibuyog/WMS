using AmpedBiz.Core.Orders;
using AmpedBiz.Core.PointOfSales;
using AmpedBiz.Core.PurchaseOrders;
using AmpedBiz.Core.SharedKernel;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace AmpedBiz.Core.Common.Services.Generators
{
    // https://stackoverflow.com/questions/4713584/how-to-generate-a-voucher-code-in-c

    internal class Option
    {
        public int Length { get; set; }

        public char[] AllowedCharacters { get; set; }

        public Option()
        {
            this.Length = 6;
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

    internal class CodeGen
    {
        public Option Option { get; set; }

        public CodeGen(Option option = null)
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

	public class CodeGenVisitor : IVisitor<Order>, IVisitor<PurchaseOrder>, IVisitor<PointOfSale>
	{
		private string Generate(string suffix)
		{
			var codeGen = new CodeGen();

			var raw = $"{codeGen.Generate()}{suffix}";

			return Regex.Replace(raw, ".{6}", "$0-");
		}

		public void Visit(PurchaseOrder target)
		{
			if (string.IsNullOrWhiteSpace(target.VoucherNumber))
			{
				target.VoucherNumber = this.Generate("VN");
			}
		}

		public void Visit(Order target)
		{
			if (string.IsNullOrWhiteSpace(target.InvoiceNumber))
			{
				target.InvoiceNumber = this.Generate("IN");
			}
		}

		public void Visit(PointOfSale target)
		{
			if (string.IsNullOrWhiteSpace(target.InvoiceNumber))
			{
				target.InvoiceNumber = this.Generate("PN");
			}
		}
	}
}

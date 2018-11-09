using AmpedBiz.Core.Common.Services.Generators;
using System.Text.RegularExpressions;

namespace AmpedBiz.Core.Orders.Services
{
    internal class InvoiceGenerator
    {
        public string Generate()
        {
            var generator = new CodeGen();

            var raw = $"{generator.Generate()}IN";

            return Regex.Replace(raw, ".{6}", "$0-");
        }
    }
}

using AmpedBiz.Core.Services.Generators;
using System.Text.RegularExpressions;

namespace AmpedBiz.Core.Services.Orders
{
    internal class InvoiceGenerator
    {
        public string Generate()
        {
            var generator = new CodeGenerator();

            var raw = $"{generator.Generate()}IN";

            return Regex.Replace(raw, ".{5}", "$0-");
        }
    }
}

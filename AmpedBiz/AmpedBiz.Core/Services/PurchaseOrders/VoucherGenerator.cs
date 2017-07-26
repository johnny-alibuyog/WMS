using AmpedBiz.Core.Services.Generators;
using System.Text.RegularExpressions;

namespace AmpedBiz.Core.Services.PurchaseOrders
{
    internal class VoucherGenerator
    {
        public string Generate()
        {
            var generator = new CodeGenerator();

            var raw = $"{generator.Generate()}VN";

            return Regex.Replace(raw, ".{6}", "$0-");
        }
    }
}

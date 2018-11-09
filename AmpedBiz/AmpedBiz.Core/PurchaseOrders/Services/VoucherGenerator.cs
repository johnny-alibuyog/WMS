using AmpedBiz.Core.Common.Services.Generators;
using System.Text.RegularExpressions;

namespace AmpedBiz.Core.PurchaseOrders.Services
{
    internal class VoucherGenerator
    {
        public string Generate()
        {
            var generator = new CodeGen();

            var raw = $"{generator.Generate()}VN";

            return Regex.Replace(raw, ".{6}", "$0-");
        }
    }
}

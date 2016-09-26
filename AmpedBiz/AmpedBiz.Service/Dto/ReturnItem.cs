using AmpedBiz.Common.CustomTypes;
using System;

namespace AmpedBiz.Service.Dto
{
    public class ReturnItem
    {
        public Guid Id { get; set; }

        public Guid ReturnId { get; set; }

        public Lookup<Guid> Product { get; set; }

        public Lookup<string> ReturnReason { get; set; }

        public decimal QuantityValue { get; set; }

        public decimal UnitPriceAmount { get; set; }

        public decimal ExtendedPriceAmount { get; set; }

        public decimal TotalPriceAmount { get; set; }
    }

    public class ReturnItemPageItem
    {
        public Guid Id { get; set; }

        public Guid ReturnId { get; set; }

        public string ProductName { get; set; }

        public string ReturnReasonName { get; set; }

        public decimal QuantityValue { get; set; }

        public decimal UnitPriceAmount { get; set; }

        public decimal ExtendedPriceAmount { get; set; }

        public decimal TotalPriceAmount { get; set; }
    }
}

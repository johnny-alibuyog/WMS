using AmpedBiz.Common.CustomTypes;
using System;

namespace AmpedBiz.Service.Dto
{
    public class ReturnItem
    {
        public Guid Id { get; set; }

        public Guid ReturnId { get; set; }

        public Lookup<Guid> Product { get; set; }

        public Lookup<string> Reason { get; set; }

        public Measure Quantity { get; set; }

        public Measure Standard { get; set; }

        public decimal UnitPriceAmount { get; set; }

        public decimal TotalPriceAmount { get; set; }
    }

    public class ReturnItemPageItem
    {
        public Guid Id { get; set; }

        public Guid ReturnId { get; set; }

        public string ProductName { get; set; }

        public string ReasonName { get; set; }

        public decimal QuantityValue { get; set; }

        public decimal UnitPriceAmount { get; set; }

        public decimal TotalPriceAmount { get; set; }
    }
}

using AmpedBiz.Common.CustomTypes;
using System;

namespace AmpedBiz.Service.Dto
{
    public class OrderReturn
    {
        public virtual Guid OrderId { get; set; }

        public Lookup<Guid> Product { get; set; }

        public Lookup<string> Reason { get; set; }

        public virtual DateTime? ReturnedOn { get; set; }

        public virtual Lookup<Guid> ReturnedBy { get; set; }

        public decimal QuantityValue { get; set; }

        public decimal ReturnedAmount { get; set; }
    }

    public class OrderReturnable
    {
        public virtual Guid OrderId { get; set; }

        public Lookup<Guid> Product { get; set; }

        public decimal QuantityValue { get; set; }

        public decimal DiscountRate { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal UnitPriceAmount { get; set; }

        public decimal ExtendedPriceAmount { get; set; }

        public decimal TotalPriceAmount { get; set; }
    }
}

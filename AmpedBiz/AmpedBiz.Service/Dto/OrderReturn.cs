using AmpedBiz.Common.CustomTypes;
using System;

namespace AmpedBiz.Service.Dto
{
    public class OrderReturn
    {
        public virtual Guid OrderId { get; set; }

        public Lookup<string> Product { get; set; }

        public virtual DateTime? ReturnedOn { get; protected set; }

        public virtual Lookup<Guid> ReturnedBy { get; protected set; }

        public decimal QuantityValue { get; set; }

        public decimal DiscountRate { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal UnitPriceAmount { get; set; }

        public decimal ExtendedPriceAmount { get; set; }

        public decimal TotalPriceAmount { get; set; }
    }

    public class OrderReturnable
    {
        public virtual Guid OrderId { get; set; }

        public Lookup<string> Product { get; set; }

        public decimal QuantityValue { get; set; }

        public decimal DiscountRate { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal UnitPriceAmount { get; set; }

        public decimal ExtendedPriceAmount { get; set; }

        public decimal TotalPriceAmount { get; set; }
    }
}

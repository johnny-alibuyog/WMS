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

        public virtual decimal QuantityValue { get; protected set; }

        public virtual decimal UnitPriceAmount { get; protected set; }

        public virtual decimal TotalPriceAmount { get; protected set; }
    }
}

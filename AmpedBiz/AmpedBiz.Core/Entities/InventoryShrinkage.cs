using System;

namespace AmpedBiz.Core.Entities
{
    public class InventoryShrinkage : Entity<InventoryShrinkage, Guid>
    {
        public virtual Tenant Tenant { get; set; }

        public virtual DateTimeOffset? Date { get; set; }

        public virtual decimal? Quantity { get; set; }

        public virtual string Reason { get; set; }

        public virtual Product Product { get; set; }
    }
}
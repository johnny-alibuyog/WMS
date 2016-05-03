using System;

namespace AmpedBiz.Core.Entities
{
    public class InventoryShrinkage : Entity<Guid, InventoryShrinkage>
    {
        public virtual Tenant Tenant { get; set; }

        public virtual DateTime? Date { get; set; }

        public virtual decimal? Quantity { get; set; }

        public virtual string Reason { get; set; }

        public virtual Product Product { get; set; }

        public InventoryShrinkage() : this(default(Guid)) { }

        public InventoryShrinkage(Guid id) : base(id) { }
    }
}
using System;

namespace AmpedBiz.Core.Entities
{
    public class InventoryAdjustmentReason : Entity<Guid, InventoryAdjustmentReason>
    {
        public virtual Tenant Tenant { get; set; }

        public virtual string Name { get; internal protected set; }

        public virtual InventoryAdjustmentType Type { get; internal protected set; }

        public InventoryAdjustmentReason() : this(null, null, null) { }

        public InventoryAdjustmentReason(Tenant tenant, string name, InventoryAdjustmentType? type = null, Guid? id = null) : base(id ?? default(Guid)) { }
    }
}

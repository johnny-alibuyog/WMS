using AmpedBiz.Core.Common;
using System;

namespace AmpedBiz.Core.Inventories
{
    public class InventoryAdjustmentReason : Entity<Guid, InventoryAdjustmentReason>, IHasTenant
    {
        public virtual Tenant Tenant { get; set; }

        public virtual string Name { get; internal protected set; }

        public virtual string Description { get; internal protected set; }

        public virtual InventoryAdjustmentType Type { get; internal protected set; }

        public InventoryAdjustmentReason() : this(null, null) { }

        public InventoryAdjustmentReason(
            string name, 
            string description, 
            InventoryAdjustmentType type = InventoryAdjustmentType.Decrease, 
            Tenant tenant = null, 
            Guid? id = null) : base(id ?? default(Guid))
        {
            this.Name = name;
            this.Description = description;
            this.Type = type;
            this.Tenant = tenant;
        }
    }
}

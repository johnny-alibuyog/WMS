using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AmpedBiz.Core.Entities
{
    public class Inventory : Entity<Inventory, Guid>
    {
        public virtual Tenant Tenant { get; set; }

        public virtual double? ReorderLevel { get; set; }

        public virtual double? TargetLevel { get; set; }

        public virtual double? MinimumReorderQuantity { get; set; }

        public virtual double? Received { get; set; }

        public virtual double? OnOrder { get; set; }

        public virtual double? Shipped { get; set; }

        public virtual double? Allocated { get; set; }

        public virtual double? BackOrdered { get; set; }

        public virtual double? InitialLevel { get; set; }

        public virtual double? OnHand { get; set; }

        public virtual double? Available { get; set; }

        public virtual double? CurrentLevel { get; set; }

        public virtual double? BelowTargetLevel { get; set; }

        public virtual double? ReorderQuantity { get; set; }

        public virtual Product Product { get; set; }

        public virtual IEnumerable<InventoryShrinkage> Shrinkages { get; set; }

        public Inventory()
        {
            this.Shrinkages = new Collection<InventoryShrinkage>();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AmpedBiz.Core.Entities
{
    public class Inventory : Entity<string, Inventory>
    {
        public virtual Tenant Tenant { get; set; }

        public virtual UnitOfMeasure UnitOfMeasure { get; set; }

        public virtual UnitOfMeasure UnitOfMeasureBase { get; set; }

        public virtual decimal? ConvertionFactor { get; set; }

        public virtual Measure ReorderLevel { get; set; }

        public virtual Measure TargetLevel { get; set; }

        public virtual Measure MinimumReorderQuantity { get; set; }

        public virtual Measure Received { get; set; }

        public virtual Measure OnOrder { get; set; }

        public virtual Measure Shipped { get; set; }

        public virtual Measure Allocated { get; set; }

        public virtual Measure BackOrdered { get; set; }

        public virtual Measure InitialLevel { get; set; }

        public virtual Measure OnHand { get; set; }

        public virtual Measure Available { get; set; }

        public virtual Measure CurrentLevel { get; set; }

        public virtual Measure BelowTargetLevel { get; set; }

        public virtual Measure ReorderQuantity { get; set; }

        public virtual Product Product { get; set; }

        public virtual IEnumerable<InventoryShrinkage> Shrinkages { get; set; }

        public Inventory() : this(default(string)) { }

        public Inventory(string id) : base(id)
        {
            this.Shrinkages = new Collection<InventoryShrinkage>();
        }
    }
}
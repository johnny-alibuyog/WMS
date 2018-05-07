using System;

namespace AmpedBiz.Core.Entities
{
    public class InventoryShrinkage : Entity<Guid, InventoryShrinkage>
    {
        public virtual Inventory Inventory { get; internal protected set; }

        public virtual User ShrinkedBy { get; internal protected set; }

        public virtual DateTime ShrinkedOn { get; internal protected set; }

        public virtual InventoryShrinkageReason Reason { get; internal protected set; }

        public virtual string Remarks { get; internal protected set; }

        public virtual Money Shrinkage { get; internal protected set; }

        public InventoryShrinkage() : this(default(Guid)) { }

        public InventoryShrinkage(Guid id) : base(id) { }
    }
}

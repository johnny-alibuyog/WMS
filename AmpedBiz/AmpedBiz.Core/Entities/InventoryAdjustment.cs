using System;

namespace AmpedBiz.Core.Entities
{
    public enum InventoryAdjustmentType
    {
        Increase,
        Decrease
    }

    public class InventoryAdjustment : Entity<Guid, InventoryAdjustment>
    {
        public virtual Inventory Inventory { get; internal protected set; }

        public virtual User AdjustedBy { get; internal protected set; }

        public virtual DateTime AdjustedOn { get; internal protected set; }

        public virtual string Reason { get; internal protected set; }

        public virtual InventoryAdjustmentType AdjustmentType { get; internal protected set; }

        public virtual Measure Adjustment { get; internal protected set; }

        public InventoryAdjustment() : this(default(Guid)) { }

        public InventoryAdjustment(Guid id) : base(id) { }
    }
}

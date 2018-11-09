using AmpedBiz.Core.Products;
using AmpedBiz.Core.Users;
using System;

namespace AmpedBiz.Core.Inventories
{
    public enum InventoryAdjustmentType
    {
        Increase = 1,
        Decrease = 2
    }

    public class InventoryAdjustment : Entity<Guid, InventoryAdjustment>
    {
        public virtual Inventory Inventory { get; internal protected set; }

        public virtual User AdjustedBy { get; protected set; }

        public virtual DateTime AdjustedOn { get; protected set; }

        public virtual InventoryAdjustmentReason Reason { get; protected set; }

        public virtual string Remarks { get; protected set; }

        public virtual InventoryAdjustmentType Type { get; protected set; }

        public virtual Measure Quantity { get; protected set; }

        public virtual Measure Standard { get; protected set; }

        public virtual Measure QuantityStandardEquivalent { get; protected set; }

        public InventoryAdjustment() : base(default(Guid)) { }

        public InventoryAdjustment(
            User adjustedBy, 
            DateTime adjustedOn, 
            InventoryAdjustmentReason reason, 
            string remarks,
            InventoryAdjustmentType type, 
            Measure quantity, 
            Measure standard,
            Inventory inventory = null, 
            Guid? id = null) : base(id ?? default(Guid))
        {
            this.Inventory = inventory;
            this.AdjustedBy = adjustedBy;
            this.AdjustedOn = adjustedOn;
            this.Reason = reason;
            this.Remarks = remarks;
            this.Type = type;
            this.Quantity = quantity;
            this.Standard = standard;

            // quantity convertion to standard uom
            this.QuantityStandardEquivalent = standard * quantity;
        }
    }
}

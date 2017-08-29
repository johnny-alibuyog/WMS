using System;

namespace AmpedBiz.Core.Entities
{
    public class PurchaseOrderItem : Entity<Guid, PurchaseOrderItem>
    {
        public virtual PurchaseOrder PurchaseOrder { get; protected internal set; }

        public virtual Product Product { get; protected set; }

        public virtual Measure Quantity { get; protected set; }

        public virtual Measure Standard { get; protected set; }

        public virtual Measure QuantityStandardEquivalent { get; protected set; }

        public virtual Money UnitCost { get; protected set; }

        public virtual Money TotalCost { get; protected set; }

        public PurchaseOrderItem() : base(default(Guid)) { }

        public PurchaseOrderItem(
            Product product, 
            Money unitCost,
            Measure quantity,
            Measure standard,
            Guid? id = null
        ) : base(id ?? default(Guid))
        {
            this.Product = product;
            this.Quantity = quantity;
            this.Standard = standard;
            this.UnitCost = unitCost;

            // quantity convertion to standard uom
            this.QuantityStandardEquivalent = standard * quantity;

            this.TotalCost = new Money(this.UnitCost.Amount * this.Quantity.Value, this.UnitCost.Currency);
        }
    }
}
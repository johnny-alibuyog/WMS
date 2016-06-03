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

        public virtual Product Product { get; set; }

        public Inventory() : this(default(string)) { }

        public Inventory(string id) : base(id) { }
    }

    public class GoodStockInventory : Inventory
    {
        public virtual Measure ReorderLevel { get; set; }

        /// <summary>
        /// The number of items that you want to have on hand to accommodate the predicted level of orders.
        /// </summary>
        public virtual Measure TargetLevel { get; set; }

        public virtual Measure MinimumReorderQuantity { get; set; }

        public virtual Measure Received { get; set; }

        public virtual Measure OnOrder { get; set; }

        public virtual Measure Shipped { get; set; }

        /// <summary>
        /// The number of items that have been ordered by customers, but not yet shipped.
        /// </summary>
        public virtual Measure Allocated { get; set; }

        public virtual Measure BackOrdered { get; set; }

        public virtual Measure InitialLevel { get; set; }

        /// <summary>
        /// The number of items that you currently have in stock.
        /// </summary>
        public virtual Measure OnHand { get; set; }

        /// <summary>
        /// The difference between the number of items on hand and the number allocated.
        /// </summary>
        //public virtual Measure Available { get { return this.OnHand - this.Allocated; } }
        public virtual Measure Available { get; set; }

        /// <summary>
        /// This is the number of items that have been lost due to damage, spoilage, loss, and so on.
        /// </summary>
        public virtual Measure Shirnkage { get; set; }

        /// <summary>
        /// The number of available items minus the number of items on backorder, plus the number of items currently on order.
        /// </summary>
        //public virtual Measure CurrentLevel { get { return this.Available + this.OnOrder - this.BackOrdered; } }
        public virtual Measure CurrentLevel { get; set; }

        /// <summary>
        /// The current number of items at which you are below your target level.
        /// </summary>
        //public virtual Measure BelowTargetLevel { get { return this.CurrentLevel - } }
        public virtual Measure BelowTargetLevel { get; set; }

        public virtual Measure ReorderQuantity { get; set; }

        public virtual IEnumerable<InventoryShrinkage> Shrinkages { get; set; }

        public GoodStockInventory() : this(default(string)) { }

        public GoodStockInventory(string id) : base(id)
        {
            this.Shrinkages = new Collection<InventoryShrinkage>();
        }
    }

    public class BadStockInventory : Inventory
    {
        public virtual Measure OnHand { get; set; }
    }
}
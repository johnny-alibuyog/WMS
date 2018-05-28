using AmpedBiz.Core.Services;
using AmpedBiz.Core.Services.Products;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AmpedBiz.Core.Entities
{
    public class Inventory : Entity<Guid, Inventory>, IAccept<IVisitor<Inventory>>
    {
        public virtual Branch Branch { get; protected internal set; }

        public virtual Product Product { get; protected internal set; }

        public virtual Measure BadStock { get; protected internal set; }

        public virtual Measure Received { get; protected internal set; }

        public virtual Measure OnOrder { get; protected internal set; }

        public virtual Measure OnHand { get; protected internal set; } // The number of items that you currently have in stock.

        public virtual Measure Allocated { get; protected internal set; } // The number of items that have been ordered by customers, but not yet shipped.

        public virtual Measure Shipped { get; protected internal set; }

        public virtual Measure BackOrdered { get; protected internal set; }

        public virtual Measure Returned { get; protected internal set; }

        public virtual Measure Available { get; protected internal set; } // this.OnHand - this.Allocated

        public virtual Measure InitialLevel { get; protected internal set; }

        public virtual Measure Shrinkage { get; protected internal set; } // This is the number of items that have been lost due to damage, spoilage, loss, and so on.

        public virtual Measure CurrentLevel { get; protected internal set; } // this.Available + this.OnOrder - this.BackOrdered

        public virtual Measure TargetLevel { get; protected internal set; } // The number of items that you want to have on hand to accommodate the predicted level of orders.

        public virtual Measure BelowTargetLevel { get; protected internal set; } // this.TargetLevel - this.CurrentLevel // The current number of items at which you are below your target level. 

        public virtual Measure ReorderLevel { get; protected internal set; }

        public virtual Measure ReorderQuantity { get; protected internal set; }

        public virtual Measure MinimumReorderQuantity { get; protected internal set; }

        public virtual Measure IncreaseAdjustment { get; protected internal set; }

        public virtual Measure DecreaseAdjustment { get; protected internal set; }

        //public virtual IEnumerable<Stock> Stocks { get; protected internal set; } = new Collection<Stock>();

        public virtual IEnumerable<InventoryAdjustment> Adjustments { get; protected internal set; } = new Collection<InventoryAdjustment>();

        public virtual InventoryMeasureConverter Convert(Func<Inventory, Measure> selector) => new InventoryMeasureConverter(this, selector);

        public virtual IEnumerable<Measure> BreakDown(Func<Inventory, Measure> selector) => new InventoryMeasureBreaker(this, selector).BreakDown();

        public virtual void Accept(IVisitor<Inventory> visitor)
        {
            visitor.Visit(this);
        }

        public Inventory() : base(default(Guid)) { }

        public Inventory(Branch branch, Product product, Guid id = default(Guid)) : base(id)
        {
            this.Branch = branch;
            this.Product = product;
        }
    }
}
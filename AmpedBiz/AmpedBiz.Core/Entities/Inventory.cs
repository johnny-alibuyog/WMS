using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AmpedBiz.Core.Entities
{
    public class Inventory : Entity<Guid, Inventory>
    {
        public virtual Product Product { get; protected set; }

        public virtual string IndividualBarcode { get; set; }

        public virtual string PackagingBarcode { get; set; }

        public virtual UnitOfMeasure UnitOfMeasure { get; set; }

        public virtual UnitOfMeasure PackagingUnitOfMeasure { get; set; }

        public virtual decimal PackagingSize { get; set; }

        public virtual Money BasePrice { get; set; }

        public virtual Money DistributorPrice { get; set; }

        public virtual Money ListPrice { get; set; }

        public virtual Money BadStockPrice { get; set; }

        public virtual Measure BadStock { get; protected set; }

        public virtual Measure Received { get; protected set; }

        public virtual Measure OnOrder { get; protected set; }

        public virtual Measure OnHand { get; protected set; } // The number of items that you currently have in stock.

        public virtual Measure Allocated { get; protected set; } // The number of items that have been ordered by customers, but not yet shipped.

        public virtual Measure Shipped { get; protected set; }

        public virtual Measure BackOrdered { get; protected set; }

        public virtual Measure Available { get; protected set; } // this.OnHand - this.Allocated

        public virtual Measure InitialLevel { get; set; }

        public virtual Measure Shrinkage { get; protected set; } // This is the number of items that have been lost due to damage, spoilage, loss, and so on.

        public virtual Measure CurrentLevel { get; protected set; } // this.Available + this.OnOrder - this.BackOrdered

        public virtual Measure TargetLevel { get; set; } // The number of items that you want to have on hand to accommodate the predicted level of orders.

        public virtual Measure BelowTargetLevel { get; protected set; } // this.TargetLevel - this.CurrentLevel // The current number of items at which you are below your target level. 

        public virtual Measure ReorderLevel { get; set; }

        public virtual Measure ReorderQuantity { get; protected set; }

        public virtual Measure MinimumReorderQuantity { get; set; }

        public virtual IEnumerable<Stock> Stocks { get; protected set; } = new Collection<Stock>();

        public virtual void Order(Measure quantity) // purchase order
        {
            this.OnOrder += quantity;

            this.Compute();
        }

        public virtual void Receive(Measure quantity) // purchase order received
        {
            this.OnOrder -= quantity;
            this.OnHand += quantity;
            this.Received += quantity;

            this.Compute();
        }

        public virtual void BackOrder(Measure quantity)
        {
            this.BackOrdered += quantity;

            this.Compute();
        }

        public virtual void Allocate(Measure quantity)
        {
            this.Allocated += quantity;

            this.Compute();
        }

        public virtual void Ship(Measure quantity)
        {
            this.Shipped += quantity;
            this.OnHand -= quantity;
            this.Allocated -= quantity;

            this.Compute();
        }

        public virtual void Shrink(Measure quantity)
        {
            this.OnHand -= quantity;
            this.Shrinkage += quantity;

            this.Compute();
        }

        public virtual void Compute()
        {
            if (this.OnHand == null)
                this.OnHand = this.InitialLevel;

            this.Available = this.OnHand - this.Allocated;
            this.CurrentLevel = this.Available + this.OnOrder - this.BackOrdered;
            this.BelowTargetLevel = this.TargetLevel - this.CurrentLevel;

            if (this.BelowTargetLevel != null)
            {
                if (this.BelowTargetLevel.Value < 0M)
                    this.BelowTargetLevel.Value = 0M;

                if (this.BelowTargetLevel.Value != 0M)
                {
                    if (this.BelowTargetLevel < this.MinimumReorderQuantity)
                        this.ReorderQuantity = this.MinimumReorderQuantity;
                    else
                        this.ReorderQuantity = this.BelowTargetLevel;
                }
            }
        }

        public Inventory() : base(default(Guid)) { }

        public Inventory(Product product, Guid id = default(Guid)) : base(id)
        {
            this.Product = product;
        }
    }
}
﻿using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AmpedBiz.Core.Entities
{
    public class Inventory : Entity<string, Inventory>
    {
        //public virtual Tenant Tenant { get; set; }

        public virtual Product Product { get; set; }

        public virtual UnitOfMeasure UnitOfMeasure { get; set; }

        public virtual UnitOfMeasure UnitOfMeasureBase { get; set; }

        public virtual decimal? ConversionFactor { get; set; }

        public virtual Money BasePrice { get; set; }

        public virtual Money RetailPrice { get; set; }

        public virtual Money WholeSalePrice { get; set; }

        public virtual Measure Received { get; set; }

        public virtual Measure OnOrder { get; set; }

        public virtual Measure OnHand { get; set; } // The number of items that you currently have in stock.

        public virtual Measure Allocated { get; set; } // The number of items that have been ordered by customers, but not yet shipped.

        public virtual Measure Shipped { get; set; }

        public virtual Measure BackOrdered { get; set; }

        public virtual Measure Available { get; set; } // this.OnHand - this.Allocated

        public virtual Measure InitialLevel { get; set; }

        public virtual Measure Shrinkage { get; set; } // This is the number of items that have been lost due to damage, spoilage, loss, and so on.

        public virtual Measure CurrentLevel { get; set; } // this.Available + this.OnOrder - this.BackOrdered

        public virtual Measure TargetLevel { get; set; } // The number of items that you want to have on hand to accommodate the predicted level of orders.

        public virtual Measure BelowTargetLevel { get; set; } // this.TargetLevel - this.CurrentLevel // The current number of items at which you are below your target level. 

        public virtual Measure ReorderLevel { get; set; }

        public virtual Measure ReorderQuantity { get; set; }

        public virtual Measure MinimumReorderQuantity { get; set; }

        public virtual IEnumerable<Stock> Stocks { get; set; } = new Collection<Stock>();

        public virtual void Receive(Measure quantity)
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

        public virtual void ReceiveBackOrder(Measure quantity)
        {
            this.OnOrder -= quantity;
            this.BackOrdered -= quantity;
            this.OnHand += quantity;
            this.Received += quantity;

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

        private void Compute()
        {
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

        public Inventory() : base(default(string)) { }

        public Inventory(Product product, string id = null) : base(id)
        {
            this.Product = product;
        }
    }
}
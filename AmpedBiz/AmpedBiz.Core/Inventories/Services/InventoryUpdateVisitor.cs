using AmpedBiz.Core.Common;
using AmpedBiz.Core.Products;
using AmpedBiz.Core.Products.Services;
using System;

namespace AmpedBiz.Core.Inventories.Services
{
	public class InventoryUpdateVisitor : IVisitor<Inventory>
    {
        public Branch Branch { get; set; }

        public Product Product { get; set; }

        public virtual Measure InitialLevel { get; set; }

        public virtual Measure TargetLevel { get; set; }

        public virtual Measure ReorderLevel { get; set; }

        public virtual Measure MinimumReorderQuantity { get; set; }

        public virtual void Visit(Inventory target)
        {
            var Standardize = new Func<Measure, Measure>((value) => this.Product.ConvertToStandard(value));

            target.Branch = this.Branch ?? target.Branch;
            target.Product = this.Product ?? target.Product;
            target.InitialLevel = Standardize(this.InitialLevel) ?? target.InitialLevel;
            target.TargetLevel = Standardize(this.TargetLevel) ?? target.TargetLevel;
            target.ReorderLevel = Standardize(this.ReorderLevel) ?? target.ReorderLevel;
            target.MinimumReorderQuantity = Standardize(this.MinimumReorderQuantity) ?? target.MinimumReorderQuantity;
            target.Accept(new InventoryRecomputeVisitor());
        }
    }
}

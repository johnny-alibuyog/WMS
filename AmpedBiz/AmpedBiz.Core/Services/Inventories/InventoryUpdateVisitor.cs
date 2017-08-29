using AmpedBiz.Core.Entities;

namespace AmpedBiz.Core.Services.Inventories
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
            target.Branch = this.Branch ?? target.Branch;
            target.Product = this.Product ?? target.Product;
            target.InitialLevel = this.InitialLevel ?? target.InitialLevel;
            target.TargetLevel = this.TargetLevel ?? target.TargetLevel;
            target.ReorderLevel = this.ReorderLevel ?? target.ReorderLevel;
            target.MinimumReorderQuantity = this.MinimumReorderQuantity ?? target.MinimumReorderQuantity;
            target.Accept(new InventoryRecomputeVisitor());
        }
    }
}

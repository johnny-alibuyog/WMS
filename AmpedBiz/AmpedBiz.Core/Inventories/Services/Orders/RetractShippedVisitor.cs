using AmpedBiz.Core.Products;

namespace AmpedBiz.Core.Inventories.Services.Orders
{
	public class RetractShippedVisitor : IVisitor<Inventory>
    {
        public Measure QuantityStandardEquivalent { get; set; }

        public virtual void Visit(Inventory target)
        {
            target.OnHand += this.QuantityStandardEquivalent;
            target.Accept(new InventoryRecomputeVisitor());
        }
    }
}

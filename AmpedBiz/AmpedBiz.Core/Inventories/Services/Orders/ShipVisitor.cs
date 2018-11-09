using AmpedBiz.Core.Products;

namespace AmpedBiz.Core.Inventories.Services.Orders
{
	public class ShipVisitor : IVisitor<Inventory>
    {
        public Measure QuantityStandardEquivalent { get; set; }

        public virtual void Visit(Inventory target)
        {
            target.Shipped += this.QuantityStandardEquivalent;
            target.OnHand -= this.QuantityStandardEquivalent;
            target.Allocated -= this.QuantityStandardEquivalent;
            target.Accept(new InventoryRecomputeVisitor());
        }
    }
}

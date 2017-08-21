using AmpedBiz.Core.Entities;

namespace AmpedBiz.Core.Services.Inventories.Orders
{
    public class ShipVisitor : IVisitor<Inventory>
    {
        public Measure Quantity { get; set; }

        public virtual void Visit(Inventory target)
        {
            target.Shipped += this.Quantity;
            target.OnHand -= this.Quantity;
            target.Allocated -= this.Quantity;
            target.Accept(new InventoryRecomputeVisitor());
        }
    }
}

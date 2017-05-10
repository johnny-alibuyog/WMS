using AmpedBiz.Core.Entities;

namespace AmpedBiz.Core.Services.Inventories.Orders
{
    public class ShipVisitor : InventoryVisitor
    {
        public Measure Quantity { get; set; }

        public override void Visit(Inventory target)
        {
            target.Shipped += this.Quantity;
            target.OnHand -= this.Quantity;
            target.Allocated -= this.Quantity;
            target.Accept(new InventoryRecomputeVisitor());
        }
    }
}

using AmpedBiz.Core.Entities;

namespace AmpedBiz.Core.Services.Inventories.PurchaseOrders
{
    public class ReceiveVisitor : InventoryVisitor
    {
        public virtual Measure Quantity { get; set; }

        public override void Visit(Inventory target)
        {
            target.OnOrder -= this.Quantity;
            target.OnHand += this.Quantity;
            target.Received += this.Quantity;
            target.Accept(new InventoryRecomputeVisitor());
        }
    }
}

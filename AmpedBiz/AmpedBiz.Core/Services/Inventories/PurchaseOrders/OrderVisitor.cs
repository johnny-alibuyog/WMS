using AmpedBiz.Core.Entities;

namespace AmpedBiz.Core.Services.Inventories.PurchaseOrders
{
    public class OrderVisitor : InventoryVisitor
    {
        public virtual Measure Quantity { get; set; }

        public override void Visit(Inventory target)
        {
            target.OnOrder += this.Quantity;
            target.Accept(new InventoryRecomputeVisitor());
        }
    }
}

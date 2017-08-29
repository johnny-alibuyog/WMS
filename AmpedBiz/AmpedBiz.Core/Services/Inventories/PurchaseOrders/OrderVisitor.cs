using AmpedBiz.Core.Entities;

namespace AmpedBiz.Core.Services.Inventories.PurchaseOrders
{
    public class OrderVisitor : IVisitor<Inventory>
    {
        public virtual Measure QuantityStandardEquivalent { get; set; }

        public virtual void Visit(Inventory target)
        {
            target.OnOrder += this.QuantityStandardEquivalent;
            target.Accept(new InventoryRecomputeVisitor());
        }
    }
}

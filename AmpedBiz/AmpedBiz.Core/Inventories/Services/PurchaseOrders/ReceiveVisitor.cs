using AmpedBiz.Core.Products;
using AmpedBiz.Core.SharedKernel;

namespace AmpedBiz.Core.Inventories.Services.PurchaseOrders
{
    public class ReceiveVisitor : IVisitor<Inventory>
    {
        public virtual Measure QuantityStandardEquivalent { get; set; }

        public virtual void Visit(Inventory target)
        {
            target.OnOrder -= this.QuantityStandardEquivalent;
            target.OnHand += this.QuantityStandardEquivalent;
            target.Received += this.QuantityStandardEquivalent;
            target.Accept(new InventoryRecomputeVisitor());
        }
    }
}

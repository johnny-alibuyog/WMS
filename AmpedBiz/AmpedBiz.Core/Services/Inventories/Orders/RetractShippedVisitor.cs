using AmpedBiz.Core.Entities;

namespace AmpedBiz.Core.Services.Inventories.Orders
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

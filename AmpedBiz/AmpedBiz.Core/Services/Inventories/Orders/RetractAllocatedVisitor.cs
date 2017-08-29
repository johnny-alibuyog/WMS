using AmpedBiz.Core.Entities;

namespace AmpedBiz.Core.Services.Inventories.Orders
{
    public class RetractAllocatedVisitor : IVisitor<Inventory>
    {
        public Measure QuantityStandardEquivalent { get; set; }

        public virtual void Visit(Inventory target)
        {
            target.Allocated -= QuantityStandardEquivalent;
            target.Accept(new InventoryRecomputeVisitor());
        }
    }
}

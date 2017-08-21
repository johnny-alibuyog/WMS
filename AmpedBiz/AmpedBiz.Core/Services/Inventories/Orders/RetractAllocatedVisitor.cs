using AmpedBiz.Core.Entities;

namespace AmpedBiz.Core.Services.Inventories.Orders
{
    public class RetractAllocatedVisitor : IVisitor<Inventory>
    {
        public Measure Quantity { get; set; }

        public virtual void Visit(Inventory target)
        {
            target.Allocated -= Quantity;
            target.Accept(new InventoryRecomputeVisitor());
        }
    }
}

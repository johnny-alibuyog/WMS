using AmpedBiz.Core.Products;
using AmpedBiz.Core.SharedKernel;

namespace AmpedBiz.Core.Inventories.Services.Orders
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

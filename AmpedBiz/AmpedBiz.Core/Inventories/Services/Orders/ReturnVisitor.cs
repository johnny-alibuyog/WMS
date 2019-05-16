using AmpedBiz.Core.Products;
using AmpedBiz.Core.Returns;
using AmpedBiz.Core.SharedKernel;

namespace AmpedBiz.Core.Inventories.Services.Orders
{
	public class ReturnVisitor : IVisitor<Inventory>
    {
        public virtual Measure QuantityStandardEquivalent { get; set; }

        public virtual ReturnReason Reason { get; set; }

        public virtual void Visit(Inventory target)
        {
            // NOTE: identify if returned item based on reason will get back to inventory

            target.OnHand += this.QuantityStandardEquivalent;
            target.Returned += this.QuantityStandardEquivalent;
            target.Accept(new InventoryRecomputeVisitor());
        }
    }
}

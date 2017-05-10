using AmpedBiz.Core.Entities;

namespace AmpedBiz.Core.Services.Inventories.Orders
{
    public class ReturnVisitor : InventoryVisitor
    {
        public virtual Measure Quantity { get; set; }

        public virtual ReturnReason Reason { get; set; }

        public override void Visit(Inventory target)
        {
            // NOTE: identify if returned item based on reason will get back to inventory

            target.OnHand += this.Quantity;
            target.Returned += this.Quantity;
            target.Accept(new InventoryRecomputeVisitor());
        }
    }
}

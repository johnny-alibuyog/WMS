using AmpedBiz.Core.Entities;

namespace AmpedBiz.Core.Services.Inventories.PurchaseOrders
{
    public class RetractOrderedVisitor : IVisitor<Inventory>
    {
        public virtual Measure Remaining { get; set; }

        public virtual PurchaseOrderStatus Status { get; set; }

        public virtual void Visit(Inventory target)
        {
            if (this.Status < PurchaseOrderStatus.Approved)
                return;

            target.OnOrder -= this.Remaining;
            target.Accept(new InventoryRecomputeVisitor());
        }
    }
}

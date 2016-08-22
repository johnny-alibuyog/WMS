using AmpedBiz.Core.Entities;

namespace AmpedBiz.Core.Services.PurchaseOrders
{
    public abstract class PurchaseOrderVisitor : IVisitor<PurchaseOrder>
    {
        public abstract void Visit(PurchaseOrder target);
    }
}

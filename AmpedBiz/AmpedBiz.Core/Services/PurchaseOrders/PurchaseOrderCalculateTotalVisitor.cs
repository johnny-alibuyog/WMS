using AmpedBiz.Core.Entities;

namespace AmpedBiz.Core.Services.PurchaseOrders
{
    public class PurchaseOrderCalculateTotalVisitor : PurchaseOrderVisitor
    {
        public override void Visit(PurchaseOrder target)
        {
            var itemTotal = default(Money);

            foreach (var item in target.Items)
            {
                itemTotal += item.ExtendedCost;
            }

            target.Total = target.Tax + target.ShippingFee + target.Payment + itemTotal;
        }
    }
}

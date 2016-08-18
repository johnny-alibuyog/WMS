using AmpedBiz.Core.Entities;

namespace AmpedBiz.Core.Services.Orders
{
    public class OrderCalculateTotalVisitor : OrderVisitor
    {
        public override void Visit(Order target)
        {
            // calculate here
            if (target.Discount != null)
                target.Discount.Amount = 0M;

            if (target.SubTotal != null)
                target.SubTotal.Amount = 0M;

            if (target.Total != null)
                target.Total.Amount = 0M;

            foreach (var item in target.Items)
            {
                target.Discount += item.Discount;
                target.SubTotal += item.ExtendedPrice;
            }

            //TODO: how to compute for Tax
            target.Total = target.Tax + target.ShippingFee + target.SubTotal - target.Discount;
        }
    }
}

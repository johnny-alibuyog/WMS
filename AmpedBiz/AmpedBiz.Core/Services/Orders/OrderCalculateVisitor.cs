using AmpedBiz.Core.Entities;

namespace AmpedBiz.Core.Services.Orders
{
    public class OrderCalculateVisitor : OrderVisitor
    {
        public override void Visit(Order target)
        {
            // calculate here
            if (target.Discount != null)
                target.Discount.Amount = 0M;

            if (target.Returned != null)
                target.Returned.Amount = 0M;

            if (target.SubTotal != null)
                target.SubTotal.Amount = 0M;

            if (target.Total != null)
                target.Total.Amount = 0M;

            if (target.Paid != null)
                target.Paid.Amount = 0M;

            foreach (var item in target.Items)
            {
                target.Discount += item.Discount;
                target.SubTotal += item.ExtendedPrice;
            }

            foreach (var item in target.Returns)
            {
                target.Returned += item.Returned;
            }

            foreach(var item in target.Payments)
            {
                target.Paid += item.Payment;
            }

            target.Total = target.Tax + target.ShippingFee + target.SubTotal - target.Discount;
        }
    }
}

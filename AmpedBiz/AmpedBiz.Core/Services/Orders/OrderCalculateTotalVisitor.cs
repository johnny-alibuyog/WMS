using AmpedBiz.Core.Entities;
using System.Linq;

namespace AmpedBiz.Core.Services.Orders
{
    public class OrderCalculateTotalVisitor : OrderVisitor
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

            foreach (var item in target.Items)
            {
                var returnDiscount = (Money)null;
                var returnExtendedPrice = (Money)null;

                var returns = target.Returns.Where(x => x.Product == item.Product);
                var returnDiscountItems = returns.Where(x => x.Discount != null);
                var returnExtendedPriceItems = returns.Where(x => x.ExtendedPrice != null);

                if (returnDiscountItems.Any())
                {
                    returnDiscount = new Money(
                        amount: returnDiscountItems.Sum(x => x.Discount.Amount), 
                        currency: returnDiscountItems.First().Discount.Currency
                    );
                }
                
                if (returnExtendedPriceItems.Any())
                {
                    returnExtendedPrice = new Money(
                        amount: returnExtendedPriceItems.Sum(x => x.Discount.Amount),
                        currency: returnExtendedPriceItems.First().Discount.Currency
                    );
                }

                target.Discount += item.Discount - returnDiscount;
                target.SubTotal += item.ExtendedPrice - returnExtendedPrice;
            }

            //TODO: how to compute for Tax
            target.Total = target.Tax + target.ShippingFee + target.SubTotal - target.Discount;
        }
    }
}

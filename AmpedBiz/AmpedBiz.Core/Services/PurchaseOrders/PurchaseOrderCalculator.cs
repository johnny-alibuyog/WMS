using AmpedBiz.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Core.Services.PurchaseOrders
{
    public class PurchaseOrderCalculator
    {
        public Money Tax(PurchaseOrder target)
        {
            return target.Tax;
        }

        public Money ShippingFee(PurchaseOrder target)
        {
            return target.ShippingFee;
        }

        public Money Discount(PurchaseOrder target)
        {
            return null;
        }

        public Money SubTotal(PurchaseOrder target)
        {
            return target.Items.Sum(x => x.TotalCost);
        }

        public Money GrandTotal(PurchaseOrder target)
        {
            return target.Tax + target.ShippingFee + target.SubTotal - target.Discount;
        }

        public Measure Remaining(Product product, IEnumerable<PurchaseOrderItem> items, IEnumerable<PurchaseOrderReceipt> receipts)
        {
            var quantity = items
               .Where(x => x.Product == product)
               .Sum(x => x.Quantity);

            var received = receipts
                .Where(x => x.Product == product)
                .Sum(x => x.Quantity);

            var remaining = quantity - received;
            if (remaining == null)
                return null;

            //if (remaining.Value < 0)
            //    return new Measure(0M, remaining.Unit);

            return remaining;
        }
    }
}

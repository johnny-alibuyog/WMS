using AmpedBiz.Core.Common;
using AmpedBiz.Core.Products;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Core.PurchaseOrders.Services
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

        internal Money Paid(PurchaseOrder target)
        {
            return target.Payments.Sum(x => x.Payment);
        }

        public Money GrandTotal(PurchaseOrder target)
        {
            return target.Tax + target.ShippingFee + target.SubTotal - target.Discount;
        }

        public Measure Remaining(Product product, IEnumerable<PurchaseOrderItem> items, IEnumerable<PurchaseOrderReceipt> receipts)
        {
            var quantity = items
               .Where(x => x.Product == product)
               .Sum(x => x.QuantityStandardEquivalent);

            var received = receipts
                .Where(x => x.Product == product)
                .Sum(x => x.QuantityStandardEquivalent);

            var remaining = quantity - received;

            return remaining;
        }
    }
}

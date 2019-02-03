using AmpedBiz.Core.Common;
using AmpedBiz.Core.Products;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Core.PurchaseOrders.Services
{
    public class PurchaseOrderCalculator
    {
        public Money Tax(PurchaseOrder target) => target.Tax;

        public Money ShippingFee(PurchaseOrder target) => target.ShippingFee;

        public Money Discount(PurchaseOrder target) => null;

        public Money SubTotal(PurchaseOrder target) => target.Items.Sum(x => x.TotalCost);

        internal Money Paid(PurchaseOrder target) => target.Payments.Sum(x => x.Payment);

        public Money GrandTotal(PurchaseOrder target) => this.Tax(target) + this.ShippingFee(target) + this.SubTotal(target) - this.Discount(target);

		public Money Balance(PurchaseOrder target)
		{
			var total = this.GrandTotal(target);
			var paid = this.Paid(target);

			var balance = total - paid;
			if (balance.Amount < 0)
				balance.Amount = 0;

			return balance;
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

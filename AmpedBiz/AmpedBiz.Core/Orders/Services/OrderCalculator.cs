using AmpedBiz.Core.Common;
using System.Linq;

namespace AmpedBiz.Core.Orders.Services
{
	public class OrderCalculator
    {
        public Money Tax(Order target) => target.Tax;

        public Money ShippingFee(Order target) => target.ShippingFee;

        public Money Discount(Order target) => target.Items.Sum(x => x.Discount);

        public Money SubTotal(Order target) => target.Items.Sum(x => x.TotalPrice);

        internal Money Paid(Order target) => target.Payments.Sum(x => x.Payment);

		internal Money Balance(Order target)
		{
			var total = this.GrandTotal(target);
			var paid = this.Paid(target);

			var balance = total - paid;
			if (balance.Amount < 0)
				balance.Amount = 0;

			return balance;
		}

        internal Money Returned(Order target) => target.Returns.Sum(x => x.Returned);

        internal Money GrandTotal(Order target) => this.Tax(target) + this.ShippingFee(target) + this.SubTotal(target) - this.Discount(target);
    }
}

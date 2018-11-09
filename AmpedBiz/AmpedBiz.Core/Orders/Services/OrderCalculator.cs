using AmpedBiz.Core.Common;
using System.Linq;

namespace AmpedBiz.Core.Orders.Services
{
	public class OrderCalculator
    {
        public Money Tax(Order target)
        {
            return target.Tax;
        }

        public Money ShippingFee(Order target)
        {
            return target.ShippingFee;
        }

        public Money Discount(Order target)
        {
            return target.Items.Sum(x => x.Discount);
        }

        public Money SubTotal(Order target)
        {
            return target.Items.Sum(x => x.TotalPrice);
        }

        internal Money Paid(Order target)
        {
            return target.Payments.Sum(x => x.Payment);
        }

        public Money Returned(Order target)
        {
            return target.Returns.Sum(x => x.Returned);
        }

        public Money GrandTotal(Order target)
        {
            return target.Tax + target.ShippingFee + target.SubTotal - target.Discount;
        }
    }
}

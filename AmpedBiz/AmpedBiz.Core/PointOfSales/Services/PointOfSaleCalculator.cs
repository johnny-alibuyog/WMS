using AmpedBiz.Core.Common;
using System.Linq;

namespace AmpedBiz.Core.PointOfSales.Services
{
	internal class PointOfSaleCalculator
	{
		public Money Discount(PointOfSale target)
		{
			return target.Items.Sum(x => x.Discount);
		}

		public Money SubTotal(PointOfSale target)
		{
			return target.Items.Sum(x => x.TotalPrice);
		}

		internal Money Paid(PointOfSale target)
		{
			return target.Payments.Sum(x => x.Payment);
		}

		public Money GrandTotal(PointOfSale target)
		{
			return target.SubTotal - target.Discount;
		}
	}
}

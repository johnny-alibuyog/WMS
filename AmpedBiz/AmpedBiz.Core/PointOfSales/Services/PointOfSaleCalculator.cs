using AmpedBiz.Core.Common;
using System.Linq;

namespace AmpedBiz.Core.PointOfSales.Services
{
	internal class PointOfSaleCalculator
	{
		internal Money Discount(PointOfSale target) => target.Items.Sum(x => x.Discount);

		internal Money SubTotal(PointOfSale target) => target.Items.Sum(x => x.TotalPrice);

		internal Money GrandTotal(PointOfSale target) => this.SubTotal(target) - this.Discount(target);

		internal Money Received(PointOfSale target) => target.Payments.Sum(x => x.Payment);

		internal Money Change(PointOfSale target) => this.Received(target) - this.GrandTotal(target);

		internal Money Paid(PointOfSale target) => this.Received(target) - this.Change(target);

		internal Money Balance(PointOfSale target)
		{
			var total = this.GrandTotal(target);
			var paid = this.Paid(target);

			var balance = total - paid;
			if (balance.Amount < 0)
				balance.Amount = 0;

			return balance;
		}

		internal PointOfSaleStatus Status(PointOfSale target)
		{
			var paid = this.Paid(target);
			var balance = this.Balance(target);

			if (paid == null || paid.Amount == 0)
				return PointOfSaleStatus.UnPaid;
			else if (balance != null && balance.Amount > 0)
				return PointOfSaleStatus.PartiallyPaid;
			else
				return PointOfSaleStatus.FullyPaid;
		}
	}
}

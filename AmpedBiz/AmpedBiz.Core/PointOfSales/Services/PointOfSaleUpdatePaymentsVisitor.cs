using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Common;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Core.PointOfSales.Services
{
	public class PointOfSaleUpdatePaymentsVisitor : IVisitor<PointOfSale>
	{
		public virtual IEnumerable<PointOfSalePayment> Payments { get; set; }

		public PointOfSaleUpdatePaymentsVisitor(IEnumerable<PointOfSalePayment> payments)
		{
			this.Payments = payments;
		}

		public void Visit(PointOfSale target)
		{
			if (this.Payments.IsNullOrEmpty())
				return;

			// allow only insert. edit and delete is not allowed for this aggregate
			var itemsToInsert = this.Payments.Except(target.Payments).ToList();

			foreach (var item in itemsToInsert)
			{
				item.PointOfSale = target;
				item.Balance = target.Total - target.Payments.Sum(x => x.Payment);
				target.Payments.Add(item);
			}
		}
	}
}

using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Inventories.Services.PointOfSale;
using AmpedBiz.Core.Products.Services;
using AmpedBiz.Core.SharedKernel;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Core.PointOfSales.Services
{
	public class PointOfSaleUpdateItemsVisitor : IVisitor<PointOfSale>
	{
		public virtual IEnumerable<PointOfSaleItem> Items { get; set; }

		public PointOfSaleUpdateItemsVisitor(IEnumerable<PointOfSaleItem> items)
		{
			this.Items = items;
		}

		public void Visit(PointOfSale target)
		{
			if (this.Items.IsNullOrEmpty())
				return;

			var itemsToInsert = this.Items.Except(target.Items).ToList();

			foreach (var item in itemsToInsert)
			{
				item.PointOfSale = target;
				item.Product.Accept(new SearchAndApplyVisitor()
				{
					Branch = target.Branch,
					InventoryVisitor = new TenderedVisitor(item.QuantityStandardEquivalent)
				});
				target.Items.Add(item);
			}
		}
	}
}

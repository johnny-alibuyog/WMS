using AmpedBiz.Core.Products;
using AmpedBiz.Core.SharedKernel;

namespace AmpedBiz.Core.Inventories.Services.PointOfSale
{
	public class TenderedVisitor : IVisitor<Inventory>
	{
		public Measure QuantityStandardEquivalent { get; private set; }

		public TenderedVisitor(Measure quantityStandardEquivalent)
		{
			this.QuantityStandardEquivalent = quantityStandardEquivalent;
		}

		public virtual void Visit(Inventory target)
		{
			target.OnHand -= this.QuantityStandardEquivalent;
			target.Accept(new InventoryRecomputeVisitor());
		}
	}
}

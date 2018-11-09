namespace AmpedBiz.Core.PointOfSales.Services
{
	public class PointOfSaleCalculateVisitor : IVisitor<PointOfSale>
	{
		public void Visit(PointOfSale target)
		{
			var calculator = new PointOfSaleCalculator();
			target.SubTotal = calculator.SubTotal(target);
			target.Discount = calculator.Discount(target);
			target.Total = calculator.GrandTotal(target);
			target.Paid = calculator.Paid(target);
		}
	}
}

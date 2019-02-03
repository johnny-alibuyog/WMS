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
			target.Received = calculator.Received(target);
			target.Change = calculator.Change(target);
			target.Paid = calculator.Paid(target);
			target.Balance = calculator.Balance(target);
			target.Status = calculator.Status(target);
		}
	}
}

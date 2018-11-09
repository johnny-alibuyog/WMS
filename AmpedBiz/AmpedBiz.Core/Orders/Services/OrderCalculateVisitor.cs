namespace AmpedBiz.Core.Orders.Services
{
    public class OrderCalculateVisitor : IVisitor<Order>
    {
        public virtual void Visit(Order target)
        {
            var calculator = new OrderCalculator();
            target.Tax = calculator.Tax(target);
            target.ShippingFee = calculator.ShippingFee(target);
            target.SubTotal = calculator.SubTotal(target);
            target.Discount = calculator.Discount(target);
            target.Total = calculator.GrandTotal(target);
            target.Paid = calculator.Paid(target);
            target.Returned = calculator.Returned(target);
        }
    }
}

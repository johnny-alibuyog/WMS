using AmpedBiz.Core.Entities;

namespace AmpedBiz.Core.Services.PurchaseOrders
{
    public class PurchaseOrderCalculateVisitor : PurchaseOrderVisitor
    {
        public override void Visit(PurchaseOrder target)
        {
            var calculator = new PurchaseOrderCalculator();
            target.Tax = calculator.Tax(target);
            target.ShippingFee = calculator.ShippingFee(target);
            target.SubTotal = calculator.SubTotal(target);
            target.Discount = calculator.Discount(target);
            target.Total = calculator.GrandTotal(target); ;
            target.Paid = calculator.Paid(target);
        }
    }
}

﻿using AmpedBiz.Core.Entities;

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

            //var totalCost = default(Money);

            //foreach (var item in target.Items)
            //{
            //    totalCost += item.TotalCost;
            //}

            //target.Total = target.Tax + target.ShippingFee + totalCost;

            //// calculate here
            //if (target.Discount != null)
            //    target.Discount.Amount = 0M;

            //if (target.SubTotal != null)
            //    target.SubTotal.Amount = 0M;

            //if (target.Paid != null)
            //    target.Paid.Amount = 0M;

            //if (target.Total != null)
            //    target.Total.Amount = 0M;

            //foreach (var item in target.Items)
            //{
            //    //target.Discount += item.Discount;
            //    target.SubTotal += item.TotalCost;
            //}

            //foreach (var item in target.Payments)
            //{
            //    target.Paid += item.Payment;
            //}

            //target.Total = target.Tax + target.ShippingFee + target.SubTotal - target.Discount;
        }
    }
}

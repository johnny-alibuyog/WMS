using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Common;
using AmpedBiz.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Core.Orders.Services
{
	public class OrderUpdatePaymentsVisitor : IVisitor<Order>
    {
        public virtual IEnumerable<OrderPayment> Payments { get; set; }

        public OrderUpdatePaymentsVisitor(IEnumerable<OrderPayment> payments)
        {
            this.Payments = payments;
        }

        public virtual void Visit(Order target)
        {
            if (this.Payments.IsNullOrEmpty())
                return;

            // allow only insert. edit and delete is not allowed for this aggregate
            var itemsToInsert = this.Payments.Except(target.Payments).ToList();

            foreach (var item in itemsToInsert)
            {
                item.Order = target;
                item.Balance = target.Total - target.Payments.Sum(x => x.Payment);
                target.Payments.Add(item);
            }

            var lastPayment = target.Payments.OrderBy(x => x.PaymentOn).Last();

            target.PaymentOn = lastPayment.PaymentOn;
            target.PaymentBy = lastPayment.PaymentBy;

            if (itemsToInsert.Any())
            {
                target.Accept(new OrderLogTransactionVisitor(
                    transactedBy: lastPayment.PaymentBy,
                    transactedOn: lastPayment.PaymentOn ?? DateTime.Now,
                    type: OrderTransactionType.PaymentCreation
                ));
            }
        }
    }
}

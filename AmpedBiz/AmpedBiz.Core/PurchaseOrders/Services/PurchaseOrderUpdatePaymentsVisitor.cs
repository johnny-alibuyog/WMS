using AmpedBiz.Common.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Core.PurchaseOrders.Services
{
	public class PurchaseOrderUpdatePaymentsVisitor : IVisitor<PurchaseOrder>
    {
        public virtual IEnumerable<PurchaseOrderPayment> Payments { get; set; }

        public PurchaseOrderUpdatePaymentsVisitor(IEnumerable<PurchaseOrderPayment> payments)
        {
            this.Payments = payments;
        }

        public virtual void Visit(PurchaseOrder target)
        {
            if (this.Payments.IsNullOrEmpty())
                return;

            // allow only insert. edit and delete is not allowed for this aggregate
            var itemsToInsert = this.Payments.Except(target.Payments).ToList();

            foreach (var item in itemsToInsert)
            {
                item.PurchaseOrder = target;
                target.Payments.Add(item);
            }

            var lastPayment = target.Payments.OrderBy(x => x.PaidOn).Last();

            target.PaidOn = lastPayment.PaidOn;
            target.PaidBy = lastPayment.PaidBy;

            if (itemsToInsert.Any())
            {
                target.Accept(new PurchaseOrderLogTransactionVisitor(
                    transactedBy: lastPayment.PaidBy,
                    transactedOn: lastPayment.PaidOn ?? DateTime.Now,
                    type: PurchaseOrderTransactionType.PaymentCreation
                ));
            }

        }
    }
}

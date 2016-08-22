using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using System.Linq;
using System.Collections.Generic;

namespace AmpedBiz.Core.Services.PurchaseOrders
{
    public class PurchaseOrderPaidVisitor : PurchaseOrderVisitor
    {
        public virtual IEnumerable<PurchaseOrderPayment> Payments { get; set; }

        public override void Visit(PurchaseOrder target)
        {
            this.AddPaymentsTo(target);
            target.Status = PurchaseOrderStatus.Paid;
        }

        private void AddPaymentsTo(PurchaseOrder target)
        {
            var lastPayment = this.Payments.OrderBy(x => x.PaidOn).LastOrDefault();
            if (lastPayment == null)
                return;

            target.PaidBy = lastPayment.PaidBy;
            target.PaidOn = lastPayment.PaidOn;
            target.PaymentType = lastPayment.PaymentType;
            target.Payment += lastPayment.Payment;

            foreach (var payment in this.Payments)
            {
                payment.PurchaseOrder = target;
                target.Payments.Add(payment);
            }
        }

    }
}

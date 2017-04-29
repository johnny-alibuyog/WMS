using AmpedBiz.Core.Entities;
using AmpedBiz.Common.Extentions;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Core.Services.Orders
{
    public class OrderUpdatePaymentsVisitor : OrderVisitor
    {
        public virtual IEnumerable<OrderPayment> Payments { get; set; }

        public OrderUpdatePaymentsVisitor(IEnumerable<OrderPayment> payments)
        {
            this.Payments = payments;
        }

        public override void Visit(Order target)
        {
            if (this.Payments.IsNullOrEmpty())
                return;

            // allow only insert. edit and delete is not allowed for this aggregate
            var itemsToInsert = this.Payments.Except(target.Payments).ToList();

            foreach (var item in itemsToInsert)
            {
                item.Order = target;
                target.Payments.Add(item);
            }

            var lastPayment = target.Payments.OrderBy(x => x.PaidOn).Last();

            target.PaidOn = lastPayment.PaidOn;
            target.PaidTo = lastPayment.PaidTo;
        }
    }
}

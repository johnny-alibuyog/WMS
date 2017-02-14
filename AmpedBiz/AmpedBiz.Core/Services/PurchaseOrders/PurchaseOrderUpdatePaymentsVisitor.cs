using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using System.Linq;
using System.Collections.Generic;

namespace AmpedBiz.Core.Services.PurchaseOrders
{
    public class PurchaseOrderUpdatePaymentsVisitor : PurchaseOrderVisitor
    {
        public virtual IEnumerable<PurchaseOrderPayment> Payments { get; set; }

        public PurchaseOrderUpdatePaymentsVisitor(IEnumerable<PurchaseOrderPayment> payments)
        {
            this.Payments = payments;
        }

        public override void Visit(PurchaseOrder target)
        {
            if (this.Payments.IsNullOrEmpty())
                return;

            var itemsToInsert = this.Payments.Except(target.Payments).ToList();
            var itemsToUpdate = target.Payments.Where(x => this.Payments.Contains(x)).ToList();
            var itemsToRemove = target.Payments.Except(this.Payments).ToList();

            foreach (var item in itemsToInsert)
            {
                item.PurchaseOrder = target;
                target.Payments.Add(item);
            }

            foreach (var item in itemsToUpdate)
            {
                var value = this.Payments.Single(x => x == item);
                item.SerializeWith(value);
                item.PurchaseOrder = target;
            }

            foreach (var item in itemsToRemove)
            {
                item.PurchaseOrder = null;
                target.Payments.Remove(item);
            }

            var lastPayment = target.Payments.OrderBy(x => x.PaidOn).Last();

            target.PaidOn = lastPayment.PaidOn;
            target.PaidBy = lastPayment.PaidBy;
        }
    }
}

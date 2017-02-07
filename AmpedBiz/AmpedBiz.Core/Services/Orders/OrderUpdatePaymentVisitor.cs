using AmpedBiz.Core.Entities;
using AmpedBiz.Common.Extentions;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Core.Services.Orders
{
    public class OrderUpdatePaymentVisitor : OrderVisitor
    {
        public virtual IEnumerable<OrderPayment> Payments { get; set; }

        public OrderUpdatePaymentVisitor(IEnumerable<OrderPayment> payments)
        {
            this.Payments = payments;
        }

        public override void Visit(Order target)
        {
            if (this.Payments.IsNullOrEmpty())
                return;

            var itemsToInsert = this.Payments.Except(target.Payments).ToList();
            var itemsToUpdate = target.Payments.Where(x => this.Payments.Contains(x)).ToList();
            var itemsToRemove = target.Payments.Except(this.Payments).ToList();

            foreach (var item in itemsToInsert)
            {
                item.Order = target;
                target.Payments.Add(item);
            }

            foreach (var item in itemsToUpdate)
            {
                var value = this.Payments.Single(x => x == item);
                item.SerializeWith(value);
                item.Order = target;
            }

            foreach (var item in itemsToRemove)
            {
                item.Order = null;
                target.Payments.Remove(item);
            }

            var lastPayment = target.Payments.OrderBy(x => x.PaidOn).Last();

            target.PaidOn = lastPayment.PaidOn;
            target.PaidTo = lastPayment.PaidBy;

            //this.AddPaymentsTo(target);
            //target.Status = OrderStatus.Paid;
        }

        //private void SetPaymentsTo(Order target)
        //{
        //    if (this.Payments.IsNullOrEmpty())
        //        return;

        //    var itemsToInsert = this.Payments.Except(target.Payments).ToList();
        //    var itemsToUpdate = target.Payments.Where(x => this.Payments.Contains(x)).ToList();
        //    var itemsToRemove = target.Payments.Except(this.Payments).ToList();

        //    foreach (var item in itemsToInsert)
        //    {
        //        item.Order = target;
        //        target.Payments.Add(item);
        //    }

        //    foreach (var item in itemsToUpdate)
        //    {
        //        var value = this.Payments.Single(x => x == item);
        //        item.SerializeWith(value);
        //        item.Order = target;
        //    }

        //    foreach (var item in itemsToRemove)
        //    {
        //        item.Order = null;
        //        target.Payments.Remove(item);
        //    }
        //}

        //private void AddPaymentsTo(Order target)
        //{
        //    var lastPayment = this.Payments.OrderBy(x => x.PaidOn).LastOrDefault();
        //    if (lastPayment == null)
        //        return;

        //    target.InvoicedOn = lastPayment.PaidOn;
        //    target.InvoicedBy = lastPayment.PaidBy;

        //    foreach (var payment in this.Payments)
        //    {
        //        payment.Order = target;
        //        target.Payments.Add(payment);
        //    }
        //}
    }
}

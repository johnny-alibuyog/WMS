using AmpedBiz.Core.Entities;
using System;

namespace AmpedBiz.Core.Services.Orders
{
    public class OrderInvoicedVisitor : OrderVisitor
    {
        public virtual DateTime? InvoicedOn { get; set; }

        public virtual User InvoicedBy { get; set; }

        public override void Visit(Order target)
        {
            foreach (var item in target.Items)
            {
                item.Invoiced();
            }

            target.InvoicedOn = this.InvoicedOn;
            target.InvoicedBy = this.InvoicedBy;
            target.Status = OrderStatus.Invoiced;
        }
    }
}

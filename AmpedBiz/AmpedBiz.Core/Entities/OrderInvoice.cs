using System;

namespace AmpedBiz.Core.Entities
{
    public class OrderInvoice : Entity<Guid, OrderInvoice>
    {
        public virtual Order Order { get; protected internal set; }

        public virtual DateTime? InvoicedOn { get; protected set; }

        public virtual User InvoicedBy { get; protected set; }

        public virtual DateTime? DueOn { get; protected set; }

        public virtual Money Tax { get; protected set; }

        public virtual Money Shipping { get; protected set; }

        public virtual Money Discount { get; protected set; }

        public virtual Money SubTotal { get; protected set; }

        public virtual Money Total { get; protected set; }

        public OrderInvoice() : base(default(Guid)) { }

        public OrderInvoice(DateTime? invoicedOn, User invoicedBy, DateTime? dueOn, Money tax, 
            Money shipping, Money dicount, Money subTotal, Guid? id = null) : base(id ?? default(Guid))
        {
            this.InvoicedOn = invoicedOn;
            this.InvoicedBy = invoicedBy;
            this.DueOn = dueOn;
            this.Tax = tax;
            this.Shipping = shipping;
            this.Discount = dicount;
            this.SubTotal = subTotal;
            this.Total = this.SubTotal + this.Tax + this.Shipping - this.Discount;
        }
    }
}

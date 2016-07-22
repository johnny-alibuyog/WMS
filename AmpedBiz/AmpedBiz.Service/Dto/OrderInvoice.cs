using AmpedBiz.Common.CustomTypes;
using System;

namespace AmpedBiz.Service.Dto
{
    public class OrderInvoice
    {
        public Guid OrderId { get; set; }

        public virtual DateTime? InvoicedOn { get; set; }

        public virtual Lookup<Guid> InvoicedBy { get; set; }

        public virtual DateTime? DueOn { get; set; }

        public virtual decimal TaxAmount { get; set; }

        public virtual decimal ShippingAmount { get; set; }

        public virtual decimal DiscountAmount { get; set; }

        public virtual decimal SubTotalAmount { get; set; }

        public virtual decimal TotalAmount { get; set; }
    }
}

using System;

namespace AmpedBiz.Service.Dto
{
    public class Invoice
    {
        public Guid Id { get; set; }

        public Order Order { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public DateTime? DueDate { get; set; }

        public decimal? TaxAmount { get; set; }

        public decimal? ShippingAmount { get; set; }

        public decimal? SubTotalAmount { get; set; }

        public decimal? TotalAmount { get; set; }
    }
}
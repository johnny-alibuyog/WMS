using AmpedBiz.Common.CustomTypes;
using AmpedBiz.Core.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AmpedBiz.Service.Dto
{
    public class OrderInvoiceDetail
    {
        public virtual string CustomerName { get; set; }

        public virtual string InvoiceNumber { get; set; }

        public virtual DateTime InvoicedOn { get; set; }

        public virtual string InvoicedByName { get; set; }

        public virtual string PricingName { get; set; }

        public virtual string PaymentTypeName { get; set; }

        public virtual string BranchName { get; set; }

        public virtual DateTime OrderedOn { get; set; }

        public virtual string OrderedByName { get; set; }

        public virtual DateTime ShippedOn { get; set; }

        public virtual string ShippedByName { get; set; }

        public virtual Address ShippingAddress { get; set; }

        public virtual decimal TaxAmount { get; set; }

        public virtual decimal ShippingFeeAmount { get; set; }

        public virtual decimal DiscountAmount { get; set; }

        public virtual decimal ReturnedAmount { get; set; }

        public virtual decimal SubTotalAmount { get; set; }

        public virtual decimal TotalAmount { get; set; }

        public virtual IEnumerable<OrderInvoiceDetailItem> Items { get; set; } = new Collection<OrderInvoiceDetailItem>();
    }

    public class OrderInvoiceDetailItem
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        public Lookup<Guid> Product { get; set; }

        public decimal QuantityValue { get; set; }

        public decimal DiscountRate { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal UnitPriceAmount { get; set; }

        public decimal ExtendedPriceAmount { get; set; }

        public decimal TotalPriceAmount { get; set; }
    }
}

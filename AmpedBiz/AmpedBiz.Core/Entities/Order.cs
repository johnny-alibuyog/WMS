using System;
using System.Collections.Generic;

namespace AmpedBiz.Core.Entities
{
    public class Order : Entity<Order, Guid>
    {
        public Order()
        {
            this.Invoices = new HashSet<Invoice>();
            this.OrderDetails = new HashSet<OrderDetail>();
        }

        public virtual Tenant Tenant
        {
            get;
            set;
        }

        public virtual DateTimeOffset? OrderDate
        {
            get;
            set;
        }

        public virtual DateTimeOffset? ShippedDate
        {
            get;
            set;
        }

        public virtual Shipper Shipper
        {
            get;
            set;
        }

        public virtual Money ShippingFee
        {
            get;
            set;
        }

        public virtual Money Tax
        {
            get;
            set;
        }

        public virtual PaymentType PaymentType
        {
            get;
            set;
        }

        public virtual DateTimeOffset? PaymentDate
        {
            get;
            set;
        }

        public virtual double? TaxRate
        {
            get;
            set;
        }

        public virtual double? OrderMonth
        {
            get;
            set;
        }

        public virtual double? OrderYear
        {
            get;
            set;
        }

        public virtual Money OrderSubTotal
        {
            get;
            set;
        }

        public virtual Money OrderTotal
        {
            get;
            set;
        }

        public virtual DateTimeOffset? ClosedDate
        {
            get;
            set;
        }

        public virtual double? OrderQuarter
        {
            get;
            set;
        }

        public virtual OrderStatus Status
        {
            get;
            set;
        }

        public virtual bool IsNew
        {
            get;
            set;
        }

        public virtual bool IsCompleted
        {
            get;
            set;
        }

        public virtual bool IsShipped
        {
            get;
            set;
        }

        public virtual bool IsInvoiced
        {
            get;
            set;
        }

        public virtual bool IsActive
        {
            get;
            set;
        }

        public virtual IEnumerable<Invoice> Invoices
        {
            get;
            set;
        }

        public virtual IEnumerable<OrderDetail> OrderDetails
        {
            get;
            set;
        }

        public virtual Employee Employee
        {
            get;
            set;
        }

        public virtual Customer Customer
        {
            get;
            set;
        }
    }
}
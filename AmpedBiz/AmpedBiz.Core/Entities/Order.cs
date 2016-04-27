using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AmpedBiz.Core.Entities
{
    public enum OrderStatus
    {
        New,
        Invoiced,
        Shipped,
        Completed,
        Cancelled
    }

    public class Order : Entity<Order, Guid>
    {
        public virtual Tenant Tenant { get; set; }

        public virtual DateTimeOffset? OrderDate { get; private set; }

        public virtual DateTimeOffset? ShippedDate { get; private set; }

        public virtual DateTimeOffset? PaymentDate { get; private set; }

        public virtual DateTimeOffset? ClosedDate { get; private set; }

        public virtual PaymentType PaymentType { get; private set; }

        public virtual Shipper Shipper { get; private set; }

        public virtual double? TaxRate { get; private set; }

        public virtual Money Tax { get; private set; }

        public virtual Money ShippingFee { get; private set; }

        public virtual Money SubTotal { get; private set; }

        public virtual Money Total { get; private set; }

        public virtual OrderStatus Status { get; private set; }

        public virtual bool IsActive { get; private set; }

        public virtual Employee Employee { get; private set; }

        public virtual Customer Customer { get; private set; }

        public virtual IEnumerable<Invoice> Invoices { get; private set; }

        public virtual IEnumerable<OrderDetail> OrderDetails { get; private set; }

        public Order()
        {
            this.Invoices = new Collection<Invoice>();
            this.OrderDetails = new HashSet<OrderDetail>();
        }

        public virtual void New()
        {
            this.Status = OrderStatus.New;
        }

        public virtual void Invoice(Employee employee, Invoice invoice)
        {
            invoice.Order = this;
            ((Collection<Invoice>)this.Invoices).Add(invoice);
            this.Status = OrderStatus.Invoiced;
        }

        public virtual void Ship(DateTimeOffset shipDate)
        {
            this.Status = OrderStatus.Shipped;

        }

        public virtual void Completed()
        {
            this.Status = OrderStatus.Completed;
        }

        public virtual void Cancelled()
        {
            this.Status = OrderStatus.Cancelled;
        }

        public virtual void AddOrderDetail(OrderDetail orderDetail)
        {
            orderDetail.Order = this;
            ((Collection<OrderDetail>)this.OrderDetails).Add(orderDetail);
        }
    }
}
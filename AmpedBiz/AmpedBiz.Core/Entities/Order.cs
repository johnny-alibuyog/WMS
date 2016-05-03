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

    public class Order : Entity<Guid, Order>
    {
        public virtual Branch Branch { get; set; }

        public virtual DateTime? OrderDate { get; protected set; }

        public virtual DateTime? ShippedDate { get; protected set; }

        public virtual DateTime? PaymentDate { get; protected set; }

        public virtual DateTime? CompletedDate { get; protected set; }

        public virtual DateTime? CancelDate { get; protected set; }

        public virtual string CancelReason { get; protected set; }

        public virtual PaymentType PaymentType { get; protected set; }

        public virtual Shipper Shipper { get; protected set; }

        public virtual double? TaxRate { get; protected set; }

        public virtual Money Tax { get; protected set; }

        public virtual Money ShippingFee { get; protected set; }

        public virtual Money SubTotal { get; protected set; }

        public virtual Money Total { get; protected set; }

        public virtual OrderStatus Status { get; protected set; }

        public virtual bool IsActive { get; protected set; }

        public virtual Employee Employee { get; protected set; }

        public virtual Customer Customer { get; protected set; }

        public virtual IEnumerable<Invoice> Invoices { get; protected set; }

        public virtual IEnumerable<OrderDetail> OrderDetails { get; protected set; }

        public Order() : this(default(Guid)) { }

        public Order(Guid id) : base(id)
        {
            this.Invoices = new Collection<Invoice>();
            this.OrderDetails = new Collection<OrderDetail>();
        }

        public Order(DateTime date, PaymentType paymentType, Shipper shipper, double? taxRate, Money tax, Money shippingFee, Employee employee, Customer customer, Branch branch) : this(default(Guid))
        {
            New(date, paymentType, shipper, taxRate, tax, shippingFee, employee, customer, branch);
        }

        public virtual void New(DateTime date, PaymentType paymentType, Shipper shipper, double? taxRate, Money tax, Money shippingFee, Employee employee, Customer customer, Branch branch)
        {
            this.Status = OrderStatus.New;
            this.IsActive = true;
            this.OrderDate = date;
            this.PaymentType = paymentType;
            this.Shipper = shipper;
            this.TaxRate = taxRate;
            this.Tax = tax;
            this.ShippingFee = shippingFee;
            this.Employee = employee;
            this.Customer = customer;
            this.Branch = branch;

            this.Total = this.Tax + this.ShippingFee;
        }

        public virtual void Invoice(Employee employee, Invoice invoice)
        {
            invoice.Order = this;
            ((Collection<Invoice>)this.Invoices).Add(invoice);
            this.Status = OrderStatus.Invoiced;
            this.IsActive = true;
        }

        public virtual void Ship(DateTime date, Shipper shipper)
        {
            this.Status = OrderStatus.Shipped;
            this.IsActive = true;
            this.Shipper = shipper;
            this.ShippedDate = date;
        }

        public virtual void Completed(DateTime date)
        {
            this.Status = OrderStatus.Completed;
            this.IsActive = false;
            this.CompletedDate = date;
        }

        public virtual void Cancelled(DateTime date, string reason)
        {
            this.Status = OrderStatus.Cancelled;
            this.IsActive = false;
            this.CancelDate = date;
            this.CancelReason = reason;
        }

        public virtual void AddOrderDetail(OrderDetail orderDetail)
        {
            orderDetail.Order = this;
            ((Collection<OrderDetail>)this.OrderDetails).Add(orderDetail);

            this.SubTotal += orderDetail.ExtendedPrice;
            this.Total += orderDetail.ExtendedPrice;
        }
    }
}
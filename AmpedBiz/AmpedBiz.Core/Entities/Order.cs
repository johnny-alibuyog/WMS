using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Services.Orders;

namespace AmpedBiz.Core.Entities
{
    public enum OrderStatus
    {
        New = 1, //active
        Staged = 2,
        Routed = 3,
        Invoiced = 4,
        PartiallyPaid = 5,
        Completed = 6,
        Cancelled = 7
    }

    public class Order : Entity<Guid, Order>
    {
        public virtual Branch Branch { get; set; }

        public virtual DateTime? OrderDate { get; protected set; }

        public virtual DateTime? StagedDate { get; protected set; }

        public virtual DateTime? RoutedDate { get; protected set; }

        public virtual DateTime? InvoicedDate { get; protected set; }

        public virtual DateTime? ShippedDate { get; protected set; }

        public virtual DateTime? PaymentDate { get; protected set; }

        public virtual DateTime? CompletedDate { get; protected set; }

        public virtual DateTime? CancelDate { get; protected set; }

        public virtual string CancelReason { get; protected set; }

        public virtual PaymentType PaymentType { get; protected set; }

        public virtual Shipper Shipper { get; protected set; }

        public virtual decimal? TaxRate { get; protected set; }

        public virtual Money Tax { get; protected set; }

        public virtual Money ShippingFee { get; protected set; }

        public virtual Money SubTotal { get; protected set; }

        public virtual Money Total { get; protected set; }

        public virtual OrderStatus Status { get; protected set; }

        public virtual bool IsActive { get; protected set; }

        public virtual Employee CreatedBy { get; protected set; }

        public virtual Employee StagedBy { get; protected set; }

        public virtual Employee RoutedBy { get; protected set; }

        public virtual Employee InvoicedBy { get; protected set; }

        public virtual Employee PartiallyPaidBy { get; protected set; }

        public virtual Employee CompletedBy { get; protected set; }

        public virtual Employee CancelledBy { get; protected set; }

        public virtual Customer Customer { get; protected set; }

        public virtual IEnumerable<Invoice> Invoices { get; protected set; }

        public virtual IEnumerable<OrderDetail> OrderDetails { get; protected set; }

        public virtual State CurrentState
        {
            get { return State.GetState(this); }
        }

        public Order() : this(default(Guid))
        {
        }

        public Order(Guid id) : base(id)
        {
            this.Invoices = new Collection<Invoice>();
            this.OrderDetails = new Collection<OrderDetail>();
        }

        public Order(PaymentType paymentType, Shipper shipper, decimal? taxRate, Money shippingFee, Employee employee, Customer customer, Branch branch) : this()
        {
            New(paymentType, shipper, taxRate, shippingFee, employee, customer, branch);
        }

        public virtual void New(PaymentType paymentType, Shipper shipper, decimal? taxRate, Money shippingFee, Employee employee, Customer customer, Branch branch)
        {
            this.Status = OrderStatus.New;
            this.IsActive = true;
            this.OrderDate = DateTime.Now;
            this.PaymentType = paymentType;
            this.Shipper = shipper;
            this.TaxRate = taxRate ?? 0.0M;
            this.ShippingFee = shippingFee ?? new Money(0.0M);
            this.CreatedBy = employee;
            this.Customer = customer;
            this.Branch = branch;
        }

        public virtual void Stage(Employee employee)
        {
            this.Status = OrderStatus.Staged;
            this.StagedBy = employee;
            this.StagedDate = DateTime.Now;
        }
        public virtual void Route(Employee employee)
        {
            this.Status = OrderStatus.Routed;
            this.RoutedBy = employee;
            this.RoutedDate = DateTime.Now;

            //allocate product from inventory
        }

        public virtual void Invoice(Employee employee, Invoice invoice)
        {
            invoice.Order = this;
            this.Invoices.Add(invoice);
            this.Status = OrderStatus.Invoiced;
            this.InvoicedBy = employee;
            this.InvoicedDate = DateTime.Now;

            //deduct from inventory
        }

        public virtual void PartiallyPay(Employee employee)
        {
            this.Status = OrderStatus.PartiallyPaid;
            this.PaymentDate = DateTime.Now;
            this.PartiallyPaidBy = employee;

            //no invoice yet?
        }

        public virtual void Complete(Employee employee)
        {
            this.Status = OrderStatus.Completed;
            this.IsActive = false;
            this.CompletedDate = DateTime.Now;
            this.CompletedBy = employee;
        }

        public virtual void Cancel(Employee employee, string reason)
        {
            this.Status = OrderStatus.Cancelled;
            this.IsActive = false;
            this.CancelDate = DateTime.Now;
            this.CancelReason = reason;
            this.CancelledBy = employee;
        }

        public virtual void AddOrderDetail(OrderDetail orderDetail)
        {
            orderDetail.Order = this;
            this.OrderDetails.Add(orderDetail);

            this.SubTotal += orderDetail.ExtendedPrice ?? new Money(0.0M);

            this.Tax = new Money(this.SubTotal.Amount * this.TaxRate.Value);

            this.Total = new Money(this.SubTotal.Amount + this.Tax.Amount + this.ShippingFee.Amount);
        }
    }
}
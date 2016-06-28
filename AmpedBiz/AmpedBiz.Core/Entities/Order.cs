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

        public virtual User CreatedBy { get; protected set; }

        public virtual User StagedBy { get; protected set; }

        public virtual User RoutedBy { get; protected set; }

        public virtual User InvoicedBy { get; protected set; }

        public virtual User PartiallyPaidBy { get; protected set; }

        public virtual User CompletedBy { get; protected set; }

        public virtual User CancelledBy { get; protected set; }

        public virtual Customer Customer { get; protected set; }

        public virtual IEnumerable<Invoice> Invoices { get; protected set; }

        public virtual IEnumerable<OrderDetail> OrderDetails { get; protected set; }

        public virtual State State
        {
            get { return State.GetState(this); }
        }

        public Order() : this(default(Guid)) { }

        public Order(Guid id) : base(id)
        {
            this.Invoices = new Collection<Invoice>();
            this.OrderDetails = new Collection<OrderDetail>();
        }

        public Order(PaymentType paymentType, Shipper shipper, decimal? taxRate, Money shippingFee, User user, Customer customer, Branch branch) : this()
        {
            New(paymentType, shipper, taxRate, shippingFee, user, customer, branch);
        }

        public virtual void New(PaymentType paymentType, Shipper shipper, decimal? taxRate, Money shippingFee, User user, Customer customer, Branch branch)
        {
            this.Status = OrderStatus.New;
            this.IsActive = true;
            this.OrderDate = DateTime.Now;
            this.PaymentType = paymentType;
            this.Shipper = shipper;
            this.TaxRate = taxRate ?? 0.0M;
            this.ShippingFee = shippingFee ?? new Money(0.0M);
            this.CreatedBy = user;
            this.Customer = customer;
            this.Branch = branch;
        }

        public virtual void Stage(User user)
        {
            this.Status = OrderStatus.Staged;
            this.StagedBy = user;
            this.StagedDate = DateTime.Now;
        }
        public virtual void Route(User user)
        {
            this.Status = OrderStatus.Routed;
            this.RoutedBy = user;
            this.RoutedDate = DateTime.Now;

            //allocate product from inventory
        }

        public virtual void Invoice(User user, Invoice invoice)
        {
            invoice.Order = this;
            this.Invoices.Add(invoice);
            this.Status = OrderStatus.Invoiced;
            this.InvoicedBy = user;
            this.InvoicedDate = DateTime.Now;

            //deduct from inventory
        }

        public virtual void PartiallyPay(User user)
        {
            this.Status = OrderStatus.PartiallyPaid;
            this.PaymentDate = DateTime.Now;
            this.PartiallyPaidBy = user;

            //no invoice yet?
        }

        public virtual void Complete(User user)
        {
            this.Status = OrderStatus.Completed;
            this.IsActive = false;
            this.CompletedDate = DateTime.Now;
            this.CompletedBy = user;
        }

        public virtual void Cancel(User user, string reason)
        {
            this.Status = OrderStatus.Cancelled;
            this.IsActive = false;
            this.CancelDate = DateTime.Now;
            this.CancelReason = reason;
            this.CancelledBy = user;
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
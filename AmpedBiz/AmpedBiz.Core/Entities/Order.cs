using System;
using System.Linq;
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

        public virtual IEnumerable<OrderItem> Items { get; protected set; }

        public virtual State State
        {
            get { return State.GetState(this); }
        }

        public Order() : this(default(Guid)) { }

        public Order(Guid id) : base(id)
        {
            this.Invoices = new Collection<Invoice>();
            this.Items = new Collection<OrderItem>();
        }

        public Order(PaymentType paymentType, Shipper shipper, decimal? taxRate, Money shippingFee, User createdBy, Customer customer, Branch branch) : this()
        {
            New(paymentType, shipper, taxRate, shippingFee, createdBy, customer, branch);
        }

        public virtual void New(PaymentType paymentType, Shipper shipper, decimal? taxRate, Money shippingFee, User createdBy,
            Customer customer, Branch branch, IEnumerable<OrderItem> orderItems = null)
        {
            this.Status = OrderStatus.New;
            this.IsActive = true;
            this.OrderDate = DateTime.Now;
            this.PaymentType = paymentType;
            this.Shipper = shipper;
            this.TaxRate = taxRate ?? 0.0M;
            this.ShippingFee = shippingFee ?? new Money(0.0M);
            this.CreatedBy = createdBy;
            this.Customer = customer;
            this.Branch = branch;
            this.SetOrderItems(orderItems);
        }

        protected internal virtual Order SetOrderItems(IEnumerable<OrderItem> items)
        {
            if (items.IsNullOrEmpty())
                return this;

            var itemsToInsert = items.Except(this.Items).ToList();
            var itemsToUpdate = this.Items.Where(x => items.Contains(x)).ToList();
            var itemsToRemove = this.Items.Except(items).ToList();

            foreach (var item in itemsToInsert)
            {
                this.AddOrderItem(item);
            }

            foreach (var item in itemsToUpdate)
            {
                var value = items.Single(x => x == item);
                item.SerializeWith(value);
                item.Order = this;
            }

            foreach (var item in itemsToRemove)
            {
                item.Order = null;
                this.Items.Remove(item);
            }

            return this;
        }

        protected internal virtual Order SetInvoices(IEnumerable<Invoice> invoices)
        {
            if (invoices.IsNullOrEmpty())
                return this;

            var itemsToInsert = invoices.Except(this.Invoices).ToList();
            var itemsToUpdate = this.Invoices.Where(x => invoices.Contains(x)).ToList();
            var itemsToRemove = this.Invoices.Except(invoices).ToList();

            foreach (var item in itemsToInsert)
            {
                this.AddInvoice(item);
            }

            foreach (var item in itemsToUpdate)
            {
                var value = invoices.Single(x => x == item);
                item.SerializeWith(value);
                item.Order = this;
            }

            foreach (var item in itemsToRemove)
            {
                item.Order = null;
                this.Invoices.Remove(item);
            }

            return this;
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

        public virtual void Invoice(User user, IEnumerable<Invoice> invoices = null)
        {
            this.Status = OrderStatus.Invoiced;
            this.InvoicedBy = user;
            this.InvoicedDate = DateTime.Now;
            this.SetInvoices(invoices);

            foreach (var item in this.Items)
            {
                item.Invoice();
            }

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

        public virtual void AddOrderItem(OrderItem orderItem)
        {
            orderItem.Order = this;
            this.Items.Add(orderItem);

            var extendedPriceAmount = (orderItem.ExtendedPrice ?? new Money(0.0M)).Amount;

            this.SubTotal += new Money(extendedPriceAmount);

            this.Tax = new Money(this.SubTotal.Amount * this.TaxRate.Value);

            this.Total = new Money(this.SubTotal.Amount + this.Tax.Amount + this.ShippingFee.Amount);
        }

        public virtual void AddInvoice(Invoice invoice)
        {
            this.Invoices = this.Invoices ?? new List<Invoice>();

            invoice.Order = this;
            this.Invoices.Add(invoice);
        }
    }
}
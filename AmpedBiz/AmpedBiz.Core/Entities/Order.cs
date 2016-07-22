using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Services.Orders;
using AmpedBiz.Core.Events.Orders;

namespace AmpedBiz.Core.Entities
{
    public enum OrderStatus
    {
        New = 1, //active
        Staged = 2,
        Routed = 3,
        Invoiced = 4,
        Paid = 5,
        Completed = 6,
        Cancelled = 7
    }

    public class Order : Entity<Guid, Order>
    {
        public virtual Branch Branch { get; set; }

        public virtual Customer Customer { get; protected set; }

        public virtual PaymentType PaymentType { get; protected set; }

        public virtual Shipper Shipper { get; protected set; }

        public virtual decimal? TaxRate { get; protected set; }

        public virtual Money Tax { get; protected set; }

        public virtual Money ShippingFee { get; protected set; }

        public virtual Money Discount { get; protected set; }

        public virtual Money SubTotal { get; protected set; }

        public virtual Money Total { get; protected set; }

        public virtual OrderStatus Status { get; protected set; }

        public virtual bool IsActive { get; protected set; } // this should be removed

        public virtual DateTime? OrderedOn { get; protected set; }

        public virtual User OrderedBy { get; protected set; }

        public virtual DateTime? CreatedOn { get; protected set; }

        public virtual User CreatedBy { get; protected set; }

        public virtual DateTime? StagedOn { get; protected set; }

        public virtual User StagedBy { get; protected set; }

        public virtual DateTime? ShippedOn { get; protected set; }

        public virtual User ShippedBy { get; protected set; }

        public virtual DateTime? RoutedOn { get; protected set; }

        public virtual User RoutedBy { get; protected set; }

        public virtual DateTime? InvoicedOn { get; protected set; }

        public virtual User InvoicedBy { get; protected set; }

        public virtual DateTime? PaidOn { get; protected set; }

        public virtual User PaidTo { get; protected set; }

        public virtual DateTime? CompletedOn { get; protected set; }

        public virtual User CompletedBy { get; protected set; }

        public virtual DateTime? CancelledOn { get; protected set; }

        public virtual User CancelledBy { get; protected set; }

        public virtual string CancellationReason { get; protected set; }

        public virtual IEnumerable<OrderItem> Items { get; protected set; } = new Collection<OrderItem>();

        public virtual IEnumerable<OrderInvoice> Invoices { get; protected set; } = new Collection<OrderInvoice>();

        public virtual State State
        {
            get { return State.GetState(this); }
        }

        public Order() : base(default(Guid)) { }

        public Order(Guid id) : base(id) { }

        protected internal virtual Order Process(OrderNewlyCreatedEvent @event)
        {
            this.CreatedBy = @event.CreatedBy ?? this.CreatedBy;
            this.CreatedOn = @event.CreatedOn ?? this.CreatedOn;
            this.Branch = @event.Branch ?? this.Branch;
            this.Customer = @event.Customer ?? this.Customer;
            this.Shipper = @event.Shipper ?? this.Shipper;
            this.PaymentType = @event.PaymentType ?? this.PaymentType;
            this.TaxRate = @event.TaxRate ?? this.TaxRate;
            this.Tax = @event.Tax ?? this.Tax;
            this.ShippingFee = @event.ShippingFee ?? this.ShippingFee;
            this.Discount = @event.Discount ?? this.Discount;
            this.SetItems(@event.Items);
            this.Status = OrderStatus.New;

            return this;
        }

        protected internal virtual Order Process(OrderStagedEvent @event)
        {
            this.StagedBy = @event.StagedBy ?? this.StagedBy;
            this.StagedOn = @event.StagedOn ?? this.StagedOn;
            this.Status = OrderStatus.Staged;

            return this;
        }

        protected internal virtual Order Process(OrderRoutedEvent @event)
        {
            this.RoutedBy = @event.RoutedBy ?? this.RoutedBy;
            this.RoutedOn = @event.RoutedOn ?? this.RoutedOn;
            this.Status = OrderStatus.Routed;

            return this;
            //allocate product from inventory
        }

        protected internal virtual Order Process(OrderInvoicedEvent @event)
        {
            this.AddInvoices(@event.Invoices);
            this.Status = OrderStatus.Invoiced;

            return this;
            //deduct from inventory
        }

        protected internal virtual Order Process(OrderPaidEvent @event)
        {
            this.PaidTo = @event.PaidTo;
            this.PaidOn = @event.PaidOn;
            this.PaymentType = @event.PaymentType;
            this.Status = OrderStatus.Paid;

            //no invoice yet?
            return this;
        }

        protected internal virtual Order Process(OrderCompletedEvent @event)
        {
            this.IsActive = false;
            this.CompletedBy = @event.CompletedBy;
            this.CompletedOn = @event.CompletedOn;
            this.Status = OrderStatus.Completed;

            return this;
        }

        protected internal virtual Order Process(OrderCancelledEvent @event)
        {
            this.IsActive = false;
            this.CancelledBy = @event.CancelledBy ?? this.CancelledBy;
            this.CancelledOn = @event.CancelledOn ?? this.CancelledOn;
            this.CancellationReason = @event.CancellationReason ?? this.CancellationReason;
            this.Status = OrderStatus.Cancelled;

            return this;
        }

        private void AddOrderItem(OrderItem orderItem)
        {
            orderItem.Order = this;
            this.Items.Add(orderItem);

            var extendedPriceAmount = (orderItem.ExtendedPrice ?? new Money(0.0M)).Amount;

            this.SubTotal += new Money(extendedPriceAmount);

            this.Tax = new Money(this.SubTotal.Amount * this.TaxRate.Value);

            this.Total = new Money(this.SubTotal.Amount + this.Tax.Amount + this.ShippingFee.Amount);
        }

        private void AddInvoice(OrderInvoice invoice)
        {
            this.Invoices = this.Invoices ?? new List<OrderInvoice>();

            invoice.Order = this;
            this.Invoices.Add(invoice);
        }

        private Order SetItems(IEnumerable<OrderItem> items)
        {
            if (items.IsNullOrEmpty())
                return this;

            var itemsToInsert = items.Except(this.Items).ToList();
            var itemsToUpdate = this.Items.Where(x => items.Contains(x)).ToList();
            var itemsToRemove = this.Items.Except(items).ToList();

            foreach (var item in itemsToInsert)
            {
                item.Order = this;
                this.Items.Add(item);
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

            // calculate here

            return this;
        }

        private Order SetInvoices(IEnumerable<OrderInvoice> invoices)
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

        private Order AddInvoices(IEnumerable<OrderInvoice> invoices)
        {
            var lastInvoice = invoices.OrderBy(x => x.InvoicedOn).LastOrDefault();
            if (lastInvoice == null)
                return this;

            this.InvoicedOn = lastInvoice.InvoicedOn;
            this.InvoicedBy = lastInvoice.InvoicedBy;

            foreach (var invoice in invoices)
            {
                invoice.Order = this;
                this.Invoices.Add(invoice);
            }

            // compute here

            return this;
        }
    }
}
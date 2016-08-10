using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Services.Orders;
using AmpedBiz.Core.Arguments.Orders;

namespace AmpedBiz.Core.Entities
{
    public enum OrderStatus
    {
        New = 1,
        Invoiced = 2,
        Paid = 3,
        Staged = 4,
        Routed = 5,
        Shipped = 6,
        Completed = 7,
        Cancelled = 8
    }

    public class Order : Entity<Guid, Order>
    {
        public virtual string OrderNumber { get; set; }

        public virtual Branch Branch { get; set; }

        public virtual Customer Customer { get; protected set; }

        public virtual PricingScheme PricingScheme { get; protected set; }

        public virtual PaymentType PaymentType { get; protected set; }

        public virtual Shipper Shipper { get; protected set; }

        public virtual Address ShippingAddress { get; protected set; }

        public virtual decimal? TaxRate { get; protected set; }

        public virtual Money Tax { get; protected set; }

        public virtual Money ShippingFee { get; protected set; }

        public virtual Money Discount { get; protected set; }

        public virtual Money SubTotal { get; protected set; }

        public virtual Money Total { get; protected set; }

        public virtual Money Payment { get; protected set; }

        public virtual OrderStatus Status { get; protected set; }

        public virtual DateTime? DueOn { get; set; }

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

        public virtual IEnumerable<OrderPayment> Payments { get; protected set; } = new Collection<OrderPayment>();

        public virtual State State
        {
            get { return State.GetState(this); }
        }

        public Order() : base(default(Guid)) { }

        public Order(Guid id) : base(id) { }

        protected internal virtual Order Process(OrderNewlyCreatedArguments args)
        {
            this.OrderNumber = args.OrderNumber ?? this.OrderNumber;
            this.CreatedBy = args.CreatedBy ?? this.CreatedBy;
            this.CreatedOn = args.CreatedOn ?? this.CreatedOn;
            this.OrderedBy = args.OrderedBy ?? this.OrderedBy;
            this.OrderedOn = args.OrderedOn ?? this.OrderedOn;
            this.Branch = args.Branch ?? this.Branch;
            this.Customer = args.Customer ?? this.Customer;
            this.Shipper = args.Shipper ?? this.Shipper;
            this.PaymentType = args.PaymentType ?? this.PaymentType;
            this.TaxRate = args.TaxRate ?? this.TaxRate;
            this.TaxRate = args.TaxRate ?? this.TaxRate;
            this.Tax = args.Tax ?? this.Tax;
            this.ShippingFee = args.ShippingFee ?? this.ShippingFee;
            this.SetItems(args.Items);
            this.CalculateTotal();
            this.Status = OrderStatus.New;

            return this;
        }

        protected internal virtual Order Process(OrderInvoicedArguments args)
        {
            this.InvoicedOn = args.InvoicedOn ?? this.InvoicedOn;
            this.InvoicedBy = args.InvoicedBy ?? this.InvoicedBy;
            this.Status = OrderStatus.Invoiced;

            return this;
        }

        protected internal virtual Order Process(OrderPaidArguments args)
        {
            this.AddPayments(args.Payments);
            this.Status = OrderStatus.Paid;

            return this;
        }

        protected internal virtual Order Process(OrderStagedArguments args)
        {
            this.StagedBy = args.StagedBy ?? this.StagedBy;
            this.StagedOn = args.StagedOn ?? this.StagedOn;
            this.Status = OrderStatus.Staged;

            return this;
        }

        protected internal virtual Order Process(OrderRoutedArguments args)
        {
            this.RoutedBy = args.RoutedBy ?? this.RoutedBy;
            this.RoutedOn = args.RoutedOn ?? this.RoutedOn;
            this.Status = OrderStatus.Routed;

            return this;
            //allocate product from inventory
        }

        protected internal virtual Order Process(OrderShippedArguments args)
        {
            this.ShippedOn = args.ShippedOn;
            this.ShippedBy = args.ShippedBy;
            this.Status = OrderStatus.Shipped;

            return this;
        }

        protected internal virtual Order Process(OrderCompletedArguments args)
        {
            this.CompletedBy = args.CompletedBy;
            this.CompletedOn = args.CompletedOn;
            this.Status = OrderStatus.Completed;

            return this;
        }

        protected internal virtual Order Process(OrderCancelledArguments args)
        {
            this.CancelledBy = args.CancelledBy ?? this.CancelledBy;
            this.CancelledOn = args.CancelledOn ?? this.CancelledOn;
            this.CancellationReason = args.CancellationReason ?? this.CancellationReason;
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

        private void AddPayment(OrderPayment payment)
        {
            this.Payments = this.Payments ?? new List<OrderPayment>();

            payment.Order = this;
            this.Payments.Add(payment);
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
            
            return this;
        }

        private Order SetPayments(IEnumerable<OrderPayment> payments)
        {
            if (payments.IsNullOrEmpty())
                return this;

            var itemsToInsert = payments.Except(this.Payments).ToList();
            var itemsToUpdate = this.Payments.Where(x => payments.Contains(x)).ToList();
            var itemsToRemove = this.Payments.Except(payments).ToList();

            foreach (var item in itemsToInsert)
            {
                this.AddPayment(item);
            }

            foreach (var item in itemsToUpdate)
            {
                var value = payments.Single(x => x == item);
                item.SerializeWith(value);
                item.Order = this;
            }

            foreach (var item in itemsToRemove)
            {
                item.Order = null;
                this.Payments.Remove(item);
            }

            return this;
        }

        private Order AddPayments(IEnumerable<OrderPayment> payments)
        {
            var lastPayment = payments.OrderBy(x => x.PaidOn).LastOrDefault();
            if (lastPayment == null)
                return this;

            this.InvoicedOn = lastPayment.PaidOn;
            this.InvoicedBy = lastPayment.PaidBy;

            foreach (var payment in payments)
            {
                payment.Order = this;
                this.Payments.Add(payment);
            }

            // compute here

            return this;
        }

        private Order CalculateTotal()
        {
            // calculate here
            if (this.Discount != null)
                this.Discount.Amount = 0M;

            if (this.SubTotal != null)
                this.SubTotal.Amount = 0M;

            if (this.Total != null)
                this.Total.Amount = 0M;

            foreach (var item in this.Items)
            {
                this.Discount += item.Discount;
                this.SubTotal += item.ExtendedPrice;
            }

            //TODO: how to compute for Tax

            this.Total = this.Tax + this.ShippingFee + this.SubTotal - this.Discount;

            return this;
        }
    }
}
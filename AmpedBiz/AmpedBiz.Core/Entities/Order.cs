using AmpedBiz.Core.Services;
using AmpedBiz.Core.Services.Orders;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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

    public class Order : Entity<Guid, Order>, IAccept<OrderVisitor>
    {
        public virtual string OrderNumber { get; internal protected set; }

        public virtual Branch Branch { get; internal protected set; }

        public virtual Customer Customer { get; internal protected set; }

        public virtual PricingScheme PricingScheme { get; internal protected set; }

        public virtual PaymentType PaymentType { get; internal protected set; }

        public virtual Shipper Shipper { get; internal protected set; }

        public virtual Address ShippingAddress { get; internal protected set; }

        public virtual decimal? TaxRate { get; internal protected set; }

        public virtual Money Tax { get; internal protected set; }

        public virtual Money ShippingFee { get; internal protected set; }

        public virtual Money Discount { get; internal protected set; }

        public virtual Money SubTotal { get; internal protected set; }

        public virtual Money Total { get; internal protected set; }

        public virtual Money Payment { get; internal protected set; }

        public virtual OrderStatus Status { get; internal protected set; } = OrderStatus.New;

        public virtual DateTime? DueOn { get; internal protected set; }

        public virtual DateTime? OrderedOn { get; internal protected set; }

        public virtual User OrderedBy { get; internal protected set; }

        public virtual DateTime? CreatedOn { get; internal protected set; }

        public virtual User CreatedBy { get; internal protected set; }

        public virtual DateTime? StagedOn { get; internal protected set; }

        public virtual User StagedBy { get; internal protected set; }

        public virtual DateTime? ShippedOn { get; internal protected set; }

        public virtual User ShippedBy { get; internal protected set; }

        public virtual DateTime? RoutedOn { get; internal protected set; }

        public virtual User RoutedBy { get; internal protected set; }

        public virtual DateTime? InvoicedOn { get; internal protected set; }

        public virtual User InvoicedBy { get; internal protected set; }

        public virtual DateTime? PaidOn { get; internal protected set; }

        public virtual User PaidTo { get; internal protected set; }

        public virtual DateTime? CompletedOn { get; internal protected set; }

        public virtual User CompletedBy { get; internal protected set; }

        public virtual DateTime? CancelledOn { get; internal protected set; }

        public virtual User CancelledBy { get; internal protected set; }

        public virtual string CancellationReason { get; internal protected set; }

        public virtual IEnumerable<OrderItem> Items { get; internal protected set; } = new Collection<OrderItem>();

        public virtual IEnumerable<OrderPayment> Payments { get; internal protected set; } = new Collection<OrderPayment>();

        public virtual StateDispatcher State
        {
            get { return new StateDispatcher(this); }
        }

        public Order() : base(default(Guid)) { }

        public Order(Guid id) : base(id) { }

        public virtual void Accept(OrderVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
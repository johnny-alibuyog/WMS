using AmpedBiz.Core.Entities;
using System;
using System.Collections.Generic;

namespace AmpedBiz.Core.Events.Orders
{
    public class OrderEvent : Event
    {
        public virtual Order Order { get; set; }

        public virtual string TransitionDescription { get; set; }

        public OrderEvent() : base(default(Guid)) { }

        public OrderEvent(Guid id) : base(id) { }
    }

    public class OrderNewlyCreatedEvent: OrderEvent
    {
        public virtual User CreatedBy { get; protected set; }

        public virtual DateTime? CreatedOn { get; protected set; }

        public virtual Branch Branch { get; protected set; }

        public virtual Customer Customer { get; protected set; }

        public virtual Shipper Shipper { get; protected set; }

        public virtual PaymentType PaymentType { get; protected set; }

        public virtual decimal? TaxRate { get; protected set; }

        public virtual Money Tax { get; protected set; }

        public virtual Money ShippingFee { get; protected set; }

        public virtual Money Discount { get; protected set; }

        public virtual IEnumerable<OrderItem> Items { get; protected set; }

        public OrderNewlyCreatedEvent(Guid? id = null, User createdBy = null, DateTime? createdOn = null, Branch branch = null,
            Customer customer = null, Shipper shipper = null, PaymentType paymentType = null, decimal? taxRate = null, Money tax = null,
            Money shippingFee = null, Money discount = null, IEnumerable<OrderItem> items = null) : base(id ?? default(Guid))
        {
            this.CreatedBy = createdBy;
            this.CreatedOn = createdOn;
            this.Branch = branch;
            this.Customer = customer;
            this.Shipper = shipper;
            this.PaymentType = paymentType;
            this.TaxRate = taxRate;
            this.Tax = tax;
            this.ShippingFee = shippingFee;
            this.Discount = discount;
            this.Items = items;
        }
    }

    public class OrderStagedEvent : OrderEvent
    {
        public virtual DateTime? StagedOn { get; protected set; }

        public virtual User StagedBy { get; protected set; }

        public OrderStagedEvent(Guid? id = null, DateTime? stagedOn = null, User stagedBy =null) : base(id ?? default(Guid))
        {
            this.StagedOn = stagedOn;
            this.StagedBy = stagedBy;
        }
    }

    public class OrderRoutedEvent : OrderEvent
    {
        public virtual DateTime? RoutedOn { get; protected set; }

        public virtual User RoutedBy { get; protected set; }

        public OrderRoutedEvent(Guid? id = null, DateTime? routedOn = null, User routedBy = null) : base(id ?? default(Guid))
        {
            this.RoutedOn = routedOn;
            this.RoutedBy = routedBy;
        }
    }

    public class OrderInvoicedEvent : OrderEvent
    {
        public virtual IEnumerable<OrderInvoice> Invoices { get; protected set; }

        public OrderInvoicedEvent(Guid? id = null, IEnumerable<OrderInvoice> invoices = null) : base(id ?? default(Guid))
        {
            this.Invoices = invoices;
        }
    }

    // TODO: Clarify if this will be removed
    public class OrderPaidEvent : OrderEvent
    {
        public virtual DateTime? PaidOn { get; protected set; }

        public virtual User PaidTo { get; protected set; }

        public virtual PaymentType PaymentType { get; protected set; }

        public OrderPaidEvent(Guid? id = null, DateTime? paidOn = null, User paidBy = null, PaymentType paymentType = null) : base(id ?? default(Guid))
        {
            this.PaidOn = paidOn;
            this.PaidTo = paidBy;
            this.PaymentType = paymentType;
        }
    }

    public class OrderCompletedEvent : OrderEvent
    {
        public virtual DateTime? CompletedOn { get; protected set; }

        public virtual User CompletedBy { get; protected set; }

        public OrderCompletedEvent(Guid? id = null, DateTime? completedOn = null, User completedBy = null) : base(id ?? default(Guid))
        {
            this.CompletedOn = completedOn;
            this.CompletedBy = completedBy;
        }
    }

    public class OrderCancelledEvent : OrderEvent
    {
        public virtual DateTime? CancelledOn { get; protected set; }

        public virtual User CancelledBy { get; protected set; }

        public virtual string CancellationReason { get; set; }

        public OrderCancelledEvent(Guid? id = null, DateTime? cancelledOn = null, User cancelledBy = null, string cancellationReason = null) : base(id ?? default(Guid))
        {
            this.CancelledOn = cancelledOn;
            this.CancelledBy = cancelledBy;
            this.CancellationReason = cancellationReason;
        }
    }
}

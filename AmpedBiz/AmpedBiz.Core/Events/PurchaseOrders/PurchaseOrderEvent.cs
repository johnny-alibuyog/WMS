using AmpedBiz.Core.Entities;
using System;
using System.Collections.Generic;

namespace AmpedBiz.Core.Envents.PurchaseOrders
{
    public class PurchaseOrderEvent : Event
    {
        public virtual string TransitionDescription { get; set; }

        public PurchaseOrderEvent() : this(default(Guid)) { }

        public PurchaseOrderEvent(Guid id) : base(id) {  }
    }

    public class PurchaseOrderNewlyCreatedEvent : PurchaseOrderEvent
    {
        public virtual User CreatedBy { get; protected set; }

        public virtual DateTime? CreatedOn { get; protected set; }

        public virtual DateTime? ExpectedOn { get; protected set; }

        public virtual PaymentType PaymentType { get; protected set; }

        public virtual Shipper Shipper { get; protected set; }

        public virtual Money ShippingFee { get; protected set; }

        public virtual Money Tax { get; protected set; }

        public virtual Supplier Supplier { get; protected set; }

        public virtual IEnumerable<PurchaseOrderItem> Items { get; protected set; }

        public PurchaseOrderNewlyCreatedEvent(Guid? id = null, User createdBy = null, DateTime? createdOn = null, 
            DateTime? expectedOn = null, PaymentType paymentType = null, Shipper shipper = null, Money shippingFee = null, 
            Money tax = null, Supplier supplier = null, IEnumerable<PurchaseOrderItem> purchaseOrderItems = null)
        {
            this.CreatedBy = createdBy;
            this.CreatedOn = createdOn;
            this.ExpectedOn = expectedOn;
            this.PaymentType = paymentType;
            this.Tax = tax;
            this.ShippingFee = shippingFee;
            this.Supplier = supplier;
            this.Items = purchaseOrderItems;
        }
    }

    public class PurchaseOrderSubmittedEvent : PurchaseOrderEvent
    {
        public virtual User SubmittedBy { get; protected set; }

        public virtual DateTime? SubmittedOn { get; protected set; }

        public PurchaseOrderSubmittedEvent() : this(default(Guid)) { }

        public PurchaseOrderSubmittedEvent(Guid? id = null, User submittedBy = null, DateTime? submittedOn = null) : base(id ?? default(Guid))
        {
            this.SubmittedBy = submittedBy;
            this.SubmittedOn = submittedOn;
        }
    }

    public class PurchaseOrderApprovedEvent : PurchaseOrderEvent
    {
        public virtual User ApprovedBy { get; protected set; }

        public virtual DateTime? ApprovedOn { get; protected set; }

        public PurchaseOrderApprovedEvent() : this(default(Guid)) { }

        public PurchaseOrderApprovedEvent(Guid? id = null, User approvedBy = null, DateTime? approvedOn = null) : base(id ?? default(Guid))
        {
            this.ApprovedBy = approvedBy;
            this.ApprovedOn = approvedOn;
        }
    }

    public class PurchaseOrderPaidEvent : PurchaseOrderEvent
    {
        public virtual IEnumerable<PurchaseOrderPayment> Payments { get; protected set; }

        public PurchaseOrderPaidEvent() : this(default(Guid)) { }

        public PurchaseOrderPaidEvent(Guid? id = null, IEnumerable<PurchaseOrderPayment> payments = null) : base(id ?? default(Guid))
        {
            this.Payments = payments;
        }
    }

    public class PurchaseOrderReceivedEvent : PurchaseOrderEvent
    {
        public virtual IEnumerable<PurchaseOrderReceipt> Receipts { get; protected set; }

        public PurchaseOrderReceivedEvent() : this(default(Guid)) { }

        public PurchaseOrderReceivedEvent(Guid? id = null, IEnumerable<PurchaseOrderReceipt> receipts = null) : base(id ?? default(Guid))
        {
            this.Receipts = receipts;
        }
    }

    public class PurchaseOrderCompletedEvent : PurchaseOrderEvent
    {
        public virtual User CompletedBy { get; protected set; }

        public virtual DateTime? CompletedOn { get; protected set; }

        public PurchaseOrderCompletedEvent() : this(default(Guid)) { }

        public PurchaseOrderCompletedEvent(Guid? id = null, User completedBy = null, DateTime? completedOn = null) : base(id ?? default(Guid))
        {
            this.CompletedBy = completedBy;
            this.CompletedOn = completedOn;
        }
    }

    public class PurchaseOrderCancelledEvent : PurchaseOrderEvent
    {
        public virtual User CancelledBy { get; protected set; }

        public virtual DateTime? CancelledOn { get; protected set; }

        public virtual string CancellationReason { get; protected set; }

        public PurchaseOrderCancelledEvent() : this(default(Guid)) { }

        public PurchaseOrderCancelledEvent(Guid? id = null, User cancelledBy = null, DateTime? cancelledOn = null, string cancellationReason = null) : base(id ?? default(Guid))
        {
            this.CancelledBy = cancelledBy;
            this.CancelledOn = cancelledOn;
            this.CancellationReason = cancellationReason;
        }
    }
}

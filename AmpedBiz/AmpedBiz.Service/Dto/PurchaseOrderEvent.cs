using AmpedBiz.Common.CustomTypes;
using System;
using System.Collections.Generic;

namespace AmpedBiz.Service.Dto
{
    public class PurchaseOrderEvent
    {
        public Guid Id { get; set; }

        public Guid PurchaseOrderId { get; set; }

        public string TransitionDescription { get; set; }
    }

    public class PurchaseOrderNewlyCreatedEvent : PurchaseOrderEvent
    {
        public Lookup<Guid> CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? ExpectedOn { get; set; }

        public Lookup<string> PaymentType { get; set; }

        public Lookup<string> Shipper { get; set; }

        public Lookup<string> Supplier { get; set; }

        public decimal ShippingFeeAmount { get; set; }

        public decimal TaxAmount { get; set; }

        public IEnumerable<PurchaseOrderItem> Items { get; set; }
    }

    public class PurchaseOrderSubmittedEvent : PurchaseOrderEvent
    {
        public Lookup<Guid> SubmittedBy { get; set; }

        public DateTime? SubmittedOn { get; set; }
    }

    public class PurchaseOrderApprovedEvent : PurchaseOrderEvent
    {
        public Lookup<Guid> ApprovedBy { get; set; }

        public DateTime? ApprovedOn { get; set; }
    }

    public class PurchaseOrderPaidEvent : PurchaseOrderEvent
    {
        public IEnumerable<PurchaseOrderPayment> Payments { get; set; }
    }

    public class PurchaseOrderReceivedEvent : PurchaseOrderEvent
    {
        public IEnumerable<PurchaseOrderReceipt> Receipts { get; set; }
    }

    public class PurchaseOrderCompletedEvent : PurchaseOrderEvent
    {
        public Lookup<Guid> CompletedBy { get; set; }

        public DateTime? CompletedOn { get; set; }
    }

    public class PurchaseOrderCancelledEvent : PurchaseOrderEvent
    {
        public Lookup<Guid> CancelledBy { get; set; }

        public DateTime? CancelledOn { get; set; }

        public string CancellationReason { get; set; }
    }
}

using AmpedBiz.Common.CustomTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AmpedBiz.Service.Dto
{
    public enum PurchaseOrderStatus
    {
        Created = 1,
        Submitted = 2,
        Approved = 3,
        Completed = 4,
        Cancelled = 5
    }

    public enum PurchaseOrderAggregate
    {
        Items = 1,
        Payments = 2,
        Receipts = 3
    }

    public class PurchaseOrder
    {
        public Guid Id { get; set; }

        public string ReferenceNumber { get; set; }

        public string VoucherNumber { get; set; }

        public Guid UserId { get; set; }

        public Lookup<string> PaymentType { get; set; }

        public Lookup<string> Shipper { get; set; }

        public Lookup<Guid> Supplier { get; set; }

        public decimal TaxAmount { get; set; }

        public decimal ShippingFeeAmount { get; set; }

        public decimal SubTotalAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal PaidAmount { get; set; }

        public PurchaseOrderStatus Status { get; set; }

        public DateTime? ExpectedOn { get; set; }

        public Lookup<Guid> CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? RecreatedOn { get; set; }

        public Lookup<Guid> RecreatedBy { get; set; }

        public Lookup<Guid> SubmittedBy { get; set; }

        public DateTime? SubmittedOn { get; set; }

        public Lookup<Guid> ApprovedBy { get; set; }

        public DateTime? ApprovedOn { get; set; }

        public Lookup<Guid> PaidBy { get; set; }

        public DateTime? PaidOn { get; set; }

        public Lookup<Guid> CompletedBy { get; set; }

        public DateTime? CompletedOn { get; set; }

        public Lookup<Guid> CancelledBy { get; set; }

        public DateTime? CancelledOn { get; set; }

        public string CancellationReason { get; set; }

        public IEnumerable<PurchaseOrderItem> Items { get; set; } = new Collection<PurchaseOrderItem>();

        public IEnumerable<PurchaseOrderPayment> Payments { get; set; } = new Collection<PurchaseOrderPayment>();

        public IEnumerable<PurchaseOrderReceipt> Receipts { get; set; } = new Collection<PurchaseOrderReceipt>();

        public IEnumerable<PurchaseOrderReceivable> Receivables { get; set; } = new Collection<PurchaseOrderReceivable>();

        public Dictionary<PurchaseOrderStatus, string> AllowedTransitions { get; set; }

        public StageDefenition<PurchaseOrderStatus, PurchaseOrderAggregate> Stage { get; set; }
    }

    public class PurchaseOrderPageItem
    {
        public Guid Id { get; set; }

        public string ReferenceNumber { get; set; }

        public string VoucherNumber { get; set; }

        public string Supplier { get; set; }

        public string Status { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string SubmittedBy { get; set; }

        public DateTime? SubmittedOn { get; set; }

        public string PaidBy { get; set; }

        public DateTime? PaidOn { get; set; }

        public decimal? TotalAmount { get; set; }
    }
}

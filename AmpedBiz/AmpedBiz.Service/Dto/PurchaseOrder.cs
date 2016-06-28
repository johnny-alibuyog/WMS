using System;
using System.Collections.Generic;

namespace AmpedBiz.Service.Dto
{
    public enum PurchaseOrderStatus
    {
        New = 1,
        Submitted = 2,
        Approved = 3,
        Paid = 4,
        Received = 5,
        Completed = 6,
        Cancelled = 7
    }

    public class PurchaseOrder
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string SupplierId { get; set; }

        public string PaymentTypeId { get; set; }

        public decimal TaxAmount { get; set; }

        public decimal ShippingFeeAmount { get; set; }

        public decimal PaymentAmount { get; set; }

        public decimal SubTotalAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public PurchaseOrderStatus Status { get; set; }

        public DateTime? ExpectedOn { get; set; }

        public UserIdentity CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public UserIdentity ApprovedBy { get; set; }

        public DateTime? ApprovedOn { get; set; }

        public UserIdentity PayedBy { get; set; }

        public DateTime? PaidOn { get; set; }

        public UserIdentity SubmitterBy { get; set; }

        public DateTime? SubmittedOn { get; set; }

        public UserIdentity CompletedBy { get; set; }

        public DateTime? CompletedOn { get; set; }

        public UserIdentity CancelledBy { get; set; }

        public DateTime? CancelledOn { get; set; }

        public string CancelReason { get; set; }

        public IEnumerable<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
    }

    public class PurchaseOrderPageItem
    {
        public Guid Id { get; set; }

        public string Supplier { get; set; }

        public string Status { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string SubmittedBy { get; set; }

        public DateTime? SubmittedOn { get; set; }

        public string PaidBy { get; set; }

        public DateTime? PaidOn { get; set; }

        public string Total { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public DateTime? OrderDate { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? ExpectedDate { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public DateTime? RejectedDate { get; set; }

        public DateTime? PaymentDate { get; set; }

        public DateTime? SubmittedDate { get; set; }

        public DateTime? ClosedDate { get; set; }

        public string PaymentTypeId { get; set; }

        public decimal TaxAmount { get; set; }

        public decimal ShippingFeeAmount { get; set; }

        public decimal PaymentAmount { get; set; }

        public decimal SubTotalAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public PurchaseOrderStatus Status { get; set; }

        public string CreatedByEmployeeId { get; set; }

        public string SubmittedByEmployeeId { get; set; }

        public string ApprovedByEmployeeId { get; set; }

        public string RejectedByEmployeeId { get; set; }

        public string CompletedByEmployeeId { get; set; }

        public string SupplierId { get; set; }

        public string Reason { get; set; }

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

        public string PayedBy { get; set; }

        public DateTime? PayedOn { get; set; }

        public string Total { get; set; }
    }
}

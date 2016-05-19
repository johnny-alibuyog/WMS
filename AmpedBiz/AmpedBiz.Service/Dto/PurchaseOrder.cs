using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Service.Dto
{
    public enum PurchaseOrderStatus
    {
        New = 1, //active
        ForApproval,
        ForCompletion,
        Completed,
        Cancelled
    }

    public class PurchaseOrder
    {
        public Guid Id { get; set; }
        public DateTime? OrderDate { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? ExpectedDate { get; set; }

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

        public string CompletedByEmployeeId { get; set; }

        public string SupplierId { get; set; }
    }

    public class PurchaseOrderPageItem
    {
        public Guid Id { get; set; }
        public string OrderDate { get; set; }

        public string CreationDate { get; set; }

        public string ExpectedDate { get; set; }

        public string PaymentDate { get; set; }

        public string SubmittedDate { get; set; }

        public string ClosedDate { get; set; }

        public string PaymentTypeName { get; set; }

        public string Tax { get; set; }

        public string ShippingFee { get; set; }

        public string Payment { get; set; }

        public string SubTotal { get; set; }

        public string Total { get; set; }

        public string StatusName { get; set; }

        public string CreatedByEmployeeName { get; set; }

        public string SubmittedByEmployeeName { get; set; }

        public string CompletedByEmployeeName { get; set; }

        public string SupplierName { get; set; }
    }
}

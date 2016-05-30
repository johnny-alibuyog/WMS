using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Service.Dto
{
    public enum OrderStatus
    {
        New, //active
        OnStaging,
        OnRoute,
        ForInvoicing,
        Invoiced,
        IncompletePayment,
        Completed,
        Cancelled
    }
    public class Order
    {
        public Guid Id { get; set; }

        public string BranchId { get; set; }

        public DateTime? OrderDate { get; set; }

        public DateTime? ShippedDate { get; set; }

        public DateTime? PaymentDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        public DateTime? CancelDate { get;  set; }

        public string CancelReason { get; set; }

        public string PaymentTypeId { get; set; }

        public string ShipperId { get; set; }

        public decimal? TaxRate { get; set; }

        public decimal TaxAmount { get; set; }

        public decimal ShippingFeeAmount { get; set; }

        public decimal SubTotalAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; }

        public bool IsActive { get; set; }

        public string EmployeeId { get; set; }

        public string CustomerId { get; set; }

        public IEnumerable<Invoice> Invoices { get; set; }

        public IEnumerable<OrderDetail> OrderDetails { get; set; }
    }

    public class OrderPageItem
    {
        //todo:
    }
}

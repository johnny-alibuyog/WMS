using System;
using System.Collections.Generic;

namespace AmpedBiz.Service.Dto
{
    public enum OrderStatus
    {
        New = 1, //active
        Staged = 2,
        Routed = 3,
        Invoiced = 4,
        PartiallyPaid = 5,
        Completed = 6,
        Cancelled = 7
    }

    public class Order
    {
        public Guid Id { get; set; }

        public string BranchId { get; set; }

        public DateTime? OrderDate { get; set; }

        public DateTime? StagedDate { get; set; }

        public DateTime? RoutedDate { get; set; }

        public DateTime? InvoicedDate { get; set; }

        public DateTime? ShippedDate { get; set; }

        public DateTime? PaymentDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        public DateTime? CancelDate { get; set; }

        public string CancelReason { get; set; }

        public string PaymentTypeId { get; set; }

        public Guid ShipperId { get; set; }

        public decimal TaxRate { get; set; }

        public decimal TaxAmount { get; set; }

        public decimal ShippingFeeAmount { get; set; }

        public decimal SubTotalAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; }

        public bool IsActive { get; set; }

        public Guid CreatedById { get; set; }

        public Guid StagedById { get; set; }

        public Guid RoutedById { get; set; }

        public Guid InvoicedById { get; set; }

        public Guid PartiallyPaidById { get; set; }

        public Guid CompletedById { get; set; }

        public Guid CancelledById { get; set; }

        public string CustomerId { get; set; }

        public IEnumerable<Invoice> Invoices { get; set; }

        public IEnumerable<OrderItem> OrderItems { get; set; }

        public Dictionary<Dto.OrderStatus, string> AllowedTransitions { get; set; }
    }

    public class OrderPageItem
    {
        public Guid Id { get; set; }

        public DateTime OrderDate { get; set; }

        public string Tax { get; set; }

        public string ShippingFee { get; set; }

        public string SubTotal { get; set; }

        public string Total { get; set; }

        public string StatusName { get; set; }

        public string CreatedByName { get; set; }

        public string CustomerName { get; set; }
    }
}
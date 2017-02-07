﻿using AmpedBiz.Common.CustomTypes;
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

    public enum PurchaseOrderAggregate
    {
        Items = 1,
        Payments = 2,
        Receipts = 3
    }

    public class PurchaseOrder
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Lookup<string> PaymentType { get; set; }

        public Lookup<string> Shipper { get; set; }

        public Lookup<Guid> Supplier { get; set; }

        public decimal TaxAmount { get; set; }

        public decimal ShippingFeeAmount { get; set; }

        public decimal PaymentAmount { get; set; }

        public decimal SubTotalAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public PurchaseOrderStatus Status { get; set; }

        public DateTime? ExpectedOn { get; set; }

        public Lookup<Guid> CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

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

        public IEnumerable<PurchaseOrderItem> Items { get; set; }

        public IEnumerable<PurchaseOrderPayment> Payments { get; set; }

        public IEnumerable<PurchaseOrderReceipt> Receipts { get; set; }

        public IEnumerable<PurchaseOrderReceivable> Receivables { get; set; }

        public Dictionary<PurchaseOrderStatus, string> AllowedTransitions { get; set; }

        public StageDefenition<PurchaseOrderStatus, PurchaseOrderAggregate> Stage { get; set; }
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

        public decimal? TotalAmount { get; set; }
    }
}

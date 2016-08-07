﻿using AmpedBiz.Common.CustomTypes;
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
        Paid = 5,
        Shipped = 6,
        Completed = 7,
        Cancelled = 8
    }

    public class Order
    {
        public Guid Id { get; set; }

        public string OrderNumber { get; set; }

        public Lookup<string> Branch { get; set; }

        public Lookup<string> Customer { get; set; }

        public Lookup<string> PricingScheme { get; set; }

        public Lookup<string> PaymentType { get; set; }

        public Lookup<string> Shipper { get; set; }

        public Address ShippingAddress { get; set; }

        public decimal? TaxRate { get; set; }

        public decimal TaxAmount { get; set; }

        public decimal ShippingFeeAmount { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal SubTotalAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; }

        public bool IsActive { get; set; } // this should be removed

        public DateTime? OrderedOn { get; set; }

        public Lookup<Guid> OrderedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public Lookup<Guid> CreatedBy { get; set; }

        public DateTime? StagedOn { get; set; }

        public Lookup<Guid> StagedBy { get; set; }

        public DateTime? ShippedOn { get; set; }

        public Lookup<Guid> ShippedBy { get; set; }

        public DateTime? RoutedOn { get; set; }

        public Lookup<Guid> RoutedBy { get; set; }

        public DateTime? InvoicedOn { get; set; }

        public Lookup<Guid> InvoicedBy { get; set; }

        public DateTime? PaidOn { get; set; }

        public Lookup<Guid> PaidTo { get; set; }

        public DateTime? CompletedOn { get; set; }

        public Lookup<Guid> CompletedBy { get; set; }

        public DateTime? CancelledOn { get; set; }

        public Lookup<Guid> CancelledBy { get; set; }

        public string CancellationReason { get; set; }

        public IEnumerable<OrderItem> Items { get; set; }

        public IEnumerable<OrderPayment> Payments { get; set; }

        public Dictionary<OrderStatus, string> AllowedTransitions { get; set; }
    }

    public class OrderPageItem
    {
        public Guid Id { get; set; }

        public string Status { get; set; }

        public string CreatedBy { get; set; }

        public string Customer { get; set; }

        public DateTime? OrderedOn { get; set; }

        public DateTime? PaidOn { get; set; }

        public decimal? TaxAmount { get; set; }

        public decimal? ShippingFeeAmount { get; set; }

        public decimal? SubTotalAmount { get; set; }

        public decimal? TotalAmount { get; set; }
    }
}
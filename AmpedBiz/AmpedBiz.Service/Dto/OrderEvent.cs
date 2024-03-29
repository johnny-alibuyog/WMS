﻿using AmpedBiz.Common.CustomTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Service.Dto
{
    public class OrderEvent
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        public string TransitionDescription { get; set; }
    }

    public class OrderNewlyCreatedEvent : OrderEvent
    {
        public Lookup<Guid> CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? ExpectedOn { get; set; }

        public Lookup<string> Branch { get; set; }

        public Lookup<string> Customer { get; set; }

        public Lookup<string> PricingScheme { get; set; }

        public Lookup<string> Shipper { get; set; }

        public Address ShippingAddress { get; set; }

        public Lookup<string> PaymentType { get; set; }

        public decimal? TaxRate { get; set; }

        public decimal TaxAmount { get; set; }

        public decimal ShippingFeeAmount { get; set; }

        public IEnumerable<OrderItem> Items { get; set; }
    }

    public class OrderStagedEvent : OrderEvent
    {
        public Lookup<Guid> StagedBy { get; set; }

        public DateTime? StagedOn { get; set; }
    }

    public class OrderRoutedEvent : OrderEvent
    {
        public Lookup<Guid> RoutedBy { get; set; }

        public DateTime? RoutedOn { get; set; }
    }

    public class OrderInvoicedEvent : OrderEvent
    {
        public Lookup<Guid> InvoicedBy { get; set; }

        public DateTime? InvoicedOn { get; set; }
    }

    public class OrderPaidEvent : OrderEvent
    {
        public IEnumerable<OrderPayment> Payments { get; set; }
    }

    public class OrderShippedEvent : OrderEvent
    {
        public Lookup<Guid> ShippedBy { get; set; }

        public DateTime? ShippedOn { get; set; }
    }

    public class OrderCompletedEvent : OrderEvent
    {
        public Lookup<Guid> CompletedBy { get; set; }

        public DateTime? CompletedOn { get; set; }
    }

    public class OrderCancelledEvent : OrderEvent
    {
        public Lookup<Guid> CancelledBy { get; set; }

        public DateTime? CancelledOn { get; set; }

        public string CancellationReason { get; set; }
    }
}

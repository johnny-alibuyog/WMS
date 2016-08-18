using AmpedBiz.Core.Entities;
using System;
using System.Collections.Generic;

namespace AmpedBiz.Core.Arguments.Orders
{
    public abstract class OrderArguments
    {
        public virtual string TransitionDescription { get; set; }
    }

    public class OrderNewlyCreatedArguments : OrderArguments
    {
        public virtual string OrderNumber { get; set; }

        public virtual User CreatedBy { get; set; }

        public virtual DateTime? CreatedOn { get; set; }

        public virtual User OrderedBy { get; set; }

        public virtual DateTime? OrderedOn { get; set; }

        public virtual Branch Branch { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual PricingScheme PricingScheme { get; set; }

        public virtual Shipper Shipper { get; set; }

        public virtual Address ShippingAddress { get; set; }

        public virtual PaymentType PaymentType { get; set; }

        public virtual decimal? TaxRate { get; set; }

        public virtual Money Tax { get; set; }

        public virtual Money ShippingFee { get; set; }

        public virtual IEnumerable<OrderItem> Items { get; set; }
    }

    public class OrderStagedArguments : OrderArguments
    {
        public virtual DateTime? StagedOn { get; set; }

        public virtual User StagedBy { get; set; }
    }

    public class OrderRoutedArguments : OrderArguments
    {
        public virtual DateTime? RoutedOn { get; set; }

        public virtual User RoutedBy { get; set; }
    }

    public class OrderInvoicedArguments : OrderArguments
    {
        public virtual DateTime? InvoicedOn { get; set; }

        public virtual User InvoicedBy { get; set; }
    }

    public class OrderPaidArguments : OrderArguments
    {
        public virtual IEnumerable<OrderPayment> Payments { get; set; }
    }

    public class OrderShippedArguments : OrderArguments
    {
        public virtual DateTime? ShippedOn { get; set; }

        public virtual User ShippedBy { get; set; }
    }

    public class OrderCompletedArguments : OrderArguments
    {
        public virtual DateTime? CompletedOn { get; set; }

        public virtual User CompletedBy { get; set; }
    }

    public class OrderCancelledArguments : OrderArguments
    {
        public virtual DateTime? CancelledOn { get; set; }

        public virtual User CancelledBy { get; set; }

        public virtual string CancellationReason { get; set; }
    }
}

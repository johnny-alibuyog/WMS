﻿using AmpedBiz.Core.Common;
using AmpedBiz.Core.Orders.Services;
using AmpedBiz.Core.Products;
using AmpedBiz.Core.SharedKernel;
using AmpedBiz.Core.Users;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AmpedBiz.Core.Orders
{
	public enum OrderStatus
    {
        Created = 1,
        Invoiced = 2,
        Staged = 3,
        Routed = 4,
        Shipped = 5,
        Completed = 6,
        Cancelled = 7
    }

    public enum OrderAggregate
    {
        Items = 1,
        Payments = 2,
        Returns = 3
    }

	public enum OrderType
	{
		Order = 1,
		PointOfSale = 2,
		RoutedSale =3
	}

    public enum OrderTransactionType
    {
        Creation,
        Invoicing,
        Staging,
        Routing,
        Shipping,
        Completion,
        Cancellation,
        ItemModification,
        PaymentCreation,
        ReturnCreation
    }

    public class Order : TransactionBase, IAccept<IVisitor<Order>>
    {
        public virtual string OrderNumber  { get; internal protected set; }

        public virtual string InvoiceNumber { get; internal protected set; }

        public virtual Branch Branch { get; internal protected set; }

        public virtual Customer Customer { get; internal protected set; }

        public virtual Pricing Pricing { get; internal protected set; }

        public virtual PaymentType PaymentType { get; internal protected set; }

        public virtual Shipper Shipper { get; internal protected set; }

        public virtual Address ShippingAddress { get; internal protected set; }

        public virtual decimal? TaxRate { get; internal protected set; }

        public virtual Money Tax { get; internal protected set; }

        public virtual Money ShippingFee { get; internal protected set; }

        public virtual Money Discount { get; internal protected set; }

        public virtual Money Returned { get; internal protected set; }
        
        public virtual Money SubTotal { get; internal protected set; }

        public virtual Money Total { get; internal protected set; }

        public virtual Money Paid { get; internal protected set; }

		public virtual Money Balance { get; internal protected set; }

		public virtual OrderType Type { get; internal protected set; } = OrderType.Order;

        public virtual OrderStatus Status { get; internal protected set; } = OrderStatus.Created;

        public virtual DateTime? DueOn { get; internal protected set; }

        public virtual DateTime? OrderedOn { get; internal protected set; }

        public virtual User OrderedBy { get; internal protected set; }

        public virtual DateTime? RecreatedOn { get; internal protected set; }

        public virtual User RecreatedBy { get; internal protected set; }

        public virtual DateTime? CreatedOn { get; internal protected set; }

        public virtual User CreatedBy { get; internal protected set; }

        public virtual DateTime? StagedOn { get; internal protected set; }

        public virtual User StagedBy { get; internal protected set; }

        public virtual DateTime? ShippedOn { get; internal protected set; }

        public virtual User ShippedBy { get; internal protected set; }

        public virtual DateTime? RoutedOn { get; internal protected set; }

        public virtual User RoutedBy { get; internal protected set; }

        public virtual DateTime? InvoicedOn { get; internal protected set; }

        public virtual User InvoicedBy { get; internal protected set; }

        public virtual DateTime? PaymentOn { get; internal protected set; }

        public virtual User PaymentBy { get; internal protected set; }

        public virtual User ReturnedBy { get; internal protected set; }

        public virtual DateTime? ReturnedOn { get; internal protected set; }

        public virtual DateTime? CompletedOn { get; internal protected set; }

        public virtual User CompletedBy { get; internal protected set; }

        public virtual DateTime? CancelledOn { get; internal protected set; }

        public virtual User CancelledBy { get; internal protected set; }

        public virtual string CancellationReason { get; internal protected set; }

        public virtual IEnumerable<OrderItem> Items { get; internal protected set; } = new Collection<OrderItem>();

        public virtual IEnumerable<OrderReturn> Returns { get; internal protected set; } = new Collection<OrderReturn>();

        public virtual IEnumerable<OrderPayment> Payments { get; internal protected set; } = new Collection<OrderPayment>();

        public virtual IEnumerable<OrderAudit> Transactions { get; internal protected set; } = new Collection<OrderAudit>();

        public virtual StateDispatcher State
        {
            get { return new StateDispatcher(this); }
        }

        public Order() : base(default(Guid)) { }

        public Order(Guid id) : base(id) { }

        public virtual void Accept(IVisitor<Order> visitor)
        {
            visitor.Visit(this);
        }
    }
}
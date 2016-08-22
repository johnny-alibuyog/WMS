﻿using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Services.PurchaseOrders;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AmpedBiz.Core.Services;

namespace AmpedBiz.Core.Entities
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

    public class PurchaseOrder : Entity<Guid, PurchaseOrder>, IAccept<PurchaseOrderVisitor>
    {
        public virtual string PurchaseOrderNumber { get;  internal protected set; }

        public virtual Tenant Tenant { get; internal protected set; }

        public virtual PaymentType PaymentType { get; internal protected set; }

        public virtual Supplier Supplier { get; internal protected set; }

        public virtual Shipper Shipper { get; internal protected set; }

        public virtual Money Tax { get; internal protected set; }

        public virtual Money ShippingFee { get; internal protected set; }

        public virtual Money Payment { get; internal protected set; }

        public virtual Money SubTotal { get; internal protected set; }

        public virtual Money Total { get; internal protected set; }

        public virtual Money Discount { get; internal protected set; }

        public virtual PurchaseOrderStatus Status { get; internal protected set; } = PurchaseOrderStatus.New;

        public virtual DateTime? ExpectedOn { get; internal protected set; }

        public virtual User CreatedBy { get; internal protected set; }

        public virtual DateTime? CreatedOn { get; internal protected set; }

        public virtual User SubmittedBy { get; internal protected set; }

        public virtual DateTime? SubmittedOn { get; internal protected set; }

        public virtual User ApprovedBy { get; internal protected set; }

        public virtual DateTime? ApprovedOn { get; internal protected set; }

        public virtual User PaidBy { get; internal protected set; }

        public virtual DateTime? PaidOn { get; internal protected set; }

        public virtual User ReceivedBy { get; internal protected set; }

        public virtual DateTime? ReceivedOn { get; internal protected set; }

        public virtual User CompletedBy { get; internal protected set; }

        public virtual DateTime? CompletedOn { get; internal protected set; }

        public virtual User CancelledBy { get; internal protected set; }

        public virtual DateTime? CancelledOn { get; internal protected set; }

        public virtual string CancellationReason { get; internal protected set; }

        public virtual IEnumerable<PurchaseOrderItem> Items { get; internal protected set; } = new Collection<PurchaseOrderItem>();

        public virtual IEnumerable<PurchaseOrderPayment> Payments { get; internal protected set; } = new Collection<PurchaseOrderPayment>();

        public virtual IEnumerable<PurchaseOrderReceipt> Receipts { get; internal protected set; } = new Collection<PurchaseOrderReceipt>();

        public virtual StateDispatcher State
        {
            get { return new StateDispatcher(this); }
        }

        public PurchaseOrder() : base(default(Guid)) { }

        public PurchaseOrder(Guid id) : base(id) { }

        public virtual void Accept(PurchaseOrderVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
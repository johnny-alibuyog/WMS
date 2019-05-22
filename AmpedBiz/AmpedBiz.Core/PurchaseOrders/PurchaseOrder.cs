using AmpedBiz.Core.Common;
using AmpedBiz.Core.Products;
using AmpedBiz.Core.PurchaseOrders.Services;
using AmpedBiz.Core.SharedKernel;
using AmpedBiz.Core.Users;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AmpedBiz.Core.PurchaseOrders
{
	public enum PurchaseOrderStatus
    {
        Created = 1,
        Submitted = 2,
        Approved = 3,
        Completed = 4,
        Cancelled = 5
    }

    public enum PurchaseOrderAggregate
    {
        Items = 1,
        Payments = 2,
        Receipts = 3
    }

    public enum PurchaseOrderTransactionType
    {
        Creation,
        Submittion,
        Approval,
        Completion,
        Cancellation,
        ItemModification,
        PaymentCreation,
        ReceivingCreation
    }

    public class PurchaseOrder : TransactionBase, IAccept<IVisitor<PurchaseOrder>>
    {
        public virtual string PurchaseOrderNumber { get; internal protected set; }

        public virtual string ReferenceNumber { get; internal protected set; }

        public virtual string VoucherNumber { get; internal protected set; }

        public virtual Branch Branch { get; internal protected set; }

        public virtual PaymentType PaymentType { get; internal protected set; }

        public virtual Supplier Supplier { get; internal protected set; }

        public virtual Shipper Shipper { get; internal protected set; }

        public virtual Money Tax { get; internal protected set; }

        public virtual Money ShippingFee { get; internal protected set; }

        public virtual Money Discount { get; internal protected set; }

        public virtual Money SubTotal { get; internal protected set; }

        public virtual Money Total { get; internal protected set; }

        public virtual Money Paid { get; internal protected set; }

		public virtual Money Balance { get; internal protected set; }

		public virtual PurchaseOrderStatus Status { get; internal protected set; } = PurchaseOrderStatus.Created;

        public virtual DateTime? ExpectedOn { get; internal protected set; }

        public virtual User CreatedBy { get; internal protected set; }

        public virtual DateTime? CreatedOn { get; internal protected set; }

        public virtual DateTime? RecreatedOn { get; internal protected set; }

        public virtual User RecreatedBy { get; internal protected set; }

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

        public virtual IEnumerable<PurchaseOrderAudit> Transactions { get; internal protected set; } = new Collection<PurchaseOrderAudit>();

        public virtual StateDispatcher State => new StateDispatcher(this);

        public PurchaseOrder() : base(default(Guid)) { }

        public PurchaseOrder(Guid id) : base(id) { }

        public virtual void Accept(IVisitor<PurchaseOrder> visitor)
        {
            visitor.Visit(this);
        }
    }
}
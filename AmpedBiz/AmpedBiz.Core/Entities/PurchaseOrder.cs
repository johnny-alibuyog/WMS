using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Services.PurchaseOrders;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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

    public class PurchaseOrder : Entity<Guid, PurchaseOrder>
    {
        public virtual Tenant Tenant { get; set; }

        public virtual PaymentType PaymentType { get; set; }

        public virtual Supplier Supplier { get; protected set; }

        public virtual Money Tax { get; protected set; }

        public virtual Money ShippingFee { get; protected set; }

        public virtual Money Payment { get; protected set; }

        public virtual Money SubTotal { get; protected set; }

        public virtual Money Total { get; protected set; }

        public virtual PurchaseOrderStatus Status { get; protected set; }

        public virtual DateTime? ExpectedOn { get; protected set; }

        public virtual User CreatedBy { get; protected set; }

        public virtual DateTime? CreatedOn { get; protected set; }

        public virtual User SubmittedBy { get; protected set; }

        public virtual DateTime? SubmittedOn { get; protected set; }

        public virtual User ApprovedBy { get; protected set; }

        public virtual DateTime? ApprovedOn { get; protected set; }

        public virtual User PaidBy { get; protected set; }

        public virtual DateTime? PaidOn { get; protected set; }

        public virtual User CompletedBy { get; protected set; }

        public virtual DateTime? CompletedOn { get; protected set; }

        public virtual User CancelledBy { get; protected set; }

        public virtual DateTime? CancelledOn { get; protected set; }

        public virtual string CancellationReason { get; protected set; }

        public virtual IEnumerable<PurchaseOrderItem> Items { get; protected set; }

        public virtual IEnumerable<PurchaseOrderPayment> Payments { get; protected set; }

        public virtual IEnumerable<PurchaseOrderReceipt> Receipts { get; protected set; }

        public virtual State State
        {
            get { return State.GetState(this); }
        }

        public PurchaseOrder() : this(default(Guid)) { }

        public PurchaseOrder(Guid id) : base(id)
        {
            this.Items = new Collection<PurchaseOrderItem>();
            this.Payments = new Collection<PurchaseOrderPayment>();
            this.Receipts = new Collection<PurchaseOrderReceipt>();
        }

        protected internal virtual PurchaseOrder New(User createdBy, DateTime createdOn, DateTime? expectedOn = null, PaymentType paymentType = null, 
            Shipper shipper = null, Money shippingFee = null, Money tax = null, Supplier supplier = null, IEnumerable<PurchaseOrderItem> purchaseOrderItems = null)
        {
            this.CreatedBy = createdBy;
            this.CreatedOn = createdOn;
            this.ExpectedOn = expectedOn ?? this.ExpectedOn;
            this.PaymentType = paymentType ?? this.PaymentType;
            this.Tax = tax ?? this.Tax;
            this.ShippingFee = shippingFee ?? this.ShippingFee;
            this.Supplier = supplier ?? this.Supplier;
            this.Total = this.Tax + this.ShippingFee + this.Payment;
            this.SetPurchaseOrderItems(purchaseOrderItems);
            this.Status = PurchaseOrderStatus.New;

            return this;
        }

        protected internal virtual PurchaseOrder Submit(User submittedBy, DateTime submittedOn)
        {
            this.SubmittedBy = submittedBy;
            this.SubmittedOn = submittedOn;
            this.Status = PurchaseOrderStatus.Submitted;

            return this;
        }

        protected internal virtual PurchaseOrder Approve(User approvedBy, DateTime approvedOn)
        {
            this.ApprovedBy = approvedBy;
            this.ApprovedOn = approvedOn;
            this.Status = PurchaseOrderStatus.Approved;

            return this;
        }

        protected internal virtual PurchaseOrder Pay(User paidBy, DateTime paidOn, Money payment, PaymentType paymentType)
        {
            this.AddPayment(new PurchaseOrderPayment(
                paidBy: paidBy,
                paidOn: paidOn,
                payment: payment,
                paymentType: paymentType
            ));
            this.Status = PurchaseOrderStatus.Paid;

            return this;
        }

        protected internal virtual PurchaseOrder Recieve(User recieveBy, DateTime recievedOn)
        {
            this.CancelledBy = recieveBy;
            this.CancelledOn = recievedOn;
            this.Status = PurchaseOrderStatus.Received;

            // TODO: Inventory

            return this;
        }

        protected internal virtual PurchaseOrder Complete(User completedBy, DateTime completedOn)
        {
            this.CompletedBy = completedBy;
            this.CompletedOn = completedOn;
            this.Status = PurchaseOrderStatus.Completed;

            return this;
        }

        protected internal virtual PurchaseOrder Cancel(User cancelledBy, DateTime cancelledOn, string cancellationReason)
        {
            this.CancelledBy = cancelledBy;
            this.CancelledOn = cancelledOn;
            this.CancellationReason = cancellationReason;
            this.Status = PurchaseOrderStatus.Cancelled;

            // TODO: Inventory adjustment if necessary

            return this;
        }

        protected virtual void Compute()
        {
            var itemTotal = default(Money);

            foreach (var item in this.Items)
            {
                itemTotal += item.Total;
            }

            this.Total = this.Tax + this.ShippingFee + this.Payment + itemTotal;
        }

        private PurchaseOrder SetPurchaseOrderItems(IEnumerable<PurchaseOrderItem> items)
        {
            if (items.IsNullOrEmpty())
                return this;

            var itemsToInsert = items.Except(this.Items).ToList();
            var itemsToUpdate = this.Items.Where(x => items.Contains(x)).ToList();
            var itemsToRemove = this.Items.Except(items).ToList();

            foreach (var item in itemsToInsert)
            {
                item.PurchaseOrder = this;
                this.Items.Add(item);
            }

            foreach (var item in itemsToUpdate)
            {
                var value = items.Single(x => x == item);
                item.SerializeWith(value);
                item.PurchaseOrder = this;
            }

            foreach (var item in itemsToRemove)
            {
                item.PurchaseOrder = null;
                this.Items.Remove(item);
            }

            return this;
        }

        private PurchaseOrder AddPayment(PurchaseOrderPayment payment)
        {
            this.PaidBy = payment.PaidBy;
            this.PaidOn = payment.PaidOn;
            this.PaymentType = payment.PaymentType;
            this.Payment += payment.Payment;

            payment.PurchaseOrder = this;
            this.Payments.Add(payment);

            return this;
        }

    }
}
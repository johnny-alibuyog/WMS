using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Services.PurchaseOrders;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AmpedBiz.Core.Envents.PurchaseOrders;

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
        public virtual string PurchaseOrderNumber { get; set; }

        public virtual Tenant Tenant { get; set; }

        public virtual PaymentType PaymentType { get; set; }

        public virtual Supplier Supplier { get; protected set; }

        public virtual Shipper Shipper { get; protected set; }

        public virtual Money Tax { get; protected set; }

        public virtual Money ShippingFee { get; protected set; }

        public virtual Money Payment { get; protected set; }

        public virtual Money SubTotal { get; protected set; }

        public virtual Money Total { get; protected set; }

        public virtual Money Discount { get; protected set; }
        
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

        public virtual User ReceivedBy { get; protected set; }

        public virtual DateTime? ReceivedOn { get; protected set; }

        public virtual User CompletedBy { get; protected set; }

        public virtual DateTime? CompletedOn { get; protected set; }

        public virtual User CancelledBy { get; protected set; }

        public virtual DateTime? CancelledOn { get; protected set; }

        public virtual string CancellationReason { get; protected set; }

        public virtual IEnumerable<PurchaseOrderItem> Items { get; protected set; } = new Collection<PurchaseOrderItem>();

        public virtual IEnumerable<PurchaseOrderPayment> Payments { get; protected set; } = new Collection<PurchaseOrderPayment>();

        public virtual IEnumerable<PurchaseOrderReceipt> Receipts { get; protected set; } = new Collection<PurchaseOrderReceipt>();

        //public virtual IEnumerable<PurchaseOrderEvent> Events { get; protected set; }

        public virtual State State
        {
            get { return State.GetState(this); }
        }

        public PurchaseOrder() : base(default(Guid)) { }

        public PurchaseOrder(Guid id) : base(id) { }

        protected internal virtual PurchaseOrder Process(PurchaseOrderNewlyCreatedEvent @event)
        {
            this.PurchaseOrderNumber = @event.PurchaseOrderNumber ?? this.PurchaseOrderNumber;
            this.CreatedBy = @event.CreatedBy ?? this.CreatedBy;
            this.CreatedOn = @event.CreatedOn ?? this.CreatedOn;
            this.ExpectedOn = @event.ExpectedOn ?? this.ExpectedOn;
            this.PaymentType = @event.PaymentType ?? this.PaymentType;
            this.Tax = @event.Tax ?? this.Tax;
            this.ShippingFee = @event.ShippingFee ?? this.ShippingFee;
            this.Shipper = @event.Shipper ?? this.Shipper;
            this.Supplier = @event.Supplier ?? this.Supplier;
            this.Total = this.Tax + this.ShippingFee + this.Payment;
            this.SetItems(@event.Items);
            this.Status = PurchaseOrderStatus.New;

            return this;
        }

        protected internal virtual PurchaseOrder Process(PurchaseOrderSubmittedEvent @event)
        {
            this.SubmittedBy = @event.SubmittedBy;
            this.SubmittedOn = @event.SubmittedOn;
            this.Status = PurchaseOrderStatus.Submitted;

            return this;
        }

        protected internal virtual PurchaseOrder Process(PurchaseOrderApprovedEvent @event)
        {
            this.ApprovedBy = @event.ApprovedBy;
            this.ApprovedOn = @event.ApprovedOn;
            this.Status = PurchaseOrderStatus.Approved;

            return this;
        }

        protected internal virtual PurchaseOrder Process(PurchaseOrderPaidEvent @event)
        {
            this.AddPayments(@event.Payments);
            this.Status = PurchaseOrderStatus.Paid;

            return this;
        }

        protected internal virtual PurchaseOrder Process(PurchaseOrderReceivedEvent @event)
        {
            this.AddReceipts(@event.Receipts);
            this.Status = PurchaseOrderStatus.Received;

            return this;
        }

        protected internal virtual PurchaseOrder Process(PurchaseOrderCompletedEvent @event)
        {
            this.CompletedBy = @event.CompletedBy;
            this.CompletedOn = @event.CompletedOn;
            this.Status = PurchaseOrderStatus.Completed;

            return this;
        }

        protected internal virtual PurchaseOrder Process(PurchaseOrderCancelledEvent @event)
        {
            this.CancelledBy = @event.CancelledBy;
            this.CancelledOn = @event.CancelledOn;
            this.CancellationReason = @event.CancellationReason;
            this.Status = PurchaseOrderStatus.Cancelled;

            // TODO: Inventory adjustment if necessary

            return this;
        }

        protected virtual void Compute()
        {
            var itemTotal = default(Money);

            foreach (var item in this.Items)
            {
                itemTotal += item.ExtendedCost;
            }

            this.Total = this.Tax + this.ShippingFee + this.Payment + itemTotal;
        }

        private PurchaseOrder SetItems(IEnumerable<PurchaseOrderItem> items)
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

        private PurchaseOrder AddPayments(IEnumerable<PurchaseOrderPayment> payments)
        {
            var lastPayment = payments.OrderBy(x => x.PaidOn).LastOrDefault();
            if (lastPayment == null)
                return this;

            this.PaidBy = lastPayment.PaidBy;
            this.PaidOn = lastPayment.PaidOn;
            this.PaymentType = lastPayment.PaymentType;
            this.Payment += lastPayment.Payment;

            foreach (var payment in payments)
            {
                payment.PurchaseOrder = this;
                this.Payments.Add(payment);
            }

            return this;
        }

        private PurchaseOrder AddReceipts(IEnumerable<PurchaseOrderReceipt> receipts)
        {
            var lastReceipt = receipts.OrderBy(x => x.ReceivedOn).LastOrDefault();
            if (lastReceipt == null)
                return this;

            this.ReceivedBy = lastReceipt.ReceivedBy;
            this.ReceivedOn = lastReceipt.ReceivedOn;

            foreach (var receipt in receipts)
            {
                receipt.PurchaseOrder = this;
                this.Receipts.Add(receipt);
            }

            return this;
        }
    }
}
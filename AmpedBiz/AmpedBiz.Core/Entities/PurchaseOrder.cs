using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Services.PurchaseOrders;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AmpedBiz.Core.Entities
{
    public enum PurchaseOrderStatus
    {
        New = 1,
        Submitted = 2,
        Approved = 3,
        Payed = 4,
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

        public virtual Employee CreatedBy { get; protected set; }

        public virtual DateTime? CreatedOn { get; protected set; }

        public virtual Employee SubmittedBy { get; protected set; }

        public virtual DateTime? SubmittedOn { get; protected set; }

        public virtual Employee ApprovedBy { get; protected set; }

        public virtual DateTime? ApprovedOn { get; protected set; }

        public virtual Employee PayedBy { get; protected set; }

        public virtual DateTime? PayedOn { get; protected set; }

        public virtual Employee CompletedBy { get; protected set; }

        public virtual DateTime? CompletedOn { get; protected set; }

        public virtual Employee CancelledBy { get; protected set; }

        public virtual DateTime? CancelledOn { get; protected set; }

        public virtual string CancellationReason { get; protected set; }

        public virtual IEnumerable<PurchaseOrderDetail> PurchaseOrderDetails { get; protected set; }

        public virtual State CurrentState
        {
            get { return State.GetState(this); }
        }

        public PurchaseOrder() : this(default(Guid)) { }

        public PurchaseOrder(Guid id) : base(id)
        {
            this.PurchaseOrderDetails = new Collection<PurchaseOrderDetail>();
        }

        protected internal virtual void New(Employee createdBy, DateTime createdOn, PaymentType paymentType = null, Shipper shipper = null, Money shippingFee = null, Money tax = null, Supplier supplier = null)
        {
            this.CreatedBy = createdBy;
            this.CreatedOn = createdOn;
            this.PaymentType = paymentType ?? this.PaymentType;
            this.Tax = tax ?? this.Tax;
            this.ShippingFee = shippingFee ?? this.ShippingFee;
            this.Supplier = supplier ?? this.Supplier;
            this.Total = this.Tax + this.ShippingFee + this.Payment;
            this.Status = PurchaseOrderStatus.New;
        }

        protected internal virtual void Submit(Employee submittedBy, DateTime submittedOn)
        {
            this.SubmittedBy = submittedBy;
            this.SubmittedOn = submittedOn;
            this.Status = PurchaseOrderStatus.Submitted;
        }

        protected internal virtual void Approve(Employee approvedBy, DateTime approvedOn)
        {
            this.ApprovedBy = approvedBy;
            this.ApprovedOn = approvedOn;
            this.Status = PurchaseOrderStatus.Approved;
        }

        protected internal virtual void Pay(Employee payedBy, DateTime payedOn, Money payment)
        {
            this.PayedBy = payedBy;
            this.PayedOn = payedOn;
            this.Payment += payment;
            this.Status = PurchaseOrderStatus.Payed;
        }

        protected internal virtual void Recieve(Employee recieveBy, DateTime recievedOn)
        {
            this.CancelledBy = recieveBy;
            this.CancelledOn = recievedOn;
            this.Status = PurchaseOrderStatus.Received;

            // TODO: Inventory
        }

        protected internal virtual void Complete(Employee completedBy, DateTime completedOn)
        {
            this.CompletedBy = completedBy;
            this.CompletedOn = completedOn;
            this.Status = PurchaseOrderStatus.Completed;
        }

        protected internal virtual void Cancel(Employee cancelledBy, DateTime cancelledOn, string cancellationReason)
        {
            this.CancelledBy = cancelledBy;
            this.CancelledOn = cancelledOn;
            this.CancellationReason = cancellationReason;
            this.Status = PurchaseOrderStatus.Cancelled;

            // TODO: Inventory adjustment if necessary
        }

        protected virtual void Compute()
        {
            var detailsTotal = default(Money);

            foreach (var detail in this.PurchaseOrderDetails)
            {

                detailsTotal += detail.Total;
            }

            this.Total = this.Tax + this.ShippingFee + this.Payment + detailsTotal;
        }

        public virtual void AddPurchaseOrderDetail(PurchaseOrderDetail orderDetail)
        {
            orderDetail.PurchaseOrder = this;
            this.PurchaseOrderDetails.Add(orderDetail);

            this.SubTotal += orderDetail.Total;
            this.Total += orderDetail.Total;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using System.Linq;

namespace AmpedBiz.Core.Entities
{
    public enum PurchaseOrderStatus
    {
        New = 1, //active
        ForApproval,
        ForCompletion,
        Completed,
        Rejected
    }

    public class PurchaseOrder : Entity<Guid, PurchaseOrder>
    {
        public virtual Tenant Tenant { get; set; }

        public virtual DateTime? OrderDate { get; set; }

        public virtual DateTime? CreationDate { get; set; }

        public virtual DateTime? ExpectedDate { get; set; }

        public virtual DateTime? PaymentDate { get; set; }

        public virtual DateTime? SubmittedDate { get; set; }

        public virtual DateTime? ApprovedDate { get; set; }

        public virtual DateTime? RejectedDate { get; set; }

        public virtual DateTime? ClosedDate { get; set; }

        public virtual PaymentType PaymentType { get; set; }

        public virtual Money Tax { get; set; }

        public virtual Money ShippingFee { get; set; }

        public virtual Money Payment { get; set; }

        public virtual Money SubTotal { get; set; }

        public virtual Money Total { get; set; }

        public virtual PurchaseOrderStatus Status { get; protected set; }

        public virtual Employee CreatedBy { get; set; }

        public virtual Employee SubmittedBy { get; set; }

        public virtual Employee ApprovedBy { get; set; }

        public virtual Employee RejectedBy { get; set; }

        public virtual Employee CompletedBy { get; set; }

        public virtual Supplier Supplier { get; set; }

        public virtual string Reason { get; set; }

        public virtual IEnumerable<PurchaseOrderDetail> PurchaseOrderDetails { get; protected set; }

        public PurchaseOrder() : this(default(Guid)) { }

        public PurchaseOrder(Guid id) : base(id)
        {
            this.PurchaseOrderDetails = new Collection<PurchaseOrderDetail>();
        }

        public virtual void New(Money payment, PaymentType paymentType, Shipper shipper, Money tax, Money shippingFee, Employee employee, Supplier supplier)
        {
            this.Status = PurchaseOrderStatus.New;
            this.OrderDate = DateTime.Now;
            this.CreationDate = DateTime.Now;
            this.PaymentType = paymentType;
            this.Payment = payment;
            this.Tax = tax;
            this.ShippingFee = shippingFee;
            this.CreatedBy = employee;
            this.Supplier = supplier;
            this.Total = this.Tax + this.ShippingFee + this.Payment;
        }

        /// <summary>
        /// For Approval
        /// </summary>
        public virtual void Submit(Employee employee)
        {
            this.Status = PurchaseOrderStatus.ForApproval;
            this.SubmittedBy = employee;
            this.SubmittedDate = DateTime.Now;
        }

        /// <summary>
        /// Not approved
        /// </summary>
        public virtual void Reject(string reason, Employee employee)
        {
            this.Status = PurchaseOrderStatus.Rejected;
            this.RejectedDate = DateTime.Now;
            this.RejectedBy = employee;
        }

        /// <summary>
        /// PO for completion
        /// PO Details is Submited
        /// </summary>
        public virtual void Approve(Employee employee)
        {
            if (this.Status != PurchaseOrderStatus.ForApproval)
                throw new BusinessException("Purchase order is not ready to approve.");

            this.Status = PurchaseOrderStatus.ForCompletion;
            this.ApprovedBy = employee;
            this.ApprovedDate = DateTime.Now;

            this.PurchaseOrderDetails.Any(po =>
            {
                po.Submit();
                return false;
            });

        }

        /// <summary>
        /// PO is completed
        /// PO Details is Posted
        /// </summary>
        /// <param name="date"></param>
        public virtual void Complete(Employee employee)
        {
            if (this.Status != PurchaseOrderStatus.ForCompletion)
                throw new BusinessException("Purchase order is not ready to complete.");

            this.Status = PurchaseOrderStatus.Completed;
            this.CompletedBy = employee;
            this.ClosedDate = DateTime.Now;

            this.PurchaseOrderDetails.Any(po =>
            {
                po.Post();
                return false;
            });
        }

        public virtual void AddPurchaseOrderDetail(PurchaseOrderDetail orderDetail)
        {
            orderDetail.PurchaseOrder = this;
            this.PurchaseOrderDetails.Add(orderDetail);

            this.SubTotal += orderDetail.ExtendedPrice;
            this.Total += orderDetail.ExtendedPrice;
        }
    }
}
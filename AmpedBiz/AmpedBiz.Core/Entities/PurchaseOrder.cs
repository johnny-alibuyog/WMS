using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AmpedBiz.Common.Extentions;

namespace AmpedBiz.Core.Entities
{
    public enum PurchaseOrderStatus
    {
        New = 1, //active
        ForApproval,
        ForCompletion,
        Completed,
        Cancelled
    }

    public class PurchaseOrder : Entity<Guid, PurchaseOrder>
    {
        public virtual Tenant Tenant { get; set; }

        public virtual DateTime? OrderDate { get; set; }

        public virtual DateTime? CreationDate { get; set; }

        public virtual DateTime? ExpectedDate { get; set; }

        public virtual DateTime? PaymentDate { get; set; }

        public virtual DateTime? SubmittedDate { get; set; }

        public virtual DateTime? ClosedDate { get; set; }

        public virtual PaymentType PaymentType { get; set; }

        public virtual Money Tax { get; set; }

        public virtual Money ShippingFee { get; set; }

        public virtual Money Payment { get; set; }

        public virtual Money SubTotal { get; set; }

        public virtual Money Total { get; set; }

        public virtual PurchaseOrderStatus Status { get; set; }

        public virtual Employee CreatedBy { get; set; }

        public virtual Employee SubmittedBy { get; set; }

        public virtual Employee CompletedBy { get; set; }

        public virtual Supplier Supplier { get; set; }

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

        public virtual void Completed(DateTime date)
        {
            this.Status = PurchaseOrderStatus.Completed;
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
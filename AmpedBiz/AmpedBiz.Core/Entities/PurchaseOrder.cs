using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AmpedBiz.Core.Entities
{
    public enum PurchaseOrderStatus
    {
        New,
        Submitted,
        Completed,
        Cancelled
    }

    public class PurchaseOrder : Entity<PurchaseOrder, Guid>
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

        public virtual Money PaymentAmount { get; set; }

        public virtual Money SubTotal { get; set; }

        public virtual Money Total { get; set; }

        public virtual PurchaseOrderStatus Status { get; set; }

        public virtual Employee CreatedBy { get; set; }

        public virtual Employee SubmittedBy { get; set; }

        public virtual Employee ClosedBy { get; set; }

        public virtual Supplier Supplier { get; set; }

        public virtual IEnumerable<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }

        public PurchaseOrder()
        {
            this.PurchaseOrderDetails = new Collection<PurchaseOrderDetail>();
        }
    }
}
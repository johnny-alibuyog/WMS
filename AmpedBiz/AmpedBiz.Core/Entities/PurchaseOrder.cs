using System;
using System.Collections.Generic;

namespace AmpedBiz.Core.Entities
{
    public class PurchaseOrder : Entity<PurchaseOrder, Guid>
    {
        public PurchaseOrder()
        {
            this.PurchaseOrderDetails = new HashSet<PurchaseOrderDetail>();
        }

        public virtual Tenant Tenant { get; set; }

        public virtual DateTimeOffset? OrderDate { get; set; }

        public virtual DateTimeOffset CreationDate { get; set; }

        public virtual DateTime? ExpectedDate { get; set; }

        public virtual Money ShippingFee { get; set; }

        public virtual decimal? Taxes { get; set; }

        public virtual DateTimeOffset? PaymentDate { get; set; }

        public virtual Money PaymentAmount { get; set; }

        public virtual PaymentType PaymentType { get; set; }

        public virtual Money OrderSubTotal { get; set; }

        public virtual Money OrderTotal { get; set; }

        public virtual DateTimeOffset? SubmittedDate { get; set; }

        public virtual DateTimeOffset? ClosedDate { get; set; }

        public virtual bool IsCompleted { get; set; }

        public virtual bool IsSubmitted { get; set; }

        public virtual bool IsNew { get; set; }

        public virtual string StatusText { get; set; }

        public virtual IEnumerable<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }

        public virtual Employee CreatedBy { get; set; }

        public virtual Employee SubmittedBy { get; set; }

        public virtual Employee ClosedBy { get; set; }

        public virtual Supplier Supplier { get; set; }
    }
}
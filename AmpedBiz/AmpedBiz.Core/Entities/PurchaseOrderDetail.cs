using System;

namespace AmpedBiz.Core.Entities
{
    public enum PurchaseOrderDetailStatus
    {
        Posted,
        Submitted
    }

    public class PurchaseOrderDetail : Entity<PurchaseOrderDetail, Guid>
    {
        public virtual PurchaseOrder PurchaseOrder { get; set; }

        public virtual Product Product { get; set; }

        public virtual decimal Quantity { get; set; }

        public virtual Money UnitCost { get; set; }

        public virtual Money ExtendedPrice { get; set; }

        public virtual DateTime? DateReceived { get; private set; }

        public virtual PurchaseOrderDetailStatus Status { get; set; }

        //public virtual bool PostedToInventory { get; private set; }

        //public virtual bool IsSubmitted { get; private set; }
    }
}
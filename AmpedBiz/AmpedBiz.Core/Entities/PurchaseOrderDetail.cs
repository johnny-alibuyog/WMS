using System;

namespace AmpedBiz.Core.Entities
{
    public enum PurchaseOrderDetailStatus
    {
        Posted,
        Submitted
    }

    public class PurchaseOrderDetail : Entity<Guid, PurchaseOrderDetail>
    {
        public virtual PurchaseOrder PurchaseOrder { get; set; }

        public virtual Product Product { get; set; }

        public virtual decimal Quantity { get; set; }

        public virtual Money UnitCost { get; set; }

        public virtual Money ExtendedPrice { get; set; }

        public virtual DateTime? DateReceived { get; private set; }

        public virtual PurchaseOrderDetailStatus Status { get; set; }

        public PurchaseOrderDetail() : this(default(Guid)) { }

        public PurchaseOrderDetail(Guid id) : base(id) { }

        public void Submit()
        {
            this.Status = PurchaseOrderDetailStatus.Submitted;
        }

        public void Post()
        {
            this.Status = PurchaseOrderDetailStatus.Posted;
        }

        //public virtual bool PostedToInventory { get; private set; }

        //public virtual bool IsSubmitted { get; private set; }
    }
}
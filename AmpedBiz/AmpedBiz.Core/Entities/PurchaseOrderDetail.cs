using System;

namespace AmpedBiz.Core.Entities
{
    public enum PurchaseOrderDetailStatus
    {
        Submitted = 1,
        Posted
    }

    public class PurchaseOrderDetail : Entity<Guid, PurchaseOrderDetail>
    {
        public virtual PurchaseOrder PurchaseOrder { get; set; }

        public virtual Product Product { get; set; }

        public virtual decimal Quantity { get; set; }

        public virtual Money UnitCost { get; set; }

        public virtual Money ExtendedPrice { get; set; }

        public virtual DateTime? DateReceived { get; protected set; }

        public virtual PurchaseOrderDetailStatus Status { get; set; }

        public PurchaseOrderDetail() : this(default(Guid)) { }

        public PurchaseOrderDetail(Guid id) : base(id) { }

        public virtual void Submit()
        {
            this.Status = PurchaseOrderDetailStatus.Submitted;
        }

        public virtual void Post()
        {
            this.Status = PurchaseOrderDetailStatus.Posted;
        }

        //public virtual bool PostedToInventory { get; private set; }

        //public virtual bool IsSubmitted { get; private set; }
    }
}
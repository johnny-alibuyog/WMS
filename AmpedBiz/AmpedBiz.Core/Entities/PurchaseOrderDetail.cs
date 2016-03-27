using System;

namespace AmpedBiz.Core.Entities
{
    public class PurchaseOrderDetail : Entity<PurchaseOrderDetail, Guid>
    {
        public PurchaseOrderDetail()
        {
        }

        public virtual Tenant Tenant { get; set; }

        public virtual double Quantity { get; set; }

        public virtual decimal? UnitCost { get; set; }

        public virtual double ExtendedPrice { get; set; }

        public virtual DateTimeOffset? DateReceived { get; set; }

        public virtual bool PostedToInventory { get; set; }

        public virtual bool IsSubmitted { get; set; }

        public virtual PurchaseOrder PurchaseOrder { get; set; }

        public virtual Product Product { get; set; }
    }
}
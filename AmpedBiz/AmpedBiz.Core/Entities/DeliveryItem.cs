using System;

namespace AmpedBiz.Core.Entities
{
    public class DeliveryItem : Entity<Guid, DeliveryItem>
    {
        public virtual Guid DeliveryId { get; set; }

        public virtual Delivery Delivery { get; set; }

        public virtual Product Product { get; set; }

        public virtual Measure Quantity { get; set; }

        public DeliveryItem() : base(default(Guid)) { }

        public DeliveryItem(Guid id) : base(id) { }
    }
}



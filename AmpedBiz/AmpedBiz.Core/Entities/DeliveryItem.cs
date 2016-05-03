using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Core.Entities
{
    public class DeliveryItem : Entity<Guid, DeliveryItem>
    {
        public virtual Guid DeliveryId { get; set; }

        public virtual Delivery Delivery { get; set; }

        public virtual Product Product { get; set; }

        public virtual Measure Quantity { get; set; }

        public DeliveryItem() : this(default(Guid)) { }

        public DeliveryItem(Guid id) : base(id) { }
    }
}



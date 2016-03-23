using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Domain.Entities
{
    public class DeliveryItem : Entity<DeliveryItem, Guid>
    {
        public virtual Guid DeliveryId { get; set; }

        public virtual Delivery Delivery { get; set; }

        public virtual Product Product { get; set; }

        public virtual Quantity Quantity { get; set; }
    }

}



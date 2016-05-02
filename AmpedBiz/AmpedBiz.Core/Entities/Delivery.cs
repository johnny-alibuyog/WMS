using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AmpedBiz.Core.Entities
{
    public class Delivery : Entity<Delivery, Guid>
    {
        public virtual DateTime Date { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Location Location { get; set; }

        public virtual IEnumerable<DeliveryItem> Items { get; protected set; }

        public Delivery()
        {
            this.Items = new Collection<DeliveryItem>();
        }
    }
}

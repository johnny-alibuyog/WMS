using System;

namespace AmpedBiz.Domain.Entities
{
    public class InvoiceItem : Entity<InvoiceItem, Guid>
    {
        public virtual Product Product { get; set; }

        public virtual Quantity Quantity { get; set; }

        public virtual Money Price { get; set; }

        //public virtual Money Tax { get; set; }

        //public virtual Money Discount { get; set; }

        //public virtual Money Total { get; set; }
    }
}

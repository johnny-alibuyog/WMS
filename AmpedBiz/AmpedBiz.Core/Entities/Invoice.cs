using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AmpedBiz.Core.Entities
{
    public class Invoice : Entity<Invoice, Guid>
    {
        public virtual Money Price { get; set; }

        public virtual DateTimeOffset Date { get; set; }

        public virtual IEnumerable<InvoiceItem> Items { get; protected set; }

        public Invoice()
        {
            this.Items = new Collection<InvoiceItem>();
        }
    }
}

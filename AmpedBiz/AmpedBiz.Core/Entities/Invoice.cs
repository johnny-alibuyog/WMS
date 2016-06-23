using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AmpedBiz.Core.Entities
{
    public class Invoice : Entity<Guid, Invoice>
    {
        public virtual Order Order { get; set; }

        public virtual DateTime? InvoiceDate { get; set; }

        public virtual DateTime? DueDate { get; set; }

        public virtual Money Tax { get; set; }

        public virtual Money Shipping { get; set; }

        public virtual Money SubTotal { get; set; }

        public virtual Money Due { get; set; }

        public Invoice() : this(default(Guid)) { }

        public Invoice(Guid id) : base(id) { }
    }
}

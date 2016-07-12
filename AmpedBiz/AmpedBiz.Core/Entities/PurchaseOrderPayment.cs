using System;

namespace AmpedBiz.Core.Entities
{
    public class PurchaseOrderPayment : Entity<Guid, PurchaseOrderPayment>
    {
        public PurchaseOrderPayment() : this(default(Guid)) { }

        public PurchaseOrderPayment(Guid id) : base(id) { }
    }
}

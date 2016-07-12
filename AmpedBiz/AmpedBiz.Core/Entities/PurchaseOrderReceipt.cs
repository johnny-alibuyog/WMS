using System;

namespace AmpedBiz.Core.Entities
{
    public class PurchaseOrderReceipt : Entity<Guid, PurchaseOrderReceipt>
    {
        public PurchaseOrderReceipt() : this(default(Guid)) { }

        public PurchaseOrderReceipt(Guid id) : base(id) { }
    }
}

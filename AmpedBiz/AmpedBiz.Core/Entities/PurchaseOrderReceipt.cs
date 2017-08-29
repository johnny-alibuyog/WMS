using System;

namespace AmpedBiz.Core.Entities
{
    public class PurchaseOrderReceipt : Entity<Guid, PurchaseOrderReceipt>
    {
        public virtual PurchaseOrder PurchaseOrder { get; set; }

        public virtual string BatchNumber { get; protected set; }

        public virtual User ReceivedBy { get; protected set; }

        public virtual DateTime? ReceivedOn { get; protected set; }

        public virtual DateTime? ExpiresOn { get; protected set; }

        public virtual Product Product { get; protected set; }

        public virtual Measure Quantity { get; protected set; }

        public virtual Measure Standard { get; protected set; }

        public virtual Measure QuantityStandardEquivalent { get; protected set; }

        public PurchaseOrderReceipt() : base(default(Guid)) { }

        public PurchaseOrderReceipt(
            string batchNumber, 
            User receivedBy, 
            DateTime? receivedOn, 
            DateTime? expiresOn, 
            Product product,
            Measure quantity,
            Measure standard,
            Guid? id = null
        ) : base(id ?? default(Guid))
        {
            this.Id = id ?? this.Id;
            this.BatchNumber = batchNumber;
            this.ReceivedBy = receivedBy;
            this.ReceivedOn = receivedOn;
            this.ExpiresOn = expiresOn;
            this.Product = product;
            this.Quantity = quantity;
            this.Standard = standard;

            // quantity convertion to standard uom
            this.QuantityStandardEquivalent = standard * quantity;
        }
    }
}

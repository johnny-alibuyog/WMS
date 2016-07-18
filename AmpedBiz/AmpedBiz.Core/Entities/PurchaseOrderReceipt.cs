using System;

namespace AmpedBiz.Core.Entities
{
    public class PurchaseOrderReceipt : Entity<Guid, PurchaseOrderReceipt>
    {
        public virtual PurchaseOrder PurchaseOrder { get; set; }

        public virtual string BatchNumber { get; set; }

        public virtual User ReceivedBy { get; protected set; }

        public virtual DateTime? ReceivedOn { get; protected set; }

        public virtual DateTime? ExpiresOn { get; protected set; }

        public virtual Product Product { get; protected set; }

        public virtual Measure Quantity { get; protected set; }

        public PurchaseOrderReceipt() : this(default(Guid)) { }

        public PurchaseOrderReceipt(Guid id) : base(id) { }

        public PurchaseOrderReceipt(Guid? id = null, string batchNumber = null, User receivedBy = null, DateTime? receivedOn = null, 
            DateTime? expiresOn = null, Product product = null, Measure quantity = null) : this(default(Guid))
        {
            this.Id = id ?? this.Id;
            this.BatchNumber = batchNumber ?? this.BatchNumber;
            this.ReceivedBy = receivedBy ?? this.ReceivedBy;
            this.ReceivedOn = receivedOn ?? this.ReceivedOn;
            this.ExpiresOn = expiresOn ?? this.ExpiresOn;
            this.Product = product ?? this.Product;
            this.Quantity = quantity ?? this.Quantity;
        }
    }
}

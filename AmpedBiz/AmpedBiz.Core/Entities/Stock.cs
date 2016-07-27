using System;

namespace AmpedBiz.Core.Entities
{
    public class Stock : Entity<Guid, Stock>, IAuditable
    {
        public virtual DateTime? CreatedOn { get; set; }

        public virtual User CreatedBy { get; set; }

        public virtual DateTime? ModifiedOn { get; set; }

        public virtual User ModifiedBy { get; set; }

        public virtual Inventory Inventory { get; protected set; }

        public virtual Measure Quantity { get; protected set; }

        public virtual DateTime? ExpiresOn { get; protected set; }

        public virtual bool Bad { get; protected set; }

        public Stock() : base(default(Guid)) { }

        public Stock(Guid id) : base(id) { }

        public Stock(Inventory inventory, Measure quantity, DateTime? expiresOn = null, bool bad = false, Guid? id = null)
            : base(id ?? default(Guid))
        {
            this.Inventory = inventory;
            this.Quantity = quantity;
            this.ExpiresOn = expiresOn;
            this.Bad = bad;
        }
    }

    public class ReceivedStock : Stock
    {

    }

    public class ReleasedStock : Stock
    {

    }

    public class ShrinkedStock : Stock
    {
        public virtual ShrinkageCause Cause { get; protected set; }

        public virtual string Remarks { get; protected set; }

        public ShrinkedStock() : base(default(Guid)) { }

        public ShrinkedStock(Inventory inventory, Measure quantity, ShrinkageCause cause, 
            string remarks, DateTime? expiresOn = null, bool bad = false, Guid? id = null)
            : base(inventory, quantity, expiresOn, bad, id)
        {
            this.Cause = cause;
            this.Remarks = remarks;
        }
    }
}

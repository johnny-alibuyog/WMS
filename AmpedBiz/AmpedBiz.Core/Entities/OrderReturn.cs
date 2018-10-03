using System;

namespace AmpedBiz.Core.Entities
{
    public class OrderReturn : ReturnItemBase
	{
        public virtual Order Order { get; protected internal set; }

		public virtual DateTime? ReturnedOn { get; protected set; }

        public virtual User ReturnedBy { get; protected set; }

        public OrderReturn() : base(default(Guid)) { }

        public OrderReturn(
            Product product, 
            ReturnReason reason, 
            DateTime? returnedOn, 
            User returnedBy, 
            Measure quantity, 
            Measure standard,
            Money returned, 
            Guid? id = null
        ) : base(id ?? default(Guid))
        {
            this.Product = product;
            this.Reason = reason;
            this.ReturnedOn = returnedOn;
            this.ReturnedBy = returnedBy;
            this.Quantity = quantity;
            this.Standard = standard;
            this.Returned = returned;

            // quantity convertion to standard uom
            this.QuantityStandardEquivalent = standard * quantity;
        }
    }
}
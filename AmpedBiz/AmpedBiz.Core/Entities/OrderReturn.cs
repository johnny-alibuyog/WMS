using System;

namespace AmpedBiz.Core.Entities
{
    public class OrderReturn : Entity<Guid, OrderReturn>
    {
        public virtual Product Product { get; protected set; }

        public virtual Order Order { get; protected internal set; }

        public virtual ReturnReason Reason { get; internal protected set; }

        public virtual DateTime? ReturnedOn { get; protected set; }

        public virtual User ReturnedBy { get; protected set; }

        public virtual Measure Quantity { get; protected set; }

        public virtual Money Returned { get; protected set; }

        public OrderReturn() : base(default(Guid)) { }

        public OrderReturn(
            Product product, 
            ReturnReason reason, 
            DateTime? returnedOn, 
            User returnedBy, 
            Measure quantity, 
            Money returned, 
            Guid? id = null
        ) : base(id ?? default(Guid))
        {
            this.Product = product;
            this.Reason = reason;
            this.ReturnedOn = returnedOn;
            this.ReturnedBy = returnedBy;
            this.Quantity = quantity;
            this.Returned = returned;
        }
    }
}
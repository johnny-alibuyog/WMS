using System;
using AmpedBiz.Common.Extentions;

namespace AmpedBiz.Core.Entities
{
    public class OrderReturn : Entity<Guid, OrderReturn>
    {
        public virtual Product Product { get; protected set; }

        public virtual Order Order { get; protected internal set; }

        public virtual DateTime? ReturnedOn { get; protected set; }

        public virtual User ReturnedBy { get; protected set; }

        public virtual Measure Quantity { get; protected set; }

        public virtual Money UnitPrice { get; protected set; }

        public virtual Money TotalPrice { get; protected set; }

        public OrderReturn() : base(default(Guid)) { }

        public OrderReturn(Product product, DateTime? returnedOn, User returnedBy, Measure quantity, Money unitPrice, Guid? id = null)
            : base(id ?? default(Guid))
        {
            this.Product = product;
            this.ReturnedOn = returnedOn;
            this.ReturnedBy = returnedBy;
            this.Quantity = quantity;
            this.UnitPrice = unitPrice;
            this.TotalPrice = new Money(quantity.Value * unitPrice.Amount, unitPrice.Currency);
        }
    }
}
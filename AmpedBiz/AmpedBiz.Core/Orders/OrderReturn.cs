using AmpedBiz.Core.Common;
using AmpedBiz.Core.Products;
using AmpedBiz.Core.Returns;
using AmpedBiz.Core.Users;
using System;

namespace AmpedBiz.Core.Orders
{
    public class OrderReturn : TransactionReturnBase
	{
        public virtual int Sequence { get; protected set; }

        public virtual Order Order { get; protected internal set; }

		public virtual DateTime? ReturnedOn { get; protected set; }

        public virtual User ReturnedBy { get; protected set; }

        public OrderReturn() : base(default(Guid)) { }

        public OrderReturn(
            int sequence,
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
            this.Sequence = sequence;
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
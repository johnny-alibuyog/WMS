using AmpedBiz.Core.Common;
using AmpedBiz.Core.Products;
using System;

namespace AmpedBiz.Core.Returns
{
	public class ReturnItem : ReturnItemBase
	{
        public virtual int Sequence { get; protected set; }

		public virtual Return Return { get; protected internal set; }

		public virtual Money UnitPrice { get; protected set; }

		public ReturnItem() : base(default(Guid)) { }

		public ReturnItem(
            int sequence,
			Product product,
			ReturnReason reason,
			Measure quantity,
			Measure standard,
			Money unitPrice,
			Guid? id = null
		) : base(id ?? default(Guid))
		{
            this.Sequence = sequence;
			this.Product = product;
			this.Reason = reason;
			this.Quantity = quantity;
			this.Standard = standard;
			this.UnitPrice = unitPrice;

			// quantity convertion to standard uom
			this.QuantityStandardEquivalent = standard * quantity;

			this.Returned = new Money((this.Quantity.Value * this.UnitPrice.Amount), this.UnitPrice.Currency);
		}
	}
}

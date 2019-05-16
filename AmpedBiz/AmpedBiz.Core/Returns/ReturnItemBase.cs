using AmpedBiz.Core.Common;
using AmpedBiz.Core.Products;
using AmpedBiz.Core.SharedKernel;
using System;

namespace AmpedBiz.Core.Returns
{
	public abstract class ReturnItemBase : Entity<Guid, ReturnItemBase>
	{
		public virtual Product Product { get; protected set; }

		public virtual ReturnReason Reason { get; internal protected set; }

		public virtual Measure Quantity { get; protected set; }

		public virtual Measure Standard { get; protected set; }

		public virtual Measure QuantityStandardEquivalent { get; protected set; }

		public virtual Money Returned { get; protected set; }

		public ReturnItemBase() : base(default(Guid)) { }

		public ReturnItemBase(Guid id) : base(id) { }
	}
}

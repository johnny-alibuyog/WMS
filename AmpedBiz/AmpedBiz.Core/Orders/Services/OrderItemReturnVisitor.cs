using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.SharedKernel;

namespace AmpedBiz.Core.Orders.Services
{
	public class OrderItemReturnVisitor : IVisitor<OrderItem>
	{
		public OrderReturn Return { get; private set; }

		public OrderItemReturnVisitor(OrderReturn @return)
		{
			this.Return = @return;
		}

		public void Visit(OrderItem target)
		{
			target.Ensure(
				that: (item) => item.Quantity >= this.Return.Quantity, 
				message: $"You can not return {this.Return.Quantity.ToStringWithSymbol()} because only {target.Quantity.ToStringWithSymbol()} is in order."
			);

			target.Quantity -= this.Return.Quantity;
			target.Compute();
		}
	}
}

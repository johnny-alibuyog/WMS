using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Common;
using AmpedBiz.Core.Inventories.Services.Orders;
using AmpedBiz.Core.Products.Services;
using AmpedBiz.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Core.Orders.Services
{
	public class OrderUpdateReturnsVisitor : IVisitor<Order>
    {
        public Branch Branch { get; set; }

        public IEnumerable<OrderReturn> Returns { get; set; }

        public OrderUpdateReturnsVisitor(IEnumerable<OrderReturn> returns, Branch branch)
        {
            this.Returns = returns;
            this.Branch = branch;
        }

        public virtual void Visit(Order target)
        {
            if (this.Returns.IsNullOrEmpty())
                return;

            // allow only insert. edit and delete is not allowed for this aggregate
            var itemsToInsert = this.Returns.Except(target.Returns).ToList();

            foreach (var item in itemsToInsert)
            {
                item.Order = target;
                item.Product.Accept(new SearchAndApplyVisitor()
                {
                    Branch = Branch,
                    InventoryVisitor = new ReturnVisitor()
                    {
                        Reason = item.Reason,
                        QuantityStandardEquivalent = item.QuantityStandardEquivalent,
                    }
                });
                target.Returns.Add(item);
				this.ReflectReturnToOrderItem(target, item);
            }

            var lastReturn = target.Returns.OrderBy(x => x.ReturnedOn).Last();

            if (itemsToInsert.Any())
            {
                target.Accept(new OrderLogTransactionVisitor(
                    transactedBy: lastReturn.ReturnedBy,
                    transactedOn: lastReturn.ReturnedOn ?? DateTime.Now,
                    type: OrderTransactionType.ReturnCreation
                ));
            }
        }

		private void ReflectReturnToOrderItem(Order order, OrderReturn @return)
		{
			var item = order.Items.FirstOrDefault(x => x.Product == @return.Product);
			item.Accept(new OrderItemReturnVisitor(@return));
		}
    }
}

using AmpedBiz.Core.Common;
using AmpedBiz.Core.Inventories.Services.Orders;
using AmpedBiz.Core.Products.Services;
using AmpedBiz.Core.SharedKernel;
using AmpedBiz.Core.Users;
using System;

namespace AmpedBiz.Core.Orders.Services
{
	public class OrderShippedVisitor : IVisitor<Order>
    {
        public Branch Branch { get; set; }

        public DateTime? ShippedOn { get; set; }

        public User ShippedBy { get; set; }

        public void Visit(Order target)
        {
            foreach (var item in target.Items)
            {
                item.Product.Accept(new SearchAndApplyVisitor()
                {
                    Branch = this.Branch,
                    InventoryVisitor = new ShipVisitor()
                    {
                        QuantityStandardEquivalent = item.QuantityStandardEquivalent
                    }
                });
            }

            target.ShippedOn = this.ShippedOn;
            target.ShippedBy = this.ShippedBy;
            target.Status = OrderStatus.Shipped;
            target.Accept(new OrderLogTransactionVisitor(
                transactedBy: this.ShippedBy,
                transactedOn: this.ShippedOn ?? DateTime.Now
            ));
        }
    }
}

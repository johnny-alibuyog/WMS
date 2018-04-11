using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Inventories.Orders;
using AmpedBiz.Core.Services.Products;
using System;

namespace AmpedBiz.Core.Services.Orders
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
        }
    }
}

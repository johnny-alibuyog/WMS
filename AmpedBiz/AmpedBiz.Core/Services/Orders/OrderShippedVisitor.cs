using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Inventories.Orders;
using AmpedBiz.Core.Services.Products;
using System;

namespace AmpedBiz.Core.Services.Orders
{
    public class OrderShippedVisitor : IVisitor<Order>
    {
        public virtual DateTime? ShippedOn { get; set; }

        public virtual User ShippedBy { get; set; }

        public virtual void Visit(Order target)
        {
            foreach (var item in target.Items)
            {
                //item.Product.Inventory.Ship(item.Quantity);
                item.Product.Accept(new SearchAndApplyVisitor()
                {
                    Branch = null,
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

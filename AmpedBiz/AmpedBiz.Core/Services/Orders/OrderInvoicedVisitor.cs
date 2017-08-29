using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Inventories.Orders;
using AmpedBiz.Core.Services.Products;
using System;

namespace AmpedBiz.Core.Services.Orders
{
    public class OrderInvoicedVisitor : IVisitor<Order>
    {
        public virtual DateTime? InvoicedOn { get; set; }

        public virtual User InvoicedBy { get; set; }

        public virtual void Visit(Order target)
        {
            foreach (var item in target.Items)
            {
                //item.Product.Inventory.Allocate(item.Quantity);
                item.Product.Accept(new SearchAndApplyVisitor()
                {
                    Branch = null,
                    InventoryVisitor = new AllocateVisitor()
                    {
                        QuantityStandardEquivalent = item.QuantityStandardEquivalent
                    }
                });

            }

            target.InvoicedOn = this.InvoicedOn;
            target.InvoicedBy = this.InvoicedBy;
            target.Status = OrderStatus.Invoiced;
        }
    }
}

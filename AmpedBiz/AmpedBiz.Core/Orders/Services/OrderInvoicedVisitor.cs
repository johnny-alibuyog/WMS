using AmpedBiz.Core.Common;
using AmpedBiz.Core.Inventories.Services.Orders;
using AmpedBiz.Core.Products.Services;
using AmpedBiz.Core.Users;
using System;

namespace AmpedBiz.Core.Orders.Services
{
	public class OrderInvoicedVisitor : IVisitor<Order>
    {
        public Branch Branch { get; set; }

        public DateTime? InvoicedOn { get; set; }

        public User InvoicedBy { get; set; }

        public void Visit(Order target)
        {
            foreach (var item in target.Items)
            {
                item.Product.Accept(new SearchAndApplyVisitor()
                {
                    Branch = Branch,
                    InventoryVisitor = new AllocateVisitor()
                    {
                        QuantityStandardEquivalent = item.QuantityStandardEquivalent
                    }
                });
            }

            target.InvoicedOn = this.InvoicedOn;
            target.InvoicedBy = this.InvoicedBy;
            target.Status = OrderStatus.Invoiced;
            target.Accept(new OrderLogTransactionVisitor(
                transactedBy: this.InvoicedBy,
                transactedOn: this.InvoicedOn ?? DateTime.Now
            ));
        }
    }
}

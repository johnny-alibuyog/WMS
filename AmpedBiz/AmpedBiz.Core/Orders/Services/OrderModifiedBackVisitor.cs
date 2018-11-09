using AmpedBiz.Core.Common;
using AmpedBiz.Core.Inventories.Services.Orders;
using AmpedBiz.Core.Products.Services;
using AmpedBiz.Core.Users;
using System;

namespace AmpedBiz.Core.Orders.Services
{
	public class OrderModifiedBackVisitor : IVisitor<Order>
    {
        public Branch Branch { get; set; }

        public DateTime? RecreatedOn { get; set; }

        public User RecreatedBy { get; set; }

        public void Visit(Order target)
        {
            switch (target.Status)
            {
                case OrderStatus.Invoiced:
                case OrderStatus.Staged:
                //case OrderStatus.Routed: // TODO: handle routing soon
                    foreach (var item in target.Items)
                    {
                        item.Product.Accept(new SearchAndApplyVisitor()
                        {
                            Branch = Branch,
                            InventoryVisitor = new RetractAllocatedVisitor()
                            {
                                QuantityStandardEquivalent = item.QuantityStandardEquivalent
                            }
                        });
                    }
                    break;

                default:
                    throw new Exception("You cannot recreate an order that has passed staging.");
            }

            target.RecreatedBy = this.RecreatedBy ?? target.RecreatedBy;
            target.RecreatedOn = this.RecreatedOn ?? target.RecreatedOn;
            target.Status = OrderStatus.Created;
            target.Accept(new OrderLogTransactionVisitor(
                transactedBy: this.RecreatedBy,
                transactedOn: this.RecreatedOn ?? DateTime.Now
            ));
        }
    }
}

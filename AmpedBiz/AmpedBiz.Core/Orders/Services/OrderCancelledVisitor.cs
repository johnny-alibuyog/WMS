using AmpedBiz.Core.Common;
using AmpedBiz.Core.Inventories.Services.Orders;
using AmpedBiz.Core.Products.Services;
using AmpedBiz.Core.SharedKernel;
using AmpedBiz.Core.Users;
using System;

namespace AmpedBiz.Core.Orders.Services
{
	public class OrderCancelledVisitor : IVisitor<Order>
    {
        public Branch Branch { get; set; }

        public DateTime? CancelledOn { get; set; }

        public User CancelledBy { get; set; }

        public string CancellationReason { get; set; }
                       
        public void Visit(Order target)
        {
            switch (target.Status)
            {
                case OrderStatus.Invoiced:
                case OrderStatus.Staged:
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

                case OrderStatus.Shipped:
                    foreach (var item in target.Items)
                    {
                        item.Product.Accept(new SearchAndApplyVisitor()
                        {
                            Branch = Branch,
                            InventoryVisitor = new RetractShippedVisitor()
                            {
                                QuantityStandardEquivalent = item.QuantityStandardEquivalent
                            }
                        });
                    }
                    break;
            }

            target.CancelledBy = this.CancelledBy ?? target.CancelledBy;
            target.CancelledOn = this.CancelledOn ?? target.CancelledOn;
            target.CancellationReason = this.CancellationReason ?? target.CancellationReason;
            target.Status = OrderStatus.Cancelled;
            target.Accept(new OrderLogTransactionVisitor(
                transactedBy: this.CancelledBy,
                transactedOn: this.CancelledOn ?? DateTime.Now
            ));
        }
    }
}

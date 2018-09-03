using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Inventories.Orders;
using AmpedBiz.Core.Services.Products;
using System;

namespace AmpedBiz.Core.Services.Orders
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

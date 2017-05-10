using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Inventories.Orders;
using AmpedBiz.Core.Services.Products;
using System;

namespace AmpedBiz.Core.Services.Orders
{
    public class OrderCancelledVisitor : OrderVisitor
    {
        public virtual DateTime? CancelledOn { get; set; }

        public virtual User CancelledBy { get; set; }

        public virtual string CancellationReason { get; set; }

        public override void Visit(Order target)
        {
            switch (target.Status)
            {
                case OrderStatus.Invoiced:
                case OrderStatus.Staged:
                    foreach (var item in target.Items)
                    {
                        item.Product.Accept(new SearchAndApplyVisitor()
                        {
                            Branch = null,
                            InventoryVisitor = new RetractAllocatedVisitor()
                            {
                                Quantity = item.Quantity
                            }
                        });

                        //item.Product.Inventory.Accept(new DeallocateVisitor()
                        //{
                        //    Quantity = item.Quantity
                        //});
                    }
                    break;

                case OrderStatus.Shipped:
                    // NOTE: when order has been shipped and was cancelled, 
                    // items should be go through returns
                    break;
            }

            target.CancelledBy = this.CancelledBy ?? target.CancelledBy;
            target.CancelledOn = this.CancelledOn ?? target.CancelledOn;
            target.CancellationReason = this.CancellationReason ?? target.CancellationReason;
            target.Status = OrderStatus.Cancelled;
        }
    }
}

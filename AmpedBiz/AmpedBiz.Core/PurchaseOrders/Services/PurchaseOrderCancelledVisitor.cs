﻿using AmpedBiz.Core.Common;
using AmpedBiz.Core.Inventories.Services.PurchaseOrders;
using AmpedBiz.Core.Products.Services;
using AmpedBiz.Core.SharedKernel;
using AmpedBiz.Core.Users;
using System;

namespace AmpedBiz.Core.PurchaseOrders.Services
{
	public class PurchaseOrderCancelledVisitor : IVisitor<PurchaseOrder>
    {
        public Branch Branch { get; set; }

        public User CancelledBy { get; set; }

        public DateTime? CancelledOn { get; set; }

        public string CancellationReason { get; set; }

        public void Visit(PurchaseOrder target)
        {
            var calculator = new PurchaseOrderCalculator();

            foreach (var item in target.Items)
            {
                item.Product.Accept(new SearchAndApplyVisitor()
                {
                    Branch = this.Branch,
                    InventoryVisitor = new RetractOrderedVisitor()
                    {
                        Status = target.Status,
                        QuantityStandardEquivalent = calculator.Remaining(
                            product: item.Product,
                            items: target.Items,
                            receipts: target.Receipts
                        )
                    }
                });
            }

            target.CancelledBy = this.CancelledBy;
            target.CancelledOn = this.CancelledOn;
            target.CancellationReason = this.CancellationReason;
            target.Status = PurchaseOrderStatus.Cancelled;
            target.Accept(new PurchaseOrderLogTransactionVisitor(
                transactedBy: this.CancelledBy,
                transactedOn: this.CancelledOn ?? DateTime.Now
            ));
        }
    }
}

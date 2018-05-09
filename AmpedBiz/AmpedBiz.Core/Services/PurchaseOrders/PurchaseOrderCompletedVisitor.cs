﻿using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Inventories.PurchaseOrders;
using AmpedBiz.Core.Services.Products;
using System;

namespace AmpedBiz.Core.Services.PurchaseOrders
{
    public class PurchaseOrderCompletedVisitor : IVisitor<PurchaseOrder>
    {
        public Branch Branch { get; set; }

        public User CompletedBy { get; set; }

        public DateTime? CompletedOn { get; set; }

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
                        Remaining = calculator.Remaining(
                            product: item.Product,
                            items: target.Items,
                            receipts: target.Receipts
                        )
                    }
                });
            }

            target.CompletedBy = this.CompletedBy;
            target.CompletedOn = this.CompletedOn;
            target.Status = PurchaseOrderStatus.Completed;
            target.Accept(new PurchaseOrderLogTransactionVisitor(
                transactedBy: this.CompletedBy,
                transactedOn: this.CompletedOn ?? DateTime.Now
            ));
        }
    }
}

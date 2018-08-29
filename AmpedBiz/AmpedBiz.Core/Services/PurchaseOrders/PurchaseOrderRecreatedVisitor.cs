﻿using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Inventories.PurchaseOrders;
using AmpedBiz.Core.Services.Products;
using System;

namespace AmpedBiz.Core.Services.PurchaseOrders
{
    public class PurchaseOrderRecreatedVisitor : IVisitor<PurchaseOrder>
    {
        public Branch Branch { get; set; }

        public User RecreatedBy { get; set; }

        public DateTime? RecreatedOn { get; set; }

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

            target.RecreatedBy = this.RecreatedBy;
            target.RecreatedOn = this.RecreatedOn;
            target.Status = PurchaseOrderStatus.Created;
            target.Accept(new PurchaseOrderLogTransactionVisitor(
                transactedBy: this.RecreatedBy,
                transactedOn: this.RecreatedOn ?? DateTime.Now
            ));
        }
    }
}

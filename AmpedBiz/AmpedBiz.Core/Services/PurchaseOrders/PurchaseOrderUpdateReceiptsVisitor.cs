﻿using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Inventories.PurchaseOrders;
using AmpedBiz.Core.Services.Products;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Core.Services.PurchaseOrders
{
    public class PurchaseOrderUpdateReceiptsVisitor : IVisitor<PurchaseOrder>
    {
        public Branch Branch { get; set; }

        public IEnumerable<PurchaseOrderReceipt> Receipts { get; set; }

        public PurchaseOrderUpdateReceiptsVisitor(IEnumerable<PurchaseOrderReceipt> receipts, Branch branch)
        {
            this.Receipts = receipts;
            this.Branch = branch;
        }

        public virtual void Visit(PurchaseOrder target)
        {
            if (this.Receipts.IsNullOrEmpty())
                return;

            var itemsToInsert = this.Receipts.Except(target.Receipts).ToList();

            foreach (var item in itemsToInsert)
            {
                item.PurchaseOrder = target;
                item.Product.Accept(new SearchAndApplyVisitor()
                {
                    Branch = this.Branch,
                    InventoryVisitor = new ReceiveVisitor()
                    {
                        QuantityStandardEquivalent = item.QuantityStandardEquivalent
                    }
                });
                target.Receipts.Add(item);
            }

            var lastReceipt = target.Receipts.OrderBy(x => x.ReceivedOn).Last();

            target.ReceivedBy = lastReceipt.ReceivedBy;
            target.ReceivedOn = lastReceipt.ReceivedOn;
        }
    }
}

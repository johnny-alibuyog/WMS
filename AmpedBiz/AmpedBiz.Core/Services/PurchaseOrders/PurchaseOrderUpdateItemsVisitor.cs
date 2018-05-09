using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Core.Services.PurchaseOrders
{
    public class PurchaseOrderUpdateItemsVisitor : IVisitor<PurchaseOrder>
    {
        public virtual IEnumerable<PurchaseOrderItem> Items { get; set; }

        public PurchaseOrderUpdateItemsVisitor(IEnumerable<PurchaseOrderItem> items)
        {
            this.Items = items;
        }

        public virtual void Visit(PurchaseOrder target)
        {
            if (this.Items.IsNullOrEmpty())
                return;

            var itemsToInsert = this.Items.Except(target.Items).ToList();
            var itemsToUpdate = target.Items.Where(x => this.Items.Contains(x)).ToList();
            var itemsToRemove = target.Items.Except(this.Items).ToList();

            foreach (var item in itemsToInsert)
            {
                item.PurchaseOrder = target;
                target.Items.Add(item);
            }

            foreach (var item in itemsToUpdate)
            {
                var value = this.Items.Single(x => x == item);
                item.SerializeWith(value);
                item.PurchaseOrder = target;
            }

            foreach (var item in itemsToRemove)
            {
                item.PurchaseOrder = null;
                target.Items.Remove(item);
            }

            if (itemsToInsert.Any() ||
                itemsToUpdate.Any() ||
                itemsToRemove.Any())
            {
                target.Accept(new PurchaseOrderLogTransactionVisitor(
                    transactedBy: target.CreatedBy,
                    transactedOn: target.CreatedOn ?? DateTime.Now,
                    type: PurchaseOrderTransactionType.ItemModification
                ));
            }
        }
    }
}

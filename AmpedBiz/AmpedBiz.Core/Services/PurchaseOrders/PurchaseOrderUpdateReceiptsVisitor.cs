using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Core.Services.PurchaseOrders
{
    public class PurchaseOrderUpdateReceiptsVisitor : PurchaseOrderVisitor
    {
        public virtual IEnumerable<PurchaseOrderReceipt> Receipts { get; set; }

        public PurchaseOrderUpdateReceiptsVisitor(IEnumerable<PurchaseOrderReceipt> receipts)
        {
            Receipts = receipts;
        }

        public override void Visit(PurchaseOrder target)
        {
            if (this.Receipts.IsNullOrEmpty())
                return;

            var itemsToInsert = this.Receipts.Except(target.Receipts).ToList();
            //var itemsToUpdate = target.Receipts.Where(x => this.Receipts.Contains(x)).ToList();
            //var itemsToRemove = target.Receipts.Except(this.Receipts).ToList();

            foreach (var item in itemsToInsert)
            {
                item.PurchaseOrder = target;
                item.Received();
                target.Receipts.Add(item);
            }

            //foreach (var item in itemsToUpdate)
            //{
            //    var value = this.Receipts.Single(x => x == item);
            //    item.SerializeWith(value);
            //    item.Order = target;
            //}

            //foreach (var item in itemsToRemove)
            //{
            //    item.PurchaseOrder = null;
            //    target.Receipts.Remove(item);
            //}

            var lastReceipt = target.Receipts.OrderBy(x => x.ReceivedOn).Last();

            target.ReceivedBy = lastReceipt.ReceivedBy;
            target.ReceivedOn = lastReceipt.ReceivedOn;

            //this.AddReceiptsTo(target);
            //target.Status = PurchaseOrderStatus.Received;
        }

        private void AddReceiptsTo(PurchaseOrder target)
        {
            var lastReceipt = this.Receipts.OrderBy(x => x.ReceivedOn).LastOrDefault();
            if (lastReceipt == null)
                return;

            target.ReceivedBy = lastReceipt.ReceivedBy;
            target.ReceivedOn = lastReceipt.ReceivedOn;

            foreach (var receipt in this.Receipts)
            {
                receipt.PurchaseOrder = target;
                receipt.Received();
                target.Receipts.Add(receipt);
            }
        }
    }
}

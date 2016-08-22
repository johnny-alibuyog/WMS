using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Core.Services.PurchaseOrders
{
    public class PurchaseOrderReceivedVisitor : PurchaseOrderVisitor
    {
        public virtual IEnumerable<PurchaseOrderReceipt> Receipts { get; set; }

        public override void Visit(PurchaseOrder target)
        {
            this.AddReceiptsTo(target);
            target.Status = PurchaseOrderStatus.Received;
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

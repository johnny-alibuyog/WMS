using AmpedBiz.Core.Common;
using AmpedBiz.Core.Inventories.Services.PurchaseOrders;
using AmpedBiz.Core.Products.Services;
using AmpedBiz.Core.SharedKernel;
using AmpedBiz.Core.Users;
using System;

namespace AmpedBiz.Core.PurchaseOrders.Services
{
	public class PurchaseOrderApprovedVisitor : IVisitor<PurchaseOrder>
    {
        public Branch Branch { get; set; }

        public User ApprovedBy { get; set; }

        public DateTime? ApprovedOn { get; set; }

        public void Visit(PurchaseOrder target)
        {
            foreach (var item in target.Items)
            {
                item.Product.Accept(new SearchAndApplyVisitor()
                {
                    Branch = this.Branch,
                    InventoryVisitor = new OrderVisitor()
                    {
                        QuantityStandardEquivalent = item.QuantityStandardEquivalent
                    }
                });
            }

            target.ApprovedBy = this.ApprovedBy;
            target.ApprovedOn = this.ApprovedOn;
            target.Status = PurchaseOrderStatus.Approved;
            target.Accept(new PurchaseOrderLogTransactionVisitor(
                transactedBy: this.ApprovedBy,
                transactedOn: this.ApprovedOn ?? DateTime.Now
            ));
        }
    }
}

using AmpedBiz.Core.Users;
using System;

namespace AmpedBiz.Core.PurchaseOrders.Services
{
	public class PurchaseOrderSubmittedVisitor : IVisitor<PurchaseOrder>
    {
        public virtual User SubmittedBy { get; set; }

        public virtual DateTime? SubmittedOn { get; set; }

        public virtual void Visit(PurchaseOrder target)
        {
            target.SubmittedBy = this.SubmittedBy;
            target.SubmittedOn = this.SubmittedOn;
            target.Status = PurchaseOrderStatus.Submitted;
            target.Accept(new PurchaseOrderLogTransactionVisitor(
                transactedBy: this.SubmittedBy,
                transactedOn: this.SubmittedOn ?? DateTime.Now
            ));
        }
    }
}

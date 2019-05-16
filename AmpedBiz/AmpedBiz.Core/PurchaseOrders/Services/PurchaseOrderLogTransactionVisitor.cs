using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.SharedKernel;
using AmpedBiz.Core.Users;
using System;

namespace AmpedBiz.Core.PurchaseOrders.Services
{
	public class PurchaseOrderLogTransactionVisitor : IVisitor<PurchaseOrder>
    {
        public virtual User TransactedBy { get; private set; }

        public virtual DateTime TransactedOn { get; private set; }

        public virtual PurchaseOrderTransactionType? Type { get; private set; }

        public PurchaseOrderLogTransactionVisitor(User transactedBy, DateTime transactedOn, PurchaseOrderTransactionType? type = null)
        {
            this.TransactedBy = transactedBy;
            this.TransactedOn = transactedOn;
            this.Type = type;
        }

        public void Visit(PurchaseOrder target)
        {
            target.Transactions.Add(new PurchaseOrderTransaction(
                purchaseOrder: target,
                transactedBy: this.TransactedBy,
                transactedOn: this.TransactedOn,
                status: target.Status,
                type: this.Type ?? this.MapTypeBy(target.Status)
            ));
        }

        private PurchaseOrderTransactionType MapTypeBy(PurchaseOrderStatus status)
        {
            switch (status)
            {
                case PurchaseOrderStatus.Created:
                    return PurchaseOrderTransactionType.Creation;

                case PurchaseOrderStatus.Submitted:
                    return PurchaseOrderTransactionType.Submittion;

                case PurchaseOrderStatus.Approved:
                    return PurchaseOrderTransactionType.Approval;

                case PurchaseOrderStatus.Completed:
                    return PurchaseOrderTransactionType.Completion;

                case PurchaseOrderStatus.Cancelled:
                    return PurchaseOrderTransactionType.Cancellation;

                default:
                    return PurchaseOrderTransactionType.Creation;
            }
        }
    }
}

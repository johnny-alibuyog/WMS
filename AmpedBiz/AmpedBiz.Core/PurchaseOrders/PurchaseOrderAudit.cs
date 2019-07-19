using AmpedBiz.Core.Common;
using AmpedBiz.Core.Users;
using System;

namespace AmpedBiz.Core.PurchaseOrders
{
    public class PurchaseOrderAudit : TransactionAuditBase
    {
        public virtual PurchaseOrder PurchaseOrder { get; protected internal set; }

        public virtual PurchaseOrderStatus Status { get; protected internal set; }

        public virtual PurchaseOrderTransactionType Type { get; protected internal set; }

        public PurchaseOrderAudit() : this(
            purchaseOrder: null,
            transactedBy: null,
            transactedOn: default(DateTime),
            status: default(PurchaseOrderStatus),
            type: default(PurchaseOrderTransactionType),
            id: default(Guid))
        { }

        public PurchaseOrderAudit(
            PurchaseOrder purchaseOrder,
            User transactedBy,
            DateTime transactedOn = default(DateTime),
            PurchaseOrderStatus status = default(PurchaseOrderStatus),
            PurchaseOrderTransactionType type = default(PurchaseOrderTransactionType),
            Guid id = default(Guid)) : base(id)
        {
            this.PurchaseOrder = purchaseOrder;
            this.TransactedBy = transactedBy;
            this.TransactedOn = transactedOn != default(DateTime) ? transactedOn : DateTime.Now;
            this.Status = status;
            this.Type = type;
        }
    }
}

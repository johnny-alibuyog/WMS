using System;

namespace AmpedBiz.Core.Entities
{
    public class PurchaseOrderTransaction : Entity<Guid, PurchaseOrderTransaction>
    {
        //public virtual PurchaseOrder PurchaseOrder { get; protected internal set; }

        //public virtual User TransactedBy { get; protected internal set; }

        //public virtual DateTime TransactedOn { get; protected internal set; }

        //public virtual PurchaseOrderStatus Status { get; protected internal set; }

        //public virtual PurchaseOrderTransactionType Type { get; protected internal set; }

        //public PurchaseOrderTransaction() : this(
        //    order: null,
        //    transactedBy: null,
        //    transactedOn: default(DateTime),
        //    status: default(OrderStatus),
        //    type: default(OrderTransactionType),
        //    id: default(Guid))
        //{ }

        //public PurchaseOrderTransaction(
        //    Order order,
        //    User transactedBy,
        //    DateTime transactedOn = default(DateTime),
        //    OrderStatus status = default(OrderStatus),
        //    OrderTransactionType type = default(OrderTransactionType),
        //    Guid id = default(Guid)) : base(id)
        //{
        //    this.Order = order;
        //    this.TransactedBy = transactedBy;
        //    this.TransactedOn = transactedOn != default(DateTime) ? transactedOn : DateTime.Now;
        //    this.Status = status;
        //    this.Type = type;
        //}
        public PurchaseOrderTransaction(Guid id) : base(id)
        {
        }
    }
}

using AmpedBiz.Core.Common;
using AmpedBiz.Core.Users;
using System;

namespace AmpedBiz.Core.Orders
{
    public class OrderAudit : TransactionAuditBase
    {
        public virtual Order Order { get; protected internal set; }

        public virtual OrderStatus Status { get; protected internal set; }

        public virtual OrderTransactionType Type { get; protected internal set; }

        public OrderAudit() : this(
            order: null, 
            transactedBy: null, 
            transactedOn: default(DateTime), 
            status: default(OrderStatus), 
            type: default(OrderTransactionType),
            id: default(Guid)) { }

        public OrderAudit(
            Order order, 
            User transactedBy, 
            DateTime transactedOn = default(DateTime), 
            OrderStatus status = default(OrderStatus),
            OrderTransactionType type = default(OrderTransactionType),
            Guid id = default(Guid)) : base(id)
        {
            this.Order = order;
            this.TransactedBy = transactedBy;
            this.TransactedOn = transactedOn != default(DateTime) ? transactedOn : DateTime.Now;
            this.Status = status;
            this.Type = type;
        }
    }
}

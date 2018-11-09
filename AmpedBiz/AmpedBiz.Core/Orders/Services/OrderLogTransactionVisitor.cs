using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Users;
using System;

namespace AmpedBiz.Core.Orders.Services
{
    public class OrderLogTransactionVisitor : IVisitor<Order>
    {
        public virtual User TransactedBy { get; private set; }

        public virtual DateTime TransactedOn { get; private set; }

        public virtual OrderTransactionType? Type { get; private set; }

        public OrderLogTransactionVisitor(User transactedBy, DateTime transactedOn, OrderTransactionType? type = null)
        {
            this.TransactedBy = transactedBy;
            this.TransactedOn = transactedOn;
            this.Type = type;
        }

        public void Visit(Order target)
        {
            target.Transactions.Add(new OrderTransaction(
                order: target,
                transactedBy: this.TransactedBy,
                transactedOn: this.TransactedOn,
                status: target.Status,
                type: this.Type ?? this.MapTypeBy(target.Status)
            ));
        }

        private OrderTransactionType MapTypeBy(OrderStatus status)
        {
            switch (status)
            {
                case OrderStatus.Created:
                    return OrderTransactionType.Creation;

                case OrderStatus.Invoiced:
                    return OrderTransactionType.Invoicing;

                case OrderStatus.Staged:
                    return OrderTransactionType.Staging;

                case OrderStatus.Routed:
                    return OrderTransactionType.Routing;

                case OrderStatus.Shipped:
                    return OrderTransactionType.Shipping;

                case OrderStatus.Completed:
                    return OrderTransactionType.Completion;

                case OrderStatus.Cancelled:
                    return OrderTransactionType.Cancellation;

                default:
                    return OrderTransactionType.Creation;
            }
        }
    }
}

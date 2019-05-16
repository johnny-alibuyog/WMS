using AmpedBiz.Core.SharedKernel;
using AmpedBiz.Core.Users;
using System;

namespace AmpedBiz.Core.Orders.Services
{
	public class OrderCompletedVisitor : IVisitor<Order>
    {
        public virtual DateTime? CompletedOn { get; set; }

        public virtual User CompletedBy { get; set; }

        public virtual void Visit(Order target)
        {
            target.CompletedBy = this.CompletedBy;
            target.CompletedOn = this.CompletedOn;
            target.Status = OrderStatus.Completed;
            target.Accept(new OrderLogTransactionVisitor(
                transactedBy: this.CompletedBy,
                transactedOn: this.CompletedOn ?? DateTime.Now
            ));

        }
    }
}

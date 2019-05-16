using AmpedBiz.Core.SharedKernel;
using AmpedBiz.Core.Users;
using System;

namespace AmpedBiz.Core.Orders.Services
{
	public class OrderRoutedVisitor : IVisitor<Order>
    {
        public virtual DateTime? RoutedOn { get; set; }

        public virtual User RoutedBy { get; set; }

        public virtual void Visit(Order target)
        {
            target.RoutedBy = this.RoutedBy ?? target.RoutedBy;
            target.RoutedOn = this.RoutedOn ?? target.RoutedOn;
            target.Status = OrderStatus.Routed;
            target.Accept(new OrderLogTransactionVisitor(
                transactedBy: this.RoutedBy,
                transactedOn: this.RoutedOn ?? DateTime.Now
            ));
        }
    }
}

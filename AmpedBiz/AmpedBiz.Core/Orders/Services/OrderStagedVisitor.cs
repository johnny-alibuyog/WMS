﻿using AmpedBiz.Core.SharedKernel;
using AmpedBiz.Core.Users;
using System;

namespace AmpedBiz.Core.Orders.Services
{
	public class OrderStagedVisitor : IVisitor<Order>
    {
        public virtual DateTime? StagedOn { get; set; }

        public virtual User StagedBy { get; set; }

        public virtual void Visit(Order target)
        {
            target.StagedBy = this.StagedBy ?? target.StagedBy;
            target.StagedOn = this.StagedOn ?? target.StagedOn;
            target.Status = OrderStatus.Staged;
            target.Accept(new OrderLogTransactionVisitor(
                transactedBy: this.StagedBy,
                transactedOn: this.StagedOn ?? DateTime.Now
            ));
        }
    }
}

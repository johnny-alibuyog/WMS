﻿using AmpedBiz.Core.Entities;
using System;

namespace AmpedBiz.Core.Services.Orders
{
    public class OrderRoutedVisitor : OrderVisitor
    {
        public virtual DateTime? RoutedOn { get; set; }

        public virtual User RoutedBy { get; set; }

        public override void Visit(Order target)
        {
            target.RoutedBy = this.RoutedBy ?? target.RoutedBy;
            target.RoutedOn = this.RoutedOn ?? target.RoutedOn;
            target.Status = OrderStatus.Routed;
        }
    }
}

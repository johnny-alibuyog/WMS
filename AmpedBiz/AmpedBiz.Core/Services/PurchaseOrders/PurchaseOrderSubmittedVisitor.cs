﻿using AmpedBiz.Core.Entities;
using System;

namespace AmpedBiz.Core.Services.PurchaseOrders
{
    public class PurchaseOrderSubmittedVisitor : PurchaseOrderVisitor
    {
        public virtual User SubmittedBy { get; set; }

        public virtual DateTime? SubmittedOn { get; set; }

        public override void Visit(PurchaseOrder target)
        {
            target.SubmittedBy = this.SubmittedBy;
            target.SubmittedOn = this.SubmittedOn;
            target.Status = PurchaseOrderStatus.Submitted;
        }
    }
}
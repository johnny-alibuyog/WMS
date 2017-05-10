﻿using AmpedBiz.Core.Entities;
using System;
using System.Collections.Generic;

namespace AmpedBiz.Core.Services.Returns
{
    public class ReturnSaveVisitor : ReturnVisitor
    {
        public virtual Branch Branch  { get; set; }

        public virtual Customer Customer  { get; set; }

        public virtual User ReturnedBy  { get; set; }

        public virtual DateTime? ReturnedOn  { get; set; }

        public virtual string Remarks  { get; set; }

        public virtual IEnumerable<ReturnItem> Items  { get; set; }

        public override void Visit(Return target)
        {
            target.Branch = this.Branch;
            target.Customer = this.Customer;
            target.ReturnedBy = this.ReturnedBy;
            target.ReturnedOn = this.ReturnedOn;
            target.Remarks = this.Remarks;
            target.Accept(new ReturnUpdateItemVisitor() { Items = this.Items });
            target.Accept(new ReturnCalculateTotalVisitor());
        }
    }
}

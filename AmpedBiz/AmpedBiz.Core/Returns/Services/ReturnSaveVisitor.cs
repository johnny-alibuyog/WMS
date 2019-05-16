using AmpedBiz.Core.Common;
using AmpedBiz.Core.SharedKernel;
using AmpedBiz.Core.Users;
using System;
using System.Collections.Generic;

namespace AmpedBiz.Core.Returns.Services
{
	public class ReturnSaveVisitor : IVisitor<Return>
    {
        public virtual Branch Branch  { get; set; }

        public virtual Customer Customer  { get; set; }

        public virtual User ReturnedBy  { get; set; }

        public virtual DateTime? ReturnedOn  { get; set; }

        public virtual string Remarks  { get; set; }

        public virtual IEnumerable<ReturnItem> Items  { get; set; }

        public virtual void Visit(Return target)
        {
            target.Branch = this.Branch;
            target.Customer = this.Customer;
            target.ReturnedBy = this.ReturnedBy;
            target.ReturnedOn = this.ReturnedOn;
            target.Remarks = this.Remarks;
            target.Accept(new ReturnUpdateItemVisitor(this.Items, this.Branch));
            target.Accept(new ReturnCalculateTotalVisitor());
        }
    }
}

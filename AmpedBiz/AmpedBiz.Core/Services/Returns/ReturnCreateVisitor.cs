using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Core.Services.Returns
{
    public class ReturnCreateVisitor : ReturnVisitor
    {
        public virtual Branch Branch  { get; set; }

        public virtual Customer Customer  { get; set; }

        public virtual User ReturnedBy  { get; set; }

        public virtual DateTime? ReturnedOn  { get; set; }

        public virtual ReturnReason Reason  { get; set; }

        public virtual string Remarks  { get; set; }

        public virtual IEnumerable<ReturnItem> Items  { get; set; }

        public override void Visit(Return target)
        {
            this.SetItemsTo(target);

            target.Branch = this.Branch;
            target.Customer = this.Customer;
            target.ReturnedBy = this.ReturnedBy;
            target.ReturnedOn = this.ReturnedOn;
            target.Reason = this.Reason;
            target.Remarks = this.Remarks;
            target.Accept(new ReturnCalculateTotalVisitor());
        }

        private void SetItemsTo(Return target)
        {
            if (this.Items.IsNullOrEmpty())
                return;

            var itemsToInsert = this.Items.Except(target.Items).ToList();
            var itemsToUpdate = target.Items.Where(x => this.Items.Contains(x)).ToList();
            var itemsToRemove = target.Items.Except(this.Items).ToList();

            foreach (var item in itemsToInsert)
            {
                item.Return = target;
                target.Items.Add(item);
            }

            foreach (var item in itemsToUpdate)
            {
                var value = this.Items.Single(x => x == item);
                item.SerializeWith(value);
                item.Return = target;
            }

            foreach (var item in itemsToRemove)
            {
                item.Return = null;
                target.Items.Remove(item);
            }
        }
    }
}

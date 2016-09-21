using AmpedBiz.Core.Entities;
using AmpedBiz.Common.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Core.Services.Returns
{
    public class ReturnCreateVisitor : ReturnVisitor
    {
        public virtual Branch Branch { get; internal protected set; }

        public virtual Customer Customer { get; internal protected set; }

        public virtual User ReturnedBy { get; internal protected set; }

        public virtual DateTime? ReturnedOn { get; internal protected set; }

        public virtual ReturnReason Reason { get; internal protected set; }

        public virtual string Remarks { get; internal protected set; }

        public virtual IEnumerable<ReturnItem> Items { get; internal protected set; }

        public override void Visit(Return target)
        {
            this.SetItemsTo(target);

            target.Branch = this.Branch;
            target.Customer = this.Customer;
            target.ReturnedBy = this.ReturnedBy;
            target.ReturnedOn = this.ReturnedOn;
            target.Reason = this.Reason;
            target.Remarks = this.Remarks;
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

using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Core.Services.Orders
{
    public class OrderReturnVisitor : OrderVisitor
    {
        public virtual IEnumerable<OrderReturn> Returns { get; set; }

        public override void Visit(Order target)
        {
            this.AddReturnsTo(target);
            target.Status = OrderStatus.Paid;
        }

        private void SetReturnsTo(Order target)
        {
            if (this.Returns.IsNullOrEmpty())
                return;

            var itemsToInsert = this.Returns.Except(target.Returns).ToList();
            var itemsToUpdate = target.Returns.Where(x => this.Returns.Contains(x)).ToList();
            var itemsToRemove = target.Returns.Except(this.Returns).ToList();

            foreach (var item in itemsToInsert)
            {
                item.Order = target;
                target.Returns.Add(item);
            }

            foreach (var item in itemsToUpdate)
            {
                var value = this.Returns.Single(x => x == item);
                item.SerializeWith(value);
                item.Order = target;
            }

            foreach (var item in itemsToRemove)
            {
                item.Order = null;
                target.Returns.Remove(item);
            }
        }

        private void AddReturnsTo(Order target)
        {
            var lastReturn = this.Returns.OrderBy(x => x.ReturnedOn).LastOrDefault();
            if (lastReturn == null)
                return;

            target.ReturnedOn = lastReturn.ReturnedOn;
            target.ReturnedBy = lastReturn.ReturnedBy;

            foreach (var @return in this.Returns)
            {
                @return.Order = target;
                target.Returns.Add(@return);
            }
        }
    }
}

using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Core.Services.Orders
{
    public class OrderUpdateReturnsVisitor : OrderVisitor
    {
        public virtual IEnumerable<OrderReturn> Returns { get; set; }

        public OrderUpdateReturnsVisitor(IEnumerable<OrderReturn> returns)
        {
            this.Returns = returns;
        }

        public override void Visit(Order target)
        {
            if (this.Returns.IsNullOrEmpty())
                return;

            var itemsToInsert = this.Returns.Except(target.Returns).ToList();
            //var itemsToUpdate = target.Returns.Where(x => this.Returns.Contains(x)).ToList();
            var itemsToRemove = target.Returns.Except(this.Returns).ToList();

            foreach (var item in itemsToInsert)
            {
                item.Order = target;
                target.Returns.Add(item);
            }

            //foreach (var item in itemsToUpdate)
            //{
            //    var value = this.Returns.Single(x => x == item);
            //    item.SerializeWith(value);
            //    item.Order = target;
            //}

            foreach (var item in itemsToRemove)
            {
                item.Order = null;
                target.Returns.Remove(item);
            }
        }
    }
}

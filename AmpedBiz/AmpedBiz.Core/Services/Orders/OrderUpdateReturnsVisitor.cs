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

            // allow only insert. edit and delete is not allowed for this aggregate
            var itemsToInsert = this.Returns.Except(target.Returns).ToList();

            foreach (var item in itemsToInsert)
            {
                item.Order = target;
                target.Returns.Add(item);
            }
        }
    }
}

using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Core.Services.Orders
{
    public class OrderUpdateItemsVisitor : IVisitor<Order>
    {
        public virtual IEnumerable<OrderItem> Items { get; set; }

        public OrderUpdateItemsVisitor(IEnumerable<OrderItem> items)
        {
            this.Items = items;
        }

        public virtual void Visit(Order target)
        {
            if (this.Items.IsNullOrEmpty())
                return;

            var itemsToInsert = this.Items.Except(target.Items).ToList();
            var itemsToUpdate = target.Items.Where(x => this.Items.Contains(x)).ToList();
            var itemsToRemove = target.Items.Except(this.Items).ToList();

            foreach (var item in itemsToInsert)
            {
                item.Order = target;
                target.Items.Add(item);
            }

            foreach (var item in itemsToUpdate)
            {
                var value = this.Items.Single(x => x == item);
                item.SerializeWith(value);
                item.Order = target;
            }

            foreach (var item in itemsToRemove)
            {
                item.Order = null;
                target.Items.Remove(item);
            }
        }
    }
}

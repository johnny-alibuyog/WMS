using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Products;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Core.Services.Returns
{
    public class ReturnUpdateItemVisitor : IVisitor<Return>
    {
        public virtual IEnumerable<ReturnItem> Items { get; set; }

        public virtual void Visit(Return target)
        {
            if (this.Items.IsNullOrEmpty())
                return;

            // allow only insert. edit and delete is not allowed for this aggregate
            var itemsToInsert = this.Items.Except(target.Items).ToList();

            foreach (var item in itemsToInsert)
            {
                item.Return = target;
                item.Product.Accept(new SearchAndApplyVisitor()
                {
                    Branch = null,
                    InventoryVisitor = new Inventories.Orders.ReturnVisitor()
                    {
                        Reason = item.ReturnReason,
                        Quantity = item.Quantity,
                    }
                });
                target.Items.Add(item);
            }
        }
    }
}

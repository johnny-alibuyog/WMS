using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Common;
using AmpedBiz.Core.Inventories.Services.Orders;
using AmpedBiz.Core.Products.Services;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Core.Returns.Services
{
	public class ReturnUpdateItemVisitor : IVisitor<Return>
    {
        public Branch Branch { get; private set; }

        public IEnumerable<ReturnItem> Items { get; private set; }

        public ReturnUpdateItemVisitor(IEnumerable<ReturnItem> items, Branch branch)
        {
            this.Items = items;
            this.Branch = branch;
        }

        public void Visit(Return target)
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
                    Branch = this.Branch,
                    InventoryVisitor = new ReturnVisitor()
                    {
                        Reason = item.Reason,
                        QuantityStandardEquivalent = item.QuantityStandardEquivalent,
                    }
                });
                target.Items.Add(item);
            }
        }
    }
}

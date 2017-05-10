using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Inventories;

namespace AmpedBiz.Core.Services.Products
{
    public class SearchAndApplyVisitor : ProductVisitor
    {
        public virtual Branch Branch { get; set; }

        public virtual InventoryVisitor InventoryVisitor { get; set; }

        public override void Visit(Product target)
        {
            // search for the inventory. this is in preparation of product 
            // having multiple inventories based on the branch.
            var inventory = target.Inventory; 

            inventory.Accept(this.InventoryVisitor);
        }
    }
}

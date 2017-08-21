using AmpedBiz.Core.Entities;

namespace AmpedBiz.Core.Services.Products
{
    public class SearchAndApplyVisitor : IVisitor<Product>
    {
        public virtual Branch Branch { get; set; }

        public virtual IVisitor<Inventory> InventoryVisitor { get; set; }

        public virtual void Visit(Product target)
        {
            // search for the inventory. this is in preparation of product 
            // having multiple inventories based on the branch.
            var inventory = target.Inventory; 

            inventory.Accept(this.InventoryVisitor);
        }
    }
}

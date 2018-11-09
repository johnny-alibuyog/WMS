using AmpedBiz.Core.Common;
using AmpedBiz.Core.Inventories;
using System.Linq;

namespace AmpedBiz.Core.Products.Services
{
	// TODO: For performance reasons, remove this soon. This results in a n+1 query since product inventories is navigated to.
	public class SearchAndApplyVisitor : IVisitor<Product>
    {
        public virtual Branch Branch { get; set; }

        public virtual IVisitor<Inventory> InventoryVisitor { get; set; }

        public virtual void Visit(Product target)
        {
            // search for the inventory. this is in preparation of product 
            // having multiple inventories based on the branch.
            var inventory = target.Inventories.FirstOrDefault(x => x.Branch == this.Branch); 

            inventory.Accept(this.InventoryVisitor);
        }
    }
}

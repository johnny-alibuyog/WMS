using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.SharedKernel;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Core.Products.Services
{
	public class ProductSuppliersUpdateVisitor : IVisitor<Product>
    {
        public IEnumerable<Supplier> Suppliers { get; set; }

        public ProductSuppliersUpdateVisitor(IEnumerable<Supplier> suppliers)
        {
            this.Suppliers = suppliers;
        }

        public virtual void Visit(Product target)
        {
            if (this.Suppliers == null)
            {
                return;
            }

            var suppliersToAdd = this.Suppliers.Except(target.Suppliers).ToList();

            var suppliersToRemove = target.Suppliers.Except(this.Suppliers).ToList();

            foreach (var role in suppliersToRemove)
            {
                var item = target.Suppliers.FirstOrDefault(x => x == role);
                target.Suppliers.Remove(item);
            }

            foreach (var role in suppliersToAdd)
            {
                target.Suppliers.Add(role);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Core.Entities
{
    public class ProductCategory : Entity<ProductCategory, string>
    {
        public virtual string Name { get; set; }

        public ProductCategory() { }

        public ProductCategory(string id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public static readonly ProductCategory Food = new ProductCategory("F", "Food");

        public static readonly ProductCategory Drinks = new ProductCategory("D", "Drinks");

        public static readonly ProductCategory Medecine = new ProductCategory("M", "Medicine");

        public static readonly IEnumerable<ProductCategory> All = new List<ProductCategory>()
        {
            ProductCategory.Food,
            ProductCategory.Drinks,
            ProductCategory.Medecine
        };
    }
}

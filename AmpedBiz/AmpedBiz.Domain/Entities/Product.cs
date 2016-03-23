using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Domain.Entities
{
    public class Product : Entity<Product, Guid>
    {
        public virtual string Name { get; set; }

        public virtual Currency Price { get; set; }

        public virtual ProductCategory Category { get; set; }
    }
}

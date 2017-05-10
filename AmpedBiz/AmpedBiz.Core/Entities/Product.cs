using AmpedBiz.Core.Services;
using AmpedBiz.Core.Services.Inventories;
using System;

namespace AmpedBiz.Core.Entities
{
    public class Product : Entity<Guid, Product>, IAccept<ProductVisitor>
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual Supplier Supplier { get; set; }

        public virtual ProductCategory Category { get; set; }

        public virtual string Image { get; set; }

        public virtual bool Discontinued { get; set; }

        public virtual Inventory Inventory { get; set; }

        public Product() : base(default(Guid))
        {
            this.Inventory = new Inventory(this);
        }

        public Product(Guid id) : base(id)
        {
            this.Inventory = new Inventory(this);
        }

        public virtual void Accept(ProductVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
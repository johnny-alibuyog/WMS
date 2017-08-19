using AmpedBiz.Core.Services;
using AmpedBiz.Core.Services.Products;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AmpedBiz.Core.Entities
{
    public class Product : Entity<Guid, Product>, IAccept<ProductVisitor>
    {
        public virtual string Code { get; protected internal set; }

        public virtual string Name { get; protected internal set; }

        public virtual string Description { get; protected internal set; }

        public virtual Supplier Supplier { get; protected internal set; }

        public virtual ProductCategory Category { get; protected internal set; }

        public virtual string Image { get; protected internal set; }

        public virtual bool Discontinued { get; protected internal set; }

        public virtual Inventory Inventory { get; protected internal set; }

        public virtual IEnumerable<ProductUnitOfMeasure> UnitOfMeasures { get; protected internal set; } 

        public Product() : base(default(Guid))
        {
            this.Inventory = new Inventory(this);
            this.UnitOfMeasures = new Collection<ProductUnitOfMeasure>();
        }

        public Product(Guid id) : base(id)
        {
            this.Inventory = new Inventory(this);
            this.UnitOfMeasures = new Collection<ProductUnitOfMeasure>();
        }

        public virtual void Accept(ProductVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
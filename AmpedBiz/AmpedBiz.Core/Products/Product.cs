using AmpedBiz.Core.Common;
using AmpedBiz.Core.Inventories;
using AmpedBiz.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AmpedBiz.Core.Products
{
    public class Product : Entity<Guid, Product>, IAccept<IVisitor<Product>>, IHasTenant
    {
        public virtual string Code { get; protected internal set; }

        public virtual string Name { get; protected internal set; }

        public virtual string Description { get; protected internal set; }

        public virtual Tenant Tenant { get; set; }

        public virtual ProductCategory Category { get; protected internal set; }

        public virtual string Image { get; protected internal set; }

        public virtual bool Discontinued { get; protected internal set; }

        public virtual IEnumerable<Supplier> Suppliers { get; protected internal set; }

        public virtual IEnumerable<Inventory> Inventories { get; protected internal set; }

        public virtual IEnumerable<ProductUnitOfMeasure> UnitOfMeasures { get; protected internal set; }

        public Product() : this(default(Guid)) { }

        public Product(Guid id) : base(id)
        {
            this.Suppliers = new Collection<Supplier>();
            this.Inventories = new Collection<Inventory>();
            this.UnitOfMeasures = new Collection<ProductUnitOfMeasure>();
        }

        public virtual void Accept(IVisitor<Product> visitor)
        {
            visitor.Visit(this);
        }
    }

    public static class ProductExtention
    {
        public static Measure StandardEquivalentMeasureOf(this Product product, UnitOfMeasure unit)
        {
            var standardEquivalentValue = product.UnitOfMeasures
                .Where(x => x.UnitOfMeasure == unit)
                .Select(x => x.StandardEquivalentValue)
                .FirstOrDefault();

            var standardUnitOfMeasure = product.UnitOfMeasures.Standard(x => x.UnitOfMeasure);

            return new Measure(value: standardEquivalentValue, unit: standardUnitOfMeasure);
        }
    }
}
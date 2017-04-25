using AmpedBiz.Core.Entities;

namespace AmpedBiz.Core.Services.Products
{
    public class ProductsCreatedVisitor
    {
        public virtual ProductCategory Category { get; set; }

        public virtual Supplier Supplier { get; set; }

        public virtual bool Discontinued { get; set; }

        public virtual UnitOfMeasure UnitOfMeasure { get; set; }

        public virtual UnitOfMeasure UnitOfMeasureBase { get; set; }

        public virtual decimal? ConversionFactor { get; set; }

        public virtual Money BasePrice { get; set; }

        public virtual Money WholesalePrice { get; set; }

        public virtual Money RetailPrice { get; set; }

        public virtual Money BadStockPrice { get; set; }

        public virtual UnitOfMeasure InitialLevel { get; set; }

        public virtual UnitOfMeasure TargetLevel { get; set; }

        public virtual UnitOfMeasure ReorderLevel { get; set; }

        public virtual UnitOfMeasure MinimumReorderQuantity { get; set; }
    }
}

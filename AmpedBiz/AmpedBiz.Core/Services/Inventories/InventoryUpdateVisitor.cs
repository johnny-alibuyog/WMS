using AmpedBiz.Core.Entities;

namespace AmpedBiz.Core.Services.Inventories
{
    public class InventoryUpdateVisitor : InventoryVisitor
    {
        public Branch Branch { get; set; }

        public Product Product { get; set; }

        public string IndividualBarcode { get; set; }

        public string PackagingBarcode { get; set; }

        public UnitOfMeasure UnitOfMeasure { get; set; }

        public UnitOfMeasure PackagingUnitOfMeasure { get; set; }

        public decimal? PackagingSize { get; set; }

        public Money BasePrice { get; set; }

        public Money WholesalePrice { get; set; }

        public Money RetailPrice { get; set; }

        public Money BadStockPrice { get; set; }

        public virtual Measure InitialLevel { get; set; }

        public virtual Measure TargetLevel { get; set; }

        public virtual Measure ReorderLevel { get; set; }

        public virtual Measure MinimumReorderQuantity { get; set; }

        public override void Visit(Inventory target)
        {
            target.Branch = this.Branch ?? target.Branch;
            target.Product = this.Product ?? target.Product;
            target.IndividualBarcode = this.IndividualBarcode ?? target.IndividualBarcode;
            target.PackagingBarcode = this.PackagingBarcode ?? target.PackagingBarcode;
            target.UnitOfMeasure = this.UnitOfMeasure ?? target.UnitOfMeasure;
            target.PackagingUnitOfMeasure = this.PackagingUnitOfMeasure ?? target.PackagingUnitOfMeasure;
            target.PackagingSize = this.PackagingSize ?? target.PackagingSize;
            target.BasePrice = this.BasePrice ?? target.BasePrice;
            target.WholesalePrice = this.WholesalePrice ?? target.WholesalePrice;
            target.RetailPrice = this.RetailPrice ?? target.RetailPrice;
            target.BadStockPrice = this.BadStockPrice ?? target.BasePrice;
            target.InitialLevel = this.InitialLevel ?? target.InitialLevel;
            target.TargetLevel = this.TargetLevel ?? target.TargetLevel;
            target.ReorderLevel = this.ReorderLevel ?? target.ReorderLevel;
            target.MinimumReorderQuantity = this.MinimumReorderQuantity ?? target.MinimumReorderQuantity;
            target.Accept(new InventoryRecomputeVisitor());
        }
    }
}

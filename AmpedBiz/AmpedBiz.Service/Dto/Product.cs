using AmpedBiz.Common.CustomTypes;
using System;

namespace AmpedBiz.Service.Dto
{
    public class Product
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Lookup<Guid> Supplier { get; set; }

        public Lookup<string> Category { get; set; }

        public bool Discontinued { get; set; }

        public string Image { get; set; }

        public Inventory Inventory { get; set; }

        public Product()
        {
            this.Inventory = new Inventory();
        }
    }

    public class ProductPageItem
    {
        public string Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string SupplierName { get; set; }

        public string CategoryName { get; set; }

        public string Image { get; set; }

        public decimal? BasePriceAmount { get; set; }

        public decimal? WholesalePriceAmount { get; set; }

        public decimal? RetailPriceAmount { get; set; }

        public bool Discontinued { get; set; }
    }

    public class DiscontinuedPageItem
    {
        public string Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string SupplierName { get; set; }

        public string CategoryName { get; set; }

        public string Image { get; set; }

        public decimal? BasePriceAmount { get; set; }

        public decimal? WholesalePriceAmount { get; set; }

        public decimal? RetailPriceAmount { get; set; }
    }

    public class ProductInventory
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string UnitOfMeasure { get; set; }

        public string PackagingUnitOfMeasure { get; set; }

        public decimal? PackagingSize { get; set; }

        public decimal? TargetValue { get; set; }

        public decimal? AvailableValue { get; set; }

        public decimal? BadStockValue { get; set; }

        public decimal? BasePriceAmount { get; set; }

        public decimal? WholesalePriceAmount { get; set; }

        public decimal? RetailPriceAmount { get; set; }

        public decimal? BadStockPriceAmount { get; set; }

        public decimal? DiscountAmount { get; set; }
    }

    public class ProductInventoryLevelPageItem
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Supplier { get; set; }

        public string UnitOfMeasure { get; set; }

        public virtual decimal? OnHandValue { get; set; }

        public virtual decimal? AllocatedValue { get; set; }

        public virtual decimal? AvailableValue { get; set; }

        public virtual decimal? OnOrderValue { get; set; }

        public virtual decimal? CurrentLevelValue { get; set; }

        public virtual decimal? TargetLevelValue { get; set; }

        public virtual decimal? BelowTargetLevelValue { get; set; }
    }

    public class ProductOrderPageItem
    {
        public Guid Id { get; set; }

        public string OrderNumber { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string Status { get; set; }

        public string CustomerName { get; set; }

        public decimal QuantityValue { get; set; }
    }

    public class ProductPurchasePageItem
    {
        public Guid Id { get; set; }

        public string PurchaseOrderNumber { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string Status { get; set; }

        public string SupplierName { get; set; }

        public decimal? UnitCostAmount { get; set; }

        public decimal? QuantityValue { get; set; }

        public decimal? ReceivedValue { get; set; }
    }

    public class NeedsReorderingPageItem
    {
        public string Id { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public string SupplierName { get; set; }

        public string CategoryName { get; set; }

        public decimal? AvailableValue { get; set; }

        public decimal? ReorderLevelValue { get; set; }

        public decimal? CurrentLevelValue { get; set; }

        public decimal? TargetLevelValue { get; set; }

        public decimal? BelowTargetValue { get; set; }
    }

    public class ProductReportPageItem
    {
        public Guid Id { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public string CategoryName { get; set; }

        public string SupplierName { get; set; }

        public decimal? OnHandValue { get; set; }

        public decimal? BasePriceAmount { get; set; }

        public decimal? WholesalePriceAmount { get; set; }

        public decimal? RetailPriceAmount { get; set; }

        public decimal? TotalBasePriceAmount { get; set; }

        public decimal? TotalWholesalePriceAmount { get; set; }

        public decimal? TotalRetailPriceAmount { get; set; }
    }
}
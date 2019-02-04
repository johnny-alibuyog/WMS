using AmpedBiz.Common.CustomTypes;
using AmpedBiz.Common.Extentions;
using System;
using System.Collections.Generic;

namespace AmpedBiz.Service.Dto
{
    public class Product
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Lookup<string> Category { get; set; }

        public bool? Discontinued { get; set; }

        public string Image { get; set; }

        public Inventory Inventory { get; set; } = new Inventory();

        public List<Supplier> Suppliers { get; set; } = new List<Supplier>();

        public List<ProductUnitOfMeasure> UnitOfMeasures { get; set; } = new List<ProductUnitOfMeasure>();
    }

    public class ProductPageItem
    {
        public string Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

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

        public string CategoryName { get; set; }

        public string Image { get; set; }

        public decimal? BasePriceAmount { get; set; }

        public decimal? WholesalePriceAmount { get; set; }

        public decimal? RetailPriceAmount { get; set; }
    }

    public class ProductInventory
    {
        public Guid Id { get; set; }

        public Guid InventoryId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public List<ProductInventoryUnitOfMeasure> UnitOfMeasures { get; set; } = new List<ProductInventoryUnitOfMeasure>();
    }

    public class ProductInventoryUnitOfMeasure
    {
		public bool IsStandard { get; set; }

        public bool IsDefault { get; set; }

		public string Barcode { get; set; }

		public UnitOfMeasure UnitOfMeasure { get; set; }

        public Measure Available { get; set; }

        public Measure TargetLevel { get; set; }

        public Measure BadStock { get; set; }

        public Measure Standard { get; set; }

        public List<ProductInventoryUnitOfMeasurePrice> Prices { get; set; } = new List<ProductInventoryUnitOfMeasurePrice>();
    }

	public class ProductInventoryUnitOfMeasurePrice
    {
		public Guid Id { get; set; }

        public Lookup<string> Pricing { get; set; }

        public decimal? PriceAmount { get; set; }
    }

    public class ProductInventoryLevelPageItem
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string UnitOfMeasure { get; set; }

        public string OnHand { get; set; }

        public string Allocated { get; set; }

        public string Available { get; set; }

        public string OnOrder { get; set; }

        public string CurrentLevel { get; set; }

        public string ReorderLevel { get; set; }

        public string TargetLevel { get; set; }

        public string BelowTargetLevel { get; set; }

        public decimal? OnHandValue { get; set; }

        public decimal? AllocatedValue { get; set; }

        public decimal? AvailableValue { get; set; }

        public decimal? OnOrderValue { get; set; }

        public decimal? CurrentLevelValue { get; set; }

        public decimal? ReorderLevelValue { get; set; }

        public decimal? TargetLevelValue { get; set; }

        public decimal? BelowTargetLevelValue { get; set; }
    }

    public class ProductOrderPageItem
    {
        public Guid Id { get; set; }

        public string OrderNumber { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string Status { get; set; }

        public string CustomerName { get; set; }

        public Measure Quantity { get; set; }
    }

    public class ProductReturnPageItem
    {
        public Guid Id { get; set; }

        public string ReasonName { get; set; }

        public DateTime? ReturnedOn { get; set; }

        public string ReturnedByName { get; set; }

        public decimal ReturnedAmount { get; set; }

        public decimal QuantityValue { get; set; }
    }

    public class ProductOrderReturnPageItem
    {
        public Guid Id { get; set; }

        public string ReasonName { get; set; }

        public DateTime? ReturnedOn { get; set; }

        public string ReturnedByName { get; set; }

        public decimal ReturnedAmount { get; set; }

        public Measure Quantity { get; set; }
    }

    public class ProductPurchasePageItem
    {
        public Guid Id { get; set; }

        public string PurchaseOrderNumber { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string Status { get; set; }

        public string SupplierName { get; set; }

        public decimal? UnitCostAmount { get; set; }

        public decimal? ReceivedValue { get; set; }

        public Measure Quantity { get; set; }
    }

    public class NeedsReorderingPageItem
    {
        public string Id { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public string CategoryName { get; set; }

        public string UnitOfMeasureName { get; set; }

        public decimal? AvailableValue { get; set; }

        public decimal? ReorderLevelValue { get; set; }

        public decimal? CurrentLevelValue { get; set; }

        public decimal? TargetLevelValue { get; set; }

        public decimal? BelowTargetValue { get; set; }

        public decimal? MinimumReorderQuantity { get; set; }

        public decimal? ReorderQuantity
        {
            get
            {
                return this.BelowTargetValue > this.MinimumReorderQuantity
                  ? this.BelowTargetValue
                  : this.MinimumReorderQuantity;
            }
        }
    }

    public class ProductReportPageItem
    {
        public Guid Id { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public string CategoryName { get; set; }

        public string OnHandUnit { get; set; }

        public decimal? OnHandValue { get; set; }

        public decimal? BasePriceAmount { get; set; }

        public decimal? WholesalePriceAmount { get; set; }

        public decimal? RetailPriceAmount { get; set; }

        public decimal? TotalBasePriceAmount => this.OnHandValue * this.BasePriceAmount;

        public decimal? TotalWholesalePriceAmount => this.OnHandValue * this.WholesalePriceAmount;

        public decimal? TotalRetailPriceAmount => this.OnHandValue * this.RetailPriceAmount;
    }

    public class ProductsDeliveredReportPageItem
    {
        public string BranchName { get; set; }

        public string CategoryName { get; set; }

        public string ProductName { get; set; }

        public string QuantityUnit { get; set; }

        public decimal? QuantityValue { get; set; }

        public decimal? UnitPriceAmount { get; set; }

        public decimal? DiscountAmount { get; set; }

        public decimal? ExtendedPriceAmount { get; set; }

        public decimal? TotalPriceAmount { get; set; }
    }

    public class ProductListingReportPageItem : IEquatable<ProductListingReportPageItem>
    {
        public string BranchName { get; set; }

        public string CategoryName { get; set; }

        public string ProductName { get; set; }

        public string QuantityUnit { get; set; }

        public decimal? OnHandValue { get; set; }

        public decimal? AvailableValue { get; set; }

        public decimal? BasePriceAmount { get; set; }

        public decimal? WholesalePriceAmount { get; set; }

        public decimal? RetailPriceAmount { get; set; }

        public decimal? SuggestedRetailPriceAmount { get; set; }

        public bool Equals(ProductListingReportPageItem other)
        {
            if (other == null)
                return false;

            if (this.BranchName != other.BranchName)
                return false;

            if (this.CategoryName != other.CategoryName)
                return false;

            if (this.ProductName != other.BranchName)
                return false;

            if (this.QuantityUnit != other.QuantityUnit)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            var hashCode = 17;

            hashCode += AddToHashIfValueIsNotDefault(this.BranchName, hashCode);
            hashCode += AddToHashIfValueIsNotDefault(this.CategoryName, hashCode);
            hashCode += AddToHashIfValueIsNotDefault(this.ProductName, hashCode);
            hashCode += AddToHashIfValueIsNotDefault(this.QuantityUnit, hashCode);

            return hashCode;
        }

        private int AddToHashIfValueIsNotDefault(object value, int hashCode)
        {
            if (value.IsNullOrDefault())
                return 0;

            return hashCode * 59 + value.GetHashCode();
        }
    }
}
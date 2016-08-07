﻿using AmpedBiz.Common.CustomTypes;
using System;

namespace AmpedBiz.Service.Dto
{
    public class Product
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Lookup<string> Supplier { get; set; }

        public Lookup<string> Category { get; set; }

        public bool Discontinued { get; set; }

        public string Image { get; set; }

        public Inventory Inventory { get; set; }
    }

    public class ProductPageItem
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string SupplierName { get; set; }

        public string CategoryName { get; set; }

        public string Image { get; set; }

        public decimal? BasePriceAmount { get; set; }

        public decimal? RetailPriceAmount { get; set; }

        public decimal? WholesalePriceAmount { get; set; }

        public bool Discontinued { get; set; }
    }

    public class ProductInventory
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string UnitOfMeasure { get; set; }

        public decimal? TargetValue { get; set; }

        public decimal? AvailableValue { get; set; }

        public decimal? BasePriceAmount { get; set; }

        public decimal? RetailPriceAmount { get; set; }

        public decimal? WholeSalePriceAmount { get; set; }

        public decimal? DiscountAmount { get; set; }
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
}
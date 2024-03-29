﻿using AmpedBiz.Common.CustomTypes;
using System;

namespace AmpedBiz.Service.Dto
{
    public class OrderItem
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        public Lookup<Guid> Product { get; set; }

        public Measure Quantity { get; set; }

        public Measure Standard { get; set; }

        public decimal DiscountRate { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal UnitPriceAmount { get; set; }

        public decimal ExtendedPriceAmount { get; set; }

        public decimal TotalPriceAmount { get; set; }
    }

    public class OrderItemPageItem
    {
        public Guid Id { get; set; }

        public string ProductName { get; set; }

        public decimal PackagingSize { get; set; }

        public string Quantity { get; set; }

        public string Standard { get; set; }

        public string QuantityStandardEquivalent { get; set; }

        public string Discount { get; set; }

        public string UnitPrice { get; set; }

        public string ExtendedPrice { get; set; }

        public string StatusName { get; set; }

        public bool InsufficientInventory { get; set; }
    }
}
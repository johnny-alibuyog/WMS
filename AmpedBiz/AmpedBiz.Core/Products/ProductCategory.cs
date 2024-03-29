﻿using AmpedBiz.Core.Common;
using AmpedBiz.Core.SharedKernel;
using System.Collections.Generic;

namespace AmpedBiz.Core.Products
{
    public class ProductCategory : Entity<string, ProductCategory>, IHasTenant
    {
        public virtual Tenant Tenant { get; set; }

        public virtual string Name { get; set; }

        public ProductCategory() : base(default(string)) { }

        public ProductCategory(string id, string name = null) : base(id)
        {
            this.Id = id;
            this.Name = name;
        }

        public static readonly ProductCategory Food = new ProductCategory("F", "Food");

        public static readonly ProductCategory Drinks = new ProductCategory("D", "Drinks");

        public static readonly ProductCategory Medecine = new ProductCategory("M", "Medicine");

        public static readonly IEnumerable<ProductCategory> All = new []
        {
            ProductCategory.Food,
            ProductCategory.Drinks,
            ProductCategory.Medecine
        };
    }
}

﻿using AmpedBiz.Common.Configurations;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Inventories;
using AmpedBiz.Core.Services.Products;
using LinqToExcel;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DefaultDataSeeders
{
    public class _021_DefaultProductSeeder : IDefaultDataSeeder
    {
        private readonly Utils _utils;
        private readonly ISessionFactory _sessionFactory;

        public _021_DefaultProductSeeder(ISessionFactory sessionFactory)
        {
            _utils = new Utils(new Random(), sessionFactory);
            this._sessionFactory = sessionFactory;
        }

        public void Seed()
        {
            var filename = Path.Combine(DatabaseConfig.Instance.GetDefaultSeederDataAbsolutePath(), @"default_products.xlsx");

            if (!File.Exists(filename))
                return;

            var productData = new ExcelQueryFactory(filename).Worksheet()
                .Select(x => new
                {
                    Code = x["Product Id"].ToString(),
                    Name = x["Product Name"].ToString(),
                    Category = x["Category"].ToString(),
                    Piece = new
                    {
                        Size = x["Size"].ToString(),
                        StandardEquivalentValue = 1M,
                        UnitOfMeasure = x["Piece UOM"].ToString(),
                        Barcode = x["Individual Barcode"].ToString(),
                        Price = new
                        {
                            WholesalePrice = Convert.ToDecimal(x["Wholesale Price Per Piece"].Cast<double>()),
                            RetailPrice = Convert.ToDecimal(x["Retail Price Per Piece"].Cast<double>()),
                            SuggestedRetailPrice = Convert.ToDecimal(x["Suggested Retail Price"].Cast<double>()),
                        },
                    },
                    Package = new
                    {
                        Size = string.Empty,
                        StandardEquivalentValue = Convert.ToDecimal(x["Piece Per Package"].Cast<double>()),
                        UnitOfMeasure = x["Package UOM"].ToString(),
                        Barcode = x["Packaging Barcode"].ToString(),
                        Price = new
                        {
                            WholesalePrice = Convert.ToDecimal(x["Wholesale Price Per Package"].Cast<double>()),
                            RetailPrice = Convert.ToDecimal(x["Retail Price Per Package"].Cast<double>()),
                            SuggestedRetailPrice = Convert.ToDecimal(x["Retail Price Per Package"].Cast<double>()),
                        }
                    },
                })
                .ToList();

            var categoryData = productData
                .Select(x => x.Category
                    .ToString()
                    .ToUpper()
                )
                .Distinct()
                .ToList();


            using (var session = this._sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                session.SetBatchSize(100);

                var exists = session.Query<Product>().Any();

                if (!exists)
                {
                    // categories
                    categoryData.ForEach(category => session.Save(new ProductCategory(category, category)));

                    var lookup = new
                    {
                        UnitOfMeasures = session.Query<UnitOfMeasure>().ToDictionary(x => x.Name, x => x),
                        Pricings = session.Query<Pricing>().ToDictionary(x => x.Id, x => x),
                    };

                    var unitOfMeasure = new
                    {
                        Individual = session.Load<UnitOfMeasure>(UnitOfMeasure.Piece.Id),
                        Packaging = session.Load<UnitOfMeasure>(UnitOfMeasure.Case.Id)
                    };

                    var defaults = new
                    {
                        Currency = session.Load<Currency>(Currency.PHP.Id),
                        Supplier = session.Query<Supplier>().FirstOrDefault()
                        // TODO: Think of better way to included the supplier from the product.
                        //       One option is to name the product seeding source with the supplier id (default_products-SMIS.xlsx)
                    };

                    // products
                    productData.ForEach(x =>
                    {
                        var product = new Product();
                        product.Accept(new ProductUpdateVisitor()
                        {
                            Code = x.Code,
                            Name = x.Name,
                            Supplier = defaults.Supplier,
                            Category = session.Load<ProductCategory>(x.Category),
                            UnitOfMeasures = new List<ProductUnitOfMeasure>()
                            {
                                // Piece
                                new ProductUnitOfMeasure(
                                    size: x.Piece.Size,
                                    isDefault: false,
                                    isStandard: true,
                                    standardEquivalentValue: x.Piece.StandardEquivalentValue,
                                    unitOfMeasure: lookup.UnitOfMeasures.GetValueSafely(x.Piece.UnitOfMeasure),
                                    prices: new List<ProductUnitOfMeasurePrice>()
                                    {
                                        new ProductUnitOfMeasurePrice(
                                            pricing: lookup.Pricings[Pricing.BasePrice.Id],
                                            price: new Money(
                                                currency: defaults.Currency,
                                                amount:  x.Piece.Price.WholesalePrice
                                            )
                                        ),
                                        new ProductUnitOfMeasurePrice(
                                            pricing: lookup.Pricings[Pricing.WholesalePrice.Id],
                                            price: new Money(
                                                currency: defaults.Currency,
                                                amount:  x.Piece.Price.WholesalePrice
                                            )
                                        ),
                                        new ProductUnitOfMeasurePrice(
                                            pricing: lookup.Pricings[Pricing.RetailPrice.Id],
                                            price: new Money(
                                                currency: defaults.Currency,
                                                amount:  x.Piece.Price.RetailPrice
                                            )
                                        ),
                                        new ProductUnitOfMeasurePrice(
                                            pricing: lookup.Pricings[Pricing.SuggestedRetailPrice.Id],
                                            price: new Money(
                                                currency: defaults.Currency,
                                                amount:  x.Piece.Price.SuggestedRetailPrice
                                            )
                                        ),
                                        new ProductUnitOfMeasurePrice(
                                            pricing: lookup.Pricings[Pricing.BadStockPrice.Id],
                                            price: new Money(
                                                currency: defaults.Currency,
                                                amount:  0M
                                            )
                                        ),
                                    }
                                ),
                                // Package
                                new ProductUnitOfMeasure(
                                    size: x.Package.Size,
                                    isDefault: true,
                                    isStandard: false,
                                    standardEquivalentValue: x.Package.StandardEquivalentValue,
                                    unitOfMeasure: lookup.UnitOfMeasures.GetValueSafely(x.Package.UnitOfMeasure),
                                    prices: new List<ProductUnitOfMeasurePrice>()
                                    {
                                        new ProductUnitOfMeasurePrice(
                                            pricing: lookup.Pricings[Pricing.BasePrice.Id],
                                            price: new Money(
                                                currency: defaults.Currency,
                                                amount:  x.Package.Price.WholesalePrice
                                            )
                                        ),
                                        new ProductUnitOfMeasurePrice(
                                            pricing: lookup.Pricings[Pricing.WholesalePrice.Id],
                                            price: new Money(
                                                currency: defaults.Currency,
                                                amount:  x.Package.Price.WholesalePrice
                                            )
                                        ),
                                        new ProductUnitOfMeasurePrice(
                                            pricing: lookup.Pricings[Pricing.RetailPrice.Id],
                                            price: new Money(
                                                currency: defaults.Currency,
                                                amount:  x.Package.Price.RetailPrice
                                            )
                                        ),
                                        new ProductUnitOfMeasurePrice(
                                            pricing: lookup.Pricings[Pricing.SuggestedRetailPrice.Id],
                                            price: new Money(
                                                currency: defaults.Currency,
                                                amount:  x.Package.Price.SuggestedRetailPrice
                                            )
                                        ),
                                        new ProductUnitOfMeasurePrice(
                                            pricing: lookup.Pricings[Pricing.BadStockPrice.Id],
                                            price: new Money(
                                                currency: defaults.Currency,
                                                amount:  0M
                                            )
                                        ),
                                    }
                                ),
                            }
                            .Where(o => o.UnitOfMeasure != null)
                        });

                        var standard = product.UnitOfMeasures.FirstOrDefault(o => o.IsStandard);

                        product.Inventory.Accept(new InventoryUpdateVisitor()
                        {
                            // inventory settings
                            // TODO: get this from excel
                            TargetLevel = new Measure(_utils.RandomInteger(100, 200), standard.UnitOfMeasure),
                            ReorderLevel = new Measure(_utils.RandomInteger(50, 75), standard.UnitOfMeasure),
                            MinimumReorderQuantity = new Measure(_utils.RandomInteger(100, 150), standard.UnitOfMeasure),

                            IndividualBarcode = x.Piece.Barcode,
                            PackagingBarcode = x.Package.Barcode,
                            PackagingSize = x.Package.StandardEquivalentValue,
                            UnitOfMeasure = unitOfMeasure.Individual,
                            PackagingUnitOfMeasure = unitOfMeasure.Packaging,

                            // TODO: check for correct values
                            //BasePrice = new Money(amount: 0M, currency: defaults.Currency),
                            //BadStockPrice = new Money(amount: 0M, currency: defaults.Currency),
                            BasePrice = new Money(amount: x.Piece.Price.WholesalePrice, currency: defaults.Currency),
                            BadStockPrice = new Money(amount: x.Piece.Price.WholesalePrice, currency: defaults.Currency),
                            WholesalePrice = new Money(amount: x.Piece.Price.WholesalePrice, currency: defaults.Currency),
                            RetailPrice = new Money(amount: x.Piece.Price.RetailPrice, currency: defaults.Currency),
                        });

                        product.EnsureValidity();

                        session.Save(product);
                    });
                }

                transaction.Commit();
            }
        }
    }
}

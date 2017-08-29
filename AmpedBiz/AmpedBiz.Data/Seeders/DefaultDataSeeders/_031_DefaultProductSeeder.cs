using AmpedBiz.Common.Configurations;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Inventories;
using AmpedBiz.Core.Services.Products;
using AmpedBiz.Data.Context;
using LinqToExcel;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DefaultDataSeeders
{
    public class _031_DefaultProductSeeder : IDefaultDataSeeder
    {
        private readonly IContext _context;
        private readonly ISessionFactory _sessionFactory;

        public _031_DefaultProductSeeder(DefaultContext context, ISessionFactory sessionFactory)
        {
            this._context = context;
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
                    InitialLevel = Convert.ToDecimal(x["Initial Level"].Cast<double>()),
                    TargetLevel = Convert.ToDecimal(x["Target Level"].Cast<double>()),
                    ReorderLevel = Convert.ToDecimal(x["Reorder Level"].Cast<double>()),
                    MinimumReorderQuantity = Convert.ToDecimal(x["Minimum Reorder Quantity"].Cast<double>()),
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


            using (var session = this._sessionFactory.RetrieveSharedSession(_context))
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
                                    barcode: x.Piece.Barcode,
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
                                    barcode: x.Package.Barcode,
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
                            .Where(o => 
                                (o.UnitOfMeasure != null) ||
                                (o.IsStandard != true && o.StandardEquivalentValue == 1)
                            )
                        });

                        var standard = product.UnitOfMeasures.FirstOrDefault(o => o.IsStandard);

                        product.Inventory.Accept(new InventoryUpdateVisitor()
                        {
                            InitialLevel = new Measure(x.InitialLevel, standard.UnitOfMeasure),
                            TargetLevel = new Measure(x.TargetLevel, standard.UnitOfMeasure),
                            ReorderLevel = new Measure(x.ReorderLevel, standard.UnitOfMeasure),
                            MinimumReorderQuantity = new Measure(x.MinimumReorderQuantity, standard.UnitOfMeasure),
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

﻿using AmpedBiz.Common.Configurations;
using AmpedBiz.Common.Extentions;
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
    public class _021_DefaultProductSeeder : IDefaultDataSeeder
    {
        private readonly IContextProvider _contextProvider;
        private readonly ISessionFactory _sessionFactory;

        public _021_DefaultProductSeeder(IContextProvider contextProvider, ISessionFactory sessionFactory)
        {
            this._contextProvider = contextProvider;
            this._sessionFactory = sessionFactory;
        }

        public bool IsSourceExternalFile => true;

        public void Seed()
        {
            var context = this._contextProvider.Build();

            var filename = Path.Combine(DatabaseConfig.Instance.Seeder.ExternalFilesAbsolutePath, context.TenantId, @"default_products.xlsx");

            if (!File.Exists(filename))
                return;

            var endingInventoryData = DefaultEndingInventory.Read(context);

            var productData = new ExcelQueryFactory(filename).Worksheet()
                .Select(x => new
                {                                                
                    Code = x["Product Id"].ToString(),
                    Name = x["Product Name"].ToString(),
                    Category = x["Category"].ToString().ToUpper(),
                    InitialLevel = Convert.ToDecimal(x["Initial Level"].Cast<double>()),
                    TargetLevel = Convert.ToDecimal(x["Target Level"].Cast<double>()),
                    ReorderLevel = Convert.ToDecimal(x["Reorder Level"].Cast<double>()),
                    MinimumReorderQuantity = Convert.ToDecimal(x["Minimum Reorder Quantity"].Cast<double>()),
                    Individual = new
                    {
                        Size = x["Size"].ToString(),
                        StandardEquivalentValue = 1M,
                        Barcode = x["Individual Barcode"].ToString(),
                        UnitOfMeasure = endingInventoryData.SelectUom(
                            x["Product Name"].ToString(),
                            x["Piece UOM"].ToString(),
                            z => z.IndividualQuantity.Key
                        ),
                        Price = new
                        {
                            WholesalePrice = Convert.ToDecimal(x["Wholesale Price Per Piece"].Cast<double>()),
                            RetailPrice = Convert.ToDecimal(x["Retail Price Per Piece"].Cast<double>()),
                            SuggestedRetailPrice = Convert.ToDecimal(x["Suggested Retail Price"].Cast<double>()),
                        },
                        InitialQuantityValue = endingInventoryData.SelectInitialQuantityValue(
                            x["Product Name"].ToString(),
                            z => z.IndividualQuantity.Value
                        ),
                    },
                    Package = new
                    {
                        Size = string.Empty,
                        StandardEquivalentValue = Convert.ToDecimal(x["Piece Per Package"].Cast<double>()),
                        Barcode = x["Packaging Barcode"].ToString(),
                        UnitOfMeasure = endingInventoryData.SelectUom(
                            x["Product Name"].ToString(),
                            x["Package UOM"].ToString(),
                            z => z.PackageQuantity.Key
                        ),
                        Price = new
                        {
                            WholesalePrice = Convert.ToDecimal(x["Wholesale Price Per Package"].Cast<double>()),
                            RetailPrice = Convert.ToDecimal(x["Retail Price Per Package"].Cast<double>()),
                            SuggestedRetailPrice = Convert.ToDecimal(x["Retail Price Per Package"].Cast<double>()),
                        },
                        InitialQuantityValue = endingInventoryData.SelectInitialQuantityValue(
                            x["Product Name"].ToString(),
                            z => z.PackageQuantity.Value
                        ),

                    },
                })
                .ToList()
                .Where(x => !string.IsNullOrWhiteSpace(x.Name));

            var categoryData = productData
                .Where(x => !string.IsNullOrWhiteSpace(x.Category))
                .Select(x => x.Category
                    .ToString()
                    .ToUpper()
                )
                .Distinct()
                .ToList();

            using (var session = this._sessionFactory.RetrieveSharedSession(context))
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
                        UnitOfMeasures = session.Query<UnitOfMeasure>().Cacheable().ToList().ToDictionary(x => x.Name, x => x),
                        Pricings = session.Query<Pricing>().Cacheable().ToList().ToDictionary(x => x.Id, x => x),
                    };

                    var currencySettings = session.Query<Setting<CurrencySetting>>().Cacheable().First();

                    var defaults = new
                    {
                        Currency = session.Load<Currency>(currencySettings.Value.DefaultCurrencyId),
                        Supplier = session.Query<Supplier>().FirstOrDefault(),
                        Branch = session.Load<Branch>(context.BranchId),
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
                                // Individual
                                new ProductUnitOfMeasure(
                                    size: x.Individual.Size,
                                    barcode: x.Individual.Barcode,
                                    isDefault: string.IsNullOrWhiteSpace(x.Package.UnitOfMeasure), //false,
                                    isStandard: true,
                                    standardEquivalentValue: x.Individual.StandardEquivalentValue,
                                    unitOfMeasure: lookup.UnitOfMeasures.GetValueOrDefault(x.Individual.UnitOfMeasure),
                                    prices: new List<ProductUnitOfMeasurePrice>()
                                    {
                                        new ProductUnitOfMeasurePrice(
                                            //branch: defaults.Branch,
                                            pricing: lookup.Pricings[Pricing.BasePrice.Id],
                                            price: new Money(
                                                currency: defaults.Currency,
                                                amount:  x.Individual.Price.WholesalePrice
                                            )
                                        ),
                                        new ProductUnitOfMeasurePrice(
                                            //branch: defaults.Branch,
                                            pricing: lookup.Pricings[Pricing.WholesalePrice.Id],
                                            price: new Money(
                                                currency: defaults.Currency,
                                                amount:  x.Individual.Price.WholesalePrice
                                            )
                                        ),
                                        new ProductUnitOfMeasurePrice(
                                            //branch: defaults.Branch,
                                            pricing: lookup.Pricings[Pricing.RetailPrice.Id],
                                            price: new Money(
                                                currency: defaults.Currency,
                                                amount:  x.Individual.Price.RetailPrice
                                            )
                                        ),
                                        new ProductUnitOfMeasurePrice(
                                            //branch: defaults.Branch,
                                            pricing: lookup.Pricings[Pricing.SuggestedRetailPrice.Id],
                                            price: new Money(
                                                currency: defaults.Currency,
                                                amount:  x.Individual.Price.SuggestedRetailPrice
                                            )
                                        ),
                                        new ProductUnitOfMeasurePrice(
                                            //branch: defaults.Branch,
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
                                    unitOfMeasure: lookup.UnitOfMeasures.GetValueOrDefault(x.Package.UnitOfMeasure),
                                    prices: new List<ProductUnitOfMeasurePrice>()
                                    {
                                        new ProductUnitOfMeasurePrice(
                                            //branch: defaults.Branch,
                                            pricing: lookup.Pricings[Pricing.BasePrice.Id],
                                            price: new Money(
                                                currency: defaults.Currency,
                                                amount:  x.Package.Price.WholesalePrice
                                            )
                                        ),
                                        new ProductUnitOfMeasurePrice(
                                            //branch: defaults.Branch,
                                            pricing: lookup.Pricings[Pricing.WholesalePrice.Id],
                                            price: new Money(
                                                currency: defaults.Currency,
                                                amount:  x.Package.Price.WholesalePrice
                                            )
                                        ),
                                        new ProductUnitOfMeasurePrice(
                                            //branch: defaults.Branch,
                                            pricing: lookup.Pricings[Pricing.RetailPrice.Id],
                                            price: new Money(
                                                currency: defaults.Currency,
                                                amount:  x.Package.Price.RetailPrice
                                            )
                                        ),
                                        new ProductUnitOfMeasurePrice(
                                            //branch: defaults.Branch,
                                            pricing: lookup.Pricings[Pricing.SuggestedRetailPrice.Id],
                                            price: new Money(
                                                currency: defaults.Currency,
                                                amount:  x.Package.Price.SuggestedRetailPrice
                                            )
                                        ),
                                        new ProductUnitOfMeasurePrice(
                                            //branch: defaults.Branch,
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
                                (o.UnitOfMeasure != null) &&
                                (o.IsStandard
                                    ? o.StandardEquivalentValue == 1
                                    : o.StandardEquivalentValue > 1
                                )
                            )
                            .ToList()
                        });

                        var standard = product.UnitOfMeasures.FirstOrDefault(o => o.IsStandard);

                        var initialLevel = new Func<Measure>(() =>
                        {
                            var measure = default(Measure);

                            var initialInventory = endingInventoryData.FirstOrDefault(o => o.Name == product.Name);
                            if (initialInventory != null)
                            {
                                if (initialInventory.HasPackage)
                                {
                                    var packageQuantity = product.UnitOfMeasures.Default(o => new Measure(initialInventory.PackageQuantity.Value, o.UnitOfMeasure));
                                    var packageStandardQuantity = product.Convert(packageQuantity, standard.UnitOfMeasure);

                                    var individualQuantity = product.UnitOfMeasures.Standard(o => new Measure(initialInventory.IndividualQuantity.Value, o.UnitOfMeasure));
                                    var individualStandardQuantity = product.Convert(individualQuantity, standard.UnitOfMeasure);

                                    measure = packageStandardQuantity + individualStandardQuantity;
                                }
                                else
                                {
                                    var individualQuantity = product.UnitOfMeasures.Standard(o => new Measure(initialInventory.IndividualQuantity.Value, o.UnitOfMeasure));
                                    var individualStandardQuantity = product.Convert(individualQuantity, standard.UnitOfMeasure);

                                    measure = individualStandardQuantity;
                                }
                            }
                            else
                            {
                                measure = new Measure(x.InitialLevel, standard.UnitOfMeasure);
                            }

                            return measure;
                        })();

                        var @default = product.UnitOfMeasures.FirstOrDefault(o => o.IsDefault);
                        var branch = session.Load<Branch>(context.BranchId);
                        var inventory = new Inventory(branch, product);

                        inventory.Accept(new InventoryUpdateVisitor()
                        {
                            Branch = branch,
                            Product = product,
                            InitialLevel = initialLevel,
                            TargetLevel = new Measure(x.TargetLevel, @default.UnitOfMeasure),
                            ReorderLevel = new Measure(x.ReorderLevel, @default.UnitOfMeasure),
                            MinimumReorderQuantity = new Measure(x.MinimumReorderQuantity, @default.UnitOfMeasure),
                        });

                        product.EnsureValidity();
                        inventory.EnsureValidity();

                        session.Save(inventory);
                        session.Save(product);
                    });
                }

                transaction.Commit();
                _sessionFactory.ReleaseSharedSession();
            }
        }
    }

    internal class DefaultEndingInventory
    {
        public string Code { get; private set; }

        public string Name { get; private set; }

        public KeyValuePair<string, decimal> IndividualQuantity { get; private set; }

        public KeyValuePair<string, decimal> PackageQuantity { get; private set; }

        public bool HasPackage => !this.PackageQuantity.Key.IsNullOrDefault();

        private DefaultEndingInventory(string code, string name, string storeQuantity)
        {
            this.Code = code;

            this.Name = name;

            if (!string.IsNullOrWhiteSpace(storeQuantity))
            {
                var items = storeQuantity.Trim().Split('&');
                if (items.Any())
                {
                    var hasPackage = (items.Count() > 1);
                    var rawIndividualQuantity = items.Last();
                    var rawPackageQuantity = hasPackage ? items.First() : string.Empty;

                    this.IndividualQuantity = this.ParseQuantity(rawIndividualQuantity);
                    this.PackageQuantity = this.ParseQuantity(rawPackageQuantity);
                }
            }
        }

        private KeyValuePair<string, decimal> ParseQuantity(string raw)
        {
            var parsedQuantity = default(KeyValuePair<string, decimal>);

            if (!string.IsNullOrWhiteSpace(raw))
            {
                var parts = raw.Trim().Split(' ');
                var umo = parts.Last();
                var quantity = decimal.Parse(parts.First());
                parsedQuantity = new KeyValuePair<string, decimal>(umo, quantity);
            }

            return parsedQuantity;
        }

        public static IReadOnlyList<DefaultEndingInventory> Read(IContext context)
        {
            var filename = Path.Combine(DatabaseConfig.Instance.Seeder.ExternalFilesAbsolutePath, context.TenantId, @"default_ending_inventory.xlsx");

            if (!File.Exists(filename))
                return new List<DefaultEndingInventory>();

            return new ExcelQueryFactory(filename).Worksheet()
                .Select(x => new
                {
                    Code = x["PRODUCT CODE"].ToString(),
                    Name = x["DESCRIPTION"].Cast<string>(),
                    QuantityStore = x["QTY STORE"].Cast<string>()
                })
                .ToList()
                .Where(x =>
                    (
                        !string.IsNullOrWhiteSpace(x.Code) ||
                        !string.IsNullOrWhiteSpace(x.Name)
                    )
                    &&
                    !string.IsNullOrWhiteSpace(x.QuantityStore)
                )
                .Select(x => new DefaultEndingInventory(
                    code: null, //x.Code,
                    name: x.Name,
                    storeQuantity: x.QuantityStore
                ))
                .ToList()
                .AsReadOnly();
        }
    }

    internal static class Extentions
    {
        public static string SelectUom(this IEnumerable<DefaultEndingInventory> endingInventories, string productName, string initialUom, Func<DefaultEndingInventory, string> selector)
        {
            if (initialUom == null)
                Console.WriteLine("null");

            if (string.IsNullOrWhiteSpace(productName))
                return initialUom;

            var endingInventory = endingInventories.FirstOrDefault(x => string.Compare(strA: x.Name, strB: productName, ignoreCase: true) == 0);
            if (endingInventory == null)
                return initialUom;

            return selector(endingInventory) ?? initialUom;
        }

        public static decimal SelectInitialQuantityValue(this IEnumerable<DefaultEndingInventory> endingInventories, string productName, Func<DefaultEndingInventory, decimal> selector)
        {
            var endingInventory = endingInventories.FirstOrDefault(x => string.Compare(strA: x.Name, strB: productName, ignoreCase: true) == 0);
            if (endingInventory == null)
                return default(decimal);

            return selector(endingInventory);
        }
    }
}

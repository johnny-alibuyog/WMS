using AmpedBiz.Common.Configurations;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Inventories;
using AmpedBiz.Core.Services.Products;
using AmpedBiz.Data.Context;
using Humanizer;
using LinqToExcel;
using LinqToExcel.Query;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AmpedBiz.Data.Seeders.DefaultDataSeeders
{
    public class _021_1_ProductSeeder : IDefaultDataSeeder
    {
        private readonly IContextProvider _contextProvider;
        private readonly ISessionFactory _sessionFactory;

        public _021_1_ProductSeeder(IContextProvider contextProvider, ISessionFactory sessionFactory)
        {
            this._contextProvider = contextProvider;
            this._sessionFactory = sessionFactory;
        }

        public bool IsSourceExternalFile => true;

        public void Seed()
        {
            var context = this._contextProvider.Build();

            var filename = Path.Combine(DatabaseConfig.Instance.Seeder.ExternalFilesAbsolutePath, context.TenantId, @"products.xlsx");

            if (!File.Exists(filename))
                return;

            var rawProducts = new ExcelQueryFactory(filename).Worksheet().ExtractRawProducts();

            using (var session = this._sessionFactory.RetrieveSharedSession(context))
            using (var transaction = session.BeginTransaction())
            {
                session.SetBatchSize(100);

                var exists = session.Query<Product>().Any();

                if (!exists)
                {
                    var lookup = new
                    {
                        Pricings = session.Query<Pricing>().Cacheable().ToList().ToDictionary(x => x.Id, x => x),
                        Categories = session.Query<ProductCategory>().Cacheable().ToList().ToDictionary(x => x.Id, x => x),
                        UnitOfMeasures = session.Query<UnitOfMeasure>().Cacheable().ToList().ToDictionary(x => x.Name, x => x)
                    };

                    var currencySettings = session.Query<Setting<CurrencySetting>>().Cacheable().First();

                    // TODO: Think of better way to included the supplier from the product.
                    //       One option is to name the product seeding source with the supplier id (default_products-SMIS.xlsx)
                    // NOTE: (2018-08-19) Products should have multiple suppliers. 
                    //       Options:
                    //       1. Product.Supplier relation should be removed
                    //       2. create many-to-many relationship between Products and Suppliers
                    var defaults = new
                    {
                        Currency = session.Load<Currency>(currencySettings.Value.DefaultCurrencyId),
                        Supplier = session.Query<Supplier>().FirstOrDefault(),
                        Branch = session.Load<Branch>(context.BranchId),
                    };

                    // products
                    rawProducts.ForEach(rawProduct =>
                    {
                        var product = rawProduct.ExtractProduct(
                            branch: defaults.Branch,
                            supplier: defaults.Supplier,
                            currency: defaults.Currency,
                            pricingLookup: lookup.Pricings,
                            categoryLookup: lookup.Categories,
                            unitOfMeasureLookup: lookup.UnitOfMeasures
                        );
                        var inventory = new Inventory(defaults.Branch, product);

                        inventory.Accept(new InventoryUpdateVisitor()
                        {
                            Branch = defaults.Branch,
                            Product = product,
                            InitialLevel = rawProduct.ExtractInventoryInitialLevel(product),
                            TargetLevel = rawProduct.ExtractInventoryTargetLevel(product),
                            ReorderLevel = rawProduct.ExtractInventoryReorderLevel(product),
                            MinimumReorderQuantity = rawProduct.ExtractInventoryMinimumReorderQuantity(product),
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

    internal class ProductImportModel
    {
        public string ProductName { get; private set; }

        public string PieceUOM { get; private set; }

        public string PackageUOM { get; private set; }

        public double InitialLevel { get; private set; }

        public double TargetLevel { get; private set; }

        public double ReorderLevel { get; private set; }

        public double MinimumReorderQuantity { get; private set; }

        public double PiecePerPackage { get; private set; }

        public string Size { get; private set; }

        public double CostPricePerPiece { get; private set; }

        public double CostPricePerPackage { get; private set; }

        public double WholesalePricePerPiece { get; private set; }

        public double WholesalePricePerPackage { get; private set; }

        public double SuggestedRetailPrice { get; private set; }

        public string IndividualBarcode { get; private set; }

        public string PackagingBarcode { get; private set; }

        public string Category { get; private set; }

        private ProductImportModel(Row row)
        {
            this.MapValueOf(row, to => to.ProductName);
            this.MapValueOf(row, to => to.PieceUOM);
            this.MapValueOf(row, to => to.PackageUOM);
            this.MapValueOf(row, to => to.InitialLevel);
            this.MapValueOf(row, to => to.TargetLevel);
            this.MapValueOf(row, to => to.ReorderLevel);
            this.MapValueOf(row, to => to.MinimumReorderQuantity);
            this.MapValueOf(row, to => to.PiecePerPackage);
            this.MapValueOf(row, to => to.Size);
            this.MapValueOf(row, to => to.CostPricePerPiece);
            this.MapValueOf(row, to => to.CostPricePerPackage);
            this.MapValueOf(row, to => to.WholesalePricePerPiece);
            this.MapValueOf(row, to => to.WholesalePricePerPackage);
            this.MapValueOf(row, to => to.SuggestedRetailPrice);
            this.MapValueOf(row, to => to.IndividualBarcode);
            this.MapValueOf(row, to => to.PackagingBarcode);
            this.MapValueOf(row, to => to.Category);
        }

        public static ProductImportModel Map(Row row)
        {
            var item = new ProductImportModel(row);

            if (string.IsNullOrWhiteSpace(item.ProductName))
                return null;

            return item;
        }
    }

    internal static class ProductImportModelExtention
    {
        public static void MapValueOf<TSource, TProperty>(this TSource target, Row row, Expression<Func<TSource, TProperty>> selector)
        {
            var memberExpression = selector.Body as MemberExpression;
            var property = memberExpression.Member as PropertyInfo;
            var columnName = property.Name.Humanize(LetterCasing.Title);
            var value = row[columnName].Cast<TProperty>();
            property.SetValue(target, value, null);
        }

        public static IReadOnlyCollection<ProductCategory> ExtractProductCategories(this IEnumerable<ProductImportModel> items)
        {
            return items
                .Where(x => !string.IsNullOrWhiteSpace(x.Category))
                .Select(x => new ProductCategory(
                    id: x.Category.Titleize(), 
                    name: x.Category.Titleize()
                ))
                .Distinct()
                .ToList()
                .AsReadOnly();
        }

        public static IReadOnlyCollection<ProductImportModel> ExtractRawProducts(this ExcelQueryable<Row> rows)
        {
            return rows
                .Select(ProductImportModel.Map)
                .Where(x => !x.IsNullOrDefault())
                .ToList()
                .AsReadOnly();
        }

        public static Product ExtractProduct(
            this ProductImportModel x,
            Branch branch,
            Supplier supplier,
            Currency currency,
            IDictionary<string, Pricing> pricingLookup,
            IDictionary<string, ProductCategory> categoryLookup,
            IDictionary<string, UnitOfMeasure> unitOfMeasureLookup
        )
        {
            var product = new Product();
            product.Accept(new ProductUpdateVisitor()
            {
                Code = x.ProductName,
                Name = x.ProductName,
                Supplier = supplier,
                Category = categoryLookup
                    .GetValueOrDefault(x.Category.Titleize())
                    .EnsureExistence($"{nameof(x.Category)} {x.Category.Titleize()} for {x.ProductName} does not exists in database."),
                UnitOfMeasures = new List<ProductUnitOfMeasure>()
                    .AddIfHasValue(x.ExtractIndividualProductUnitOfMeasure(
                        branch: branch,
                        supplier: supplier,
                        currency: currency,
                        pricingLookup: pricingLookup,
                        unitOfMeasureLookup: unitOfMeasureLookup
                    ))
                    .AddIfHasValue(x.ExtractPackageProductUnitOfMeasure(
                        branch: branch,
                        supplier: supplier,
                        currency: currency,
                        pricingLookup: pricingLookup,
                        unitOfMeasureLookup: unitOfMeasureLookup
                    ))
            });
            return product;
        }

        public static IReadOnlyCollection<Product> ExtractProducts(
            this IEnumerable<ProductImportModel> rawProducts,
            Branch branch,
            Supplier supplier,
            Currency currency,
            IDictionary<string, Pricing> pricingLookup,
            IDictionary<string, ProductCategory> categoryLookup,
            IDictionary<string, UnitOfMeasure> unitOfMeasureLookup
        )
        {
            return rawProducts
                .Select(x =>
                    x.ExtractProduct(
                        branch: branch,
                        supplier: supplier,
                        currency: currency,
                        pricingLookup: pricingLookup,
                        categoryLookup: categoryLookup,
                        unitOfMeasureLookup: unitOfMeasureLookup
                    )
                )
                .ToList()
                .AsReadOnly();
        }

        public static ProductUnitOfMeasure ExtractIndividualProductUnitOfMeasure(
            this ProductImportModel instance,
            Branch branch,
            Supplier supplier,
            Currency currency,
            IDictionary<string, Pricing> pricingLookup,
            IDictionary<string, UnitOfMeasure> unitOfMeasureLookup
        )
        {
            return new ProductUnitOfMeasure(
                size: instance.Size,
                barcode: instance.IndividualBarcode,
                isDefault: string.IsNullOrWhiteSpace(instance.PackageUOM), //false,
                isStandard: true,
                standardEquivalentValue: 1M,
                unitOfMeasure: unitOfMeasureLookup
                    .GetValueOrDefault(instance.PieceUOM)
                    .EnsureExistence($"{nameof(instance.PieceUOM).Humanize(LetterCasing.Title)} for {instance.ProductName} should contain value."),
                prices: new List<ProductUnitOfMeasurePrice>()
                {
                        new ProductUnitOfMeasurePrice(
                            //branch: defaults.Branch,
                            pricing: pricingLookup[Pricing.BasePrice.Id],
                            price: new Money(
                                currency: currency,
                                amount: Convert.ToDecimal(instance.CostPricePerPiece)
                            )
                        ),
                        new ProductUnitOfMeasurePrice(
                            //branch: defaults.Branch,
                            pricing: pricingLookup[Pricing.WholesalePrice.Id],
                            price: new Money(
                                currency: currency,
                                amount:  Convert.ToDecimal(instance.WholesalePricePerPiece)
                            )
                        ),
                        new ProductUnitOfMeasurePrice(
                            //branch: defaults.Branch,
                            pricing: pricingLookup[Pricing.RetailPrice.Id],
                            price: new Money(
                                currency: currency,
                                amount:  Convert.ToDecimal(instance.SuggestedRetailPrice)
                            )
                        ),
                        new ProductUnitOfMeasurePrice(
                            //branch: defaults.Branch,
                            pricing: pricingLookup[Pricing.SuggestedRetailPrice.Id],
                            price: new Money(
                                currency: currency,
                                amount:  0M //Convert.ToDecimal(instance.SuggestedRetailPrice)
                            )
                        ),
                        new ProductUnitOfMeasurePrice(
                            //branch: defaults.Branch,
                            pricing: pricingLookup[Pricing.BadStockPrice.Id],
                            price: new Money(
                                currency: currency,
                                amount:  0M
                            )
                        ),
                }
            );
        }

        public static ProductUnitOfMeasure ExtractPackageProductUnitOfMeasure(
            this ProductImportModel instance,
            Branch branch,
            Supplier supplier,
            Currency currency,
            IDictionary<string, Pricing> pricingLookup,
            IDictionary<string, UnitOfMeasure> unitOfMeasureLookup
        )
        {
            // NOTE: Package UOM is opional. There is a product that is purchased and sold as one unit only.
            if (string.IsNullOrEmpty(instance.PackageUOM))
            {
                return null;
            }

            return new ProductUnitOfMeasure(
                size: string.Empty,
                barcode: instance.PackagingBarcode,
                isDefault: true,
                isStandard: false,
                standardEquivalentValue: Convert.ToDecimal(instance.PiecePerPackage),
                unitOfMeasure: unitOfMeasureLookup.GetValueOrDefault(instance.PackageUOM),
                prices: new List<ProductUnitOfMeasurePrice>()
                {
                        new ProductUnitOfMeasurePrice(
                            //branch: defaults.Branch,
                            pricing: pricingLookup[Pricing.BasePrice.Id],
                            price: new Money(
                                currency: currency,
                                amount:  Convert.ToDecimal(instance.CostPricePerPackage)
                            )
                        ),
                        new ProductUnitOfMeasurePrice(
                            //branch: defaults.Branch,
                            pricing: pricingLookup[Pricing.WholesalePrice.Id],
                            price: new Money(
                                currency: currency,
                                amount:  Convert.ToDecimal(instance.WholesalePricePerPackage)
                            )
                        ),
                        new ProductUnitOfMeasurePrice(
                            //branch: sdefaults.Branch,
                            pricing: pricingLookup[Pricing.RetailPrice.Id],
                            price: new Money(
                                currency: currency,
                                amount:  0M
                            )
                        ),
                        new ProductUnitOfMeasurePrice(
                            //branch: defaults.Branch,
                            pricing: pricingLookup[Pricing.SuggestedRetailPrice.Id],
                            price: new Money(
                                currency: currency,
                                amount:  0M
                            )
                        ),
                        new ProductUnitOfMeasurePrice(
                            //branch: defaults.Branch,
                            pricing: pricingLookup[Pricing.BadStockPrice.Id],
                            price: new Money(
                                currency: currency,
                                amount:  0M
                            )
                        ),
                }
            );
        }

        public static IReadOnlyCollection<UnitOfMeasure> ExtractUnitOfMeasures(this IEnumerable<ProductImportModel> item)
        {
            return item.SelectMany(ExtractUnitOfMeasures).ToList().AsReadOnly();
        }

        public static IReadOnlyCollection<UnitOfMeasure> ExtractUnitOfMeasures(this ProductImportModel item)
        {
            var create = new Func<string, UnitOfMeasure>((value) => 
                !string.IsNullOrWhiteSpace(value) 
                    ? new UnitOfMeasure(value, value)
                    : default(UnitOfMeasure)
            );

            var unitOfMeasures = new List<UnitOfMeasure>()
                .Append(create(item.PieceUOM)
                    .EnsureExistence($"{nameof(item.PieceUOM).Titleize()} for product {item.ProductName} doesn't have value.")
                )
                .AddIfHasValue(create(item.PackageUOM));

            return unitOfMeasures.ToList().AsReadOnly();
        }

        public static Measure ExtractInventoryInitialLevel(this ProductImportModel rawProduct, Product product)
        {
            // NOTE: Uom of "InitialValue" is standard since value on stock could 
            //  be a combination of package plus pieces that should be converted 
            //  to standard uni (piece)

            return new Measure(
                unit: product.UnitOfMeasures.Standard(x => x.UnitOfMeasure), 
                value: Convert.ToDecimal(rawProduct.InitialLevel)
            );
        }

        public static Measure ExtractInventoryTargetLevel(this ProductImportModel rawProduct, Product product)
        {
            return new Measure(
                unit: product.UnitOfMeasures.Default(x => x.UnitOfMeasure),
                value: Convert.ToDecimal(rawProduct.TargetLevel)
            );
        }

        public static Measure ExtractInventoryReorderLevel(this ProductImportModel rawProduct, Product product)
        {
            return new Measure(
                unit: product.UnitOfMeasures.Default(x => x.UnitOfMeasure),
                value: Convert.ToDecimal(rawProduct.ReorderLevel)
            );
        }

        public static Measure ExtractInventoryMinimumReorderQuantity(this ProductImportModel rawProduct, Product product)
        {
            return new Measure(
                unit: product.UnitOfMeasures.Default(x => x.UnitOfMeasure),
                value: Convert.ToDecimal(rawProduct.MinimumReorderQuantity)
            );
        }
    }
}

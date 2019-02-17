using AmpedBiz.Common.Configurations;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Common;
using AmpedBiz.Core.Inventories;
using AmpedBiz.Core.Inventories.Services;
using AmpedBiz.Core.Products;
using AmpedBiz.Core.Products.Services;
using AmpedBiz.Core.Settings;
using AmpedBiz.Data.Context;
using Humanizer;
using LinqToExcel;
using LinqToExcel.Attributes;
using LinqToExcel.Query;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

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

            var rawProducts = context.ExtractRawProducts();

            if (rawProducts != null && !rawProducts.Any())
            {
                return;
            }

            using (var session = this._sessionFactory.RetrieveSharedSession(context))
            using (var transaction = session.BeginTransaction())
            {
                session.SetBatchSize(100);

                var exists = session.Query<Product>().Any();

                if (!exists)
                {
                    var lookup = new
                    {
                        Pricings = session.Query<Pricing>().Cacheable().ToList().ToDictionary(x => x.Id),
                        Suppliers = session.Query<Supplier>().Cacheable().ToList().ToDictionary(x => x.Code),
                        Categories = session.Query<ProductCategory>().Cacheable().ToList().ToDictionary(x => x.Id),
                        UnitOfMeasures = session.Query<UnitOfMeasure>().Cacheable().ToList().ToDictionary(x => x.Name)
                    };

                    var currencySettings = session.Query<Setting<CurrencySetting>>().Cacheable().First();

                    // TODO: Think of better way to included the supplier from the product.
                    //       One option is to name the product seeding source with the supplier id (default_products-SMIS.xlsx)
                    var defaults = new
                    {
                        Currency = session.Load<Currency>(currencySettings.Value.DefaultCurrencyId),
                        Branch = session.Load<Branch>(context.BranchId),
                    };

                    // products
                    rawProducts.ForEach(rawProduct =>
                    {
                        var product = rawProduct.ExtractProduct(
                            branch: defaults.Branch,
                            currency: defaults.Currency,
                            pricingLookup: lookup.Pricings,
                            supplierLookup: lookup.Suppliers,
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

    internal class ProductImportModel : IContainsUnmappedCells
    {
        [ExcelColumn("Product Name")]
        public string ProductName { get; set; }

        [ExcelColumn("Standard UOM (Piece)")]
        public string StandardUOM { get; set; }

        [ExcelColumn("Standard Barcode (Piece)")]
        public string StandardBardcode { get; set; }

        [ExcelColumn("Default UOM (Pack)")]
        public string DefaultUOM { get; set; }

        [ExcelColumn("Default Barcode (Pack)")]
        public string DefaultBarcode { get; set; }

        [ExcelColumn("Piece Per Pack")]
        public double PiecePerPack { get; set; }

        [ExcelColumn("Size")]
        public string Size { get; set; }

        [ExcelColumn("Category")]
        public string Category { get; set; }

        [ExcelColumn("Suppliers")]
        public string Suppliers { get; set; }

        public IDictionary<string, Cell> UnmappedCells { get; } = new Dictionary<string, Cell>();

        public static Func<ProductImportModel, KeyValuePair<string, Cell>, bool> IsCellProductPrice = (product, cell) => 
            new[] { '/', '(', ')' }.All(x => cell.Key.Contains(x)) &&
            (
                cell.Value.Cast<decimal>() > 0 ||
                (
                    cell.Key.Contains("default", StringComparison.InvariantCultureIgnoreCase) &&
                    cell.Key.Contains(Pricing.BasePrice.Name, StringComparison.InvariantCultureIgnoreCase) &&
                    product.DefaultUOM.IsNullOrWhiteSpace() == false &&
                    product.PiecePerPack > 0

                )
            );

        public static Func<string, bool> IsStandard =
            (standardEquivalent) => standardEquivalent.IsEqualTo("standard");

        public static Func<string, ProductImportModel, bool> IsDefault = (standardEquivalent, product) =>
            standardEquivalent.IsEqualTo("default") ||
            (
                IsStandard(standardEquivalent) &&
                product.DefaultUOM.IsNullOrWhiteSpace() &&
                product.PiecePerPack.IsNullOrDefault()
            );
    }

    internal class Segment
    {
        public Pricing Pricing { get; private set; }

        public UnitOfMeasure Unit { get; private set; }

        public decimal StandardEquivalent { get; private set; }

        public bool IsStandard { get; private set; }

        public bool IsDefault { get; private set; }

        public Segment(Pricing pricing, UnitOfMeasure unit, decimal standardEquivalent, bool isStandard, bool isDefault)
        {
            Pricing = pricing;
            Unit = unit;
            StandardEquivalent = standardEquivalent;
            IsStandard = isStandard;
            IsDefault = isDefault;
        }
    }

    internal static class ProductImportModelExtention
    {
        public static IReadOnlyCollection<ProductImportModel> ExtractRawProducts(this IContext context)
        {
            var filename = Path.Combine(DatabaseConfig.Instance.Seeder.ExternalFilesAbsolutePath, context.TenantId, @"products.xlsx");

            if (!File.Exists(filename))
                return Enumerable.Empty<ProductImportModel>().ToList().AsReadOnly();

            var excel = new ExcelQueryFactory()
            {
                FileName = filename,
                ReadOnly = true,
                TrimSpaces = TrimSpacesType.Both,
                StrictMapping = StrictMappingType.None,
            };

            return excel
                .Worksheet<ProductImportModel>()
                .Where(x =>
                    x.ProductName != null &&
                    x.ProductName != string.Empty
                )
                .ToList();
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

        public static IReadOnlyCollection<UnitOfMeasure> ExtractUnitOfMeasures(this IEnumerable<ProductImportModel> items)
        {
            return (
                    items.Select(x => x.StandardUOM)
                )
                .Union(
                    items.Select(x => x.DefaultUOM)
                )
                .Distinct()
                .Where(x => !x.IsNullOrWhiteSpace())
                .Select(x => new UnitOfMeasure(
                    id: x.ToLowerInvariant(),
                    name: x.Titleize()
                ))
                .ToList();
        }

        public static IReadOnlyCollection<Product> ExtractProducts(
            this IEnumerable<ProductImportModel> rawProducts,
            Branch branch,
            Supplier supplier,
            Currency currency,
            IDictionary<string, Pricing> pricingLookup,
            IDictionary<string, Supplier> supplierLookup,
            IDictionary<string, ProductCategory> categoryLookup,
            IDictionary<string, UnitOfMeasure> unitOfMeasureLookup
        )
        {
            return rawProducts
                .Select(Map)
                .ToList()
                .AsReadOnly();

            Product Map(ProductImportModel x) => x.ExtractProduct(
                branch: branch,
                currency: currency,
                pricingLookup: pricingLookup,
                supplierLookup: supplierLookup,
                categoryLookup: categoryLookup,
                unitOfMeasureLookup: unitOfMeasureLookup
            );
        }

        public static Product ExtractProduct(
            this ProductImportModel importModel,
            Branch branch,
            Currency currency,
            IDictionary<string, Pricing> pricingLookup,
            IDictionary<string, Supplier> supplierLookup,
            IDictionary<string, ProductCategory> categoryLookup,
            IDictionary<string, UnitOfMeasure> unitOfMeasureLookup
        )
        {
            var product = new Product();

            product.Accept(new ProductUpdateVisitor()
            {
                Code = importModel.ProductName,
                Name = importModel.ProductName,
                Category = categoryLookup
                    .GetValueOrDefault(
                        importModel.Category
                            .EnsureExistence($"Produc {importModel.Category} requires category.")
                            .Titleize()
                    )
                    .EnsureExistence(
                        $"{nameof(importModel.Category)} {importModel.Category.Titleize()} for " +
                        $"{importModel.ProductName} does not exists in database."
                    ),
                Suppliers = ParseSuppliers(),
                UnitOfMeasures = ParseUnitOfMeasures()
            });

            return product;

            IReadOnlyCollection<Supplier> ParseSuppliers()
            {
                return importModel.Suppliers
                    .Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .Select(x => supplierLookup
                        .GetValueOrDefault(x)
                        .EnsureExistence($"Supplier {x} does not exists in database.")
                    )
                    .ToList();
            }

            IReadOnlyCollection<ProductUnitOfMeasure> ParseUnitOfMeasures()
            {
                return importModel.ExtractProductUnitOfMeasures(
                    branch: branch,
                    currency: currency,
                    pricingLookup: pricingLookup,
                    unitOfMeasureLookup: unitOfMeasureLookup
                );
            }
        }

        private static IReadOnlyCollection<ProductUnitOfMeasure> ExtractProductUnitOfMeasures(
            this ProductImportModel product,
            Branch branch,
            Currency currency,
            IDictionary<string, Pricing> pricingLookup,
            IDictionary<string, UnitOfMeasure> unitOfMeasureLookup)
        {
            var isDefaultBasePriceHasUOM = new Func<Segment, bool>((segment) => 
                segment.IsDefault &&
                segment.Pricing == Pricing.BasePrice &&
                product.DefaultUOM.IsNullOrWhiteSpace() == false &&
                product.PiecePerPack > 0
            );

            var result = product.UnmappedCells
                .Where(cell => 
                    ProductImportModel.IsCellProductPrice(product, cell)
                )
                .Select(x => new
                {
                    Segments = ParseKeySegments(x.Key),
                    Amount = x.Value.Cast<decimal>()
                })
                .GroupBy(x => new
                {
                    Unit = x.Segments.Unit,
                    StandardEquivalent = x.Segments.StandardEquivalent,
                    IsStandard = x.Segments.IsStandard,
                    IsDefault = x.Segments.IsDefault
                })
                .Select(group =>
                    new ProductUnitOfMeasure(
                        size: product.Size,
                        barcode:
                            (group.Key.IsStandard) ? product.StandardBardcode :
                            (group.Key.IsDefault) ? product.DefaultBarcode : (string)null,
                        isStandard: group.Key.IsStandard,
                        isDefault: group.Key.IsDefault,
                        standardEquivalentValue: (decimal)group.Key.StandardEquivalent,
                        unitOfMeasure: group.Key.Unit,
                        prices: pricingLookup
                            .Select(x =>
                                new ProductUnitOfMeasurePrice(
                                    //branck: branch, TODO: this sould be branch specific
                                    pricing: x.Value,
                                    price: new Money(
                                        currency: currency,
                                        amount: group
                                            .Where(o => o.Segments.Pricing == x.Value)
                                            .Select(o => o.Amount)
                                            .FirstOrDefault()
                                    )
                                )
                            )
                            .ToList()
                    )
                )
                .ToList();

            EnsureDefaultPricing();

            return result;

            void EnsureDefaultPricing()
            {
                var productUOM = new
                {
                    Standard = result.Standard(x => x),
                    Default = result.Default(x => x)
                };

                if (productUOM.Standard == productUOM.Default)
                {
                    return;
                }

                var basePrice = new
                {
                    Standard = productUOM.Standard.Prices
                        .FirstOrDefault(x => x.Pricing == Pricing.BasePrice),
                    Default = productUOM.Default.Prices
                        .FirstOrDefault(x => x.Pricing == Pricing.BasePrice),
                };

                if (basePrice.Default.Price.Amount == 0)
                {
                    basePrice.Default.Price.Amount = basePrice.Standard.Price.Amount * productUOM.Default.StandardEquivalentValue;
                }
            }

            Segment ParseKeySegments(string key)
            {
                var parts = key
                    .Split('/', '(')
                    .Select(x => x
                        .Replace(")", string.Empty)
                        .Replace("#", ".")
                        .Trim()
                    )
                    .ToArray();

                return new Segment(
                    pricing: pricingLookup
                        .Where(x =>
                            x.Key.IsEqualTo(parts[0]) ||
                            x.Value.Id.IsEqualTo(parts[0]) ||
                            x.Value.Name.IsEqualTo(parts[0])
                        )
                        .Select(x => x.Value)
                        .FirstOrDefault()
                        .EnsureExistence(
                            $"Product {product.ProductName} has pricing " +
                            $"{parts[0]} that does not exists in database."
                        ),
                    unit: new Func<UnitOfMeasure>(() =>
                    {
                        switch (parts[2].ToLower())
                        {
                            case "standard": return ResolveUnit(parts[2], Select(product, (x) => x.StandardUOM));
                            case "default": return ResolveUnit(parts[2], Select(product, (x) => x.DefaultUOM));
                            default: return ResolveUnit(parts[2], parts[1]);
                        }

                        string Select(ProductImportModel instance, Expression<Func<ProductImportModel, string>> selector) =>
                            selector.Compile().Invoke(instance).Ensure(
                                that: (x) => !x.IsNullOrWhiteSpace(),
                                message: $"Product {product.ProductName} requires to have " +
                                    $"{(((MemberExpression)selector.Body).Member.Name).Titleize()}."
                            );

                        UnitOfMeasure ResolveUnit(string type, string unitId) =>
                            unitOfMeasureLookup
                                .Where(x =>
                                    x.Key.IsEqualTo(unitId) ||
                                    x.Value.Id.IsEqualTo(unitId) ||
                                    x.Value.Name.IsEqualTo(unitId)
                                )
                                .Select(x => x.Value)
                                .FirstOrDefault()
                                .EnsureExistence(
                                    $"Product {product.ProductName} has a {type}" +
                                    $" unit of {unitId} that does not exists in database."
                                );
                    })(),
                    standardEquivalent: new Func<decimal>(() =>
                    {
                        switch (parts[2].ToLower())
                        {
                            case "standard":
                                return 1;

                            case "default":
                                return Convert.ToDecimal(
                                    product.PiecePerPack.Ensure(
                                        that: x => x > 0,
                                        message: $"Product {product.ProductName} should contain " +
                                            $"{nameof(product.PiecePerPack).Titleize()} " +
                                            $"since it has price for Default UOM ({parts[1]})."
                                    )
                                );

                            default:
                                return Convert.ToDecimal(parts[2]);
                        }
                    })(),
                    isStandard: ProductImportModel.IsStandard(parts[2]),
                    isDefault: ProductImportModel.IsDefault(parts[2], product)
                );
            }
        }

        private static Measure ExtractInventoryField(this ProductImportModel rawProduct, string columnName, Product product)
        {
            var column = rawProduct.UnmappedCells
                .FirstOrDefault(x => x.Key.StartsWith(columnName, StringComparison.InvariantCultureIgnoreCase))
                .EnsureExistence($"Worksheet should contain column {columnName}.");

            var segments = ParseColumnNameSegments(column.Key);

            return new Measure(
                unit: ResolveUnit(segments.uomType),
                value: Convert.ToDecimal(column.Value.Cast<double>())
            );

            (string fieldName, string uomType) ParseColumnNameSegments(string columnFullName)
            {
                if (columnFullName.Contains('(') && columnFullName.Contains(')'))
                {
                    var segmentArr = columnFullName
                        .Split(new[] { "(" }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.Replace(")", string.Empty))
                        .ToList();

                    return (fieldName: segmentArr[0], uomType: segmentArr[1]);
                }

                return (fieldName: columnFullName, uomType: null);
            }

            UnitOfMeasure ResolveUnit(string uomType) =>
                (uomType.StartsWith("default", StringComparison.InvariantCultureIgnoreCase))
                    ? product.UnitOfMeasures.Default(x => x.UnitOfMeasure)
                    : product.UnitOfMeasures.Standard(x => x.UnitOfMeasure);
        }

        public static Measure ExtractInventoryInitialLevel(this ProductImportModel rawProduct, Product product)
            => rawProduct.ExtractInventoryField("Initial Level", product);

        public static Measure ExtractInventoryTargetLevel(this ProductImportModel rawProduct, Product product)
            => rawProduct.ExtractInventoryField("Target Level", product);

        public static Measure ExtractInventoryReorderLevel(this ProductImportModel rawProduct, Product product)
            => rawProduct.ExtractInventoryField("Reorder Level", product);

        public static Measure ExtractInventoryMinimumReorderQuantity(this ProductImportModel rawProduct, Product product)
            => rawProduct.ExtractInventoryField("Minimum Reorder Quantity", product);
    }
}

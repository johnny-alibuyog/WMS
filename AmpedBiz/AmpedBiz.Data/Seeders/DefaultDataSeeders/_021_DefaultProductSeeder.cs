using AmpedBiz.Core.Entities;
using LinqToExcel;
using NHibernate;
using NHibernate.Linq;
using System;
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
            var productsFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\Default\default_products.xlsx");
            var supplierFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\Default\default_suppliers.xlsx");

            if (!File.Exists(productsFilename) || !File.Exists(supplierFilename))
                return;

            var supplierData = new ExcelQueryFactory(supplierFilename).Worksheet()
                .Select(x => new
                {
                    Code = x["Supplier Id"],
                    Name = x["Supplier Name"]
                })
                .ToList();

            var productData = new ExcelQueryFactory(productsFilename).Worksheet()
                .Select(x => new
                {
                    Code = x["Product Id"].ToString(),
                    Name = x["Product Name"].ToString(),
                    Size = x["Size"].ToString(),
                    PiecePerPackage = Convert.ToDecimal(x["Piece Per Package"].Cast<double>()),
                    WholesalePerPiece = Convert.ToDecimal(x["Wholesale Price Per Piece"].Cast<double>()),
                    WholesalePerPackage = Convert.ToDecimal(x["Wholesale Price Per Package"].Cast<double>()),
                    RetailPricePerPiece = Convert.ToDecimal(x["Retail Price Per Piece"].Cast<double>()),
                    RetailPricePerPackage = Convert.ToDecimal(x["Retail Price Per Package"].Cast<double>()),
                    SuggestedRetailPrice = Convert.ToDecimal(x["Suggested Retail Price"].Cast<double>()),
                    IndividualBarcode = x["Individual Barcode"].ToString(),
                    PackagingBarcode = x["Packaging Barcode"].ToString(),
                    Category = x["Category"].ToString(),
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
                    // suppliers
                    supplierData.ForEach(supplier => session.Save(new Supplier()
                    {
                        Code = supplier.Code,
                        Name = supplier.Name
                    }));

                    // categories
                    categoryData.ForEach(category => session.Save(new ProductCategory(category, category)));

                    var unitOfMeasure = new
                    {
                        Individual = session.Load<UnitOfMeasure>(UnitOfMeasure.Piece.Id),
                        Packaging = session.Load<UnitOfMeasure>(UnitOfMeasure.Case.Id)
                    };

                    var defaults = new
                    {
                        Currency = session.Load<Currency>(Currency.PHP.Id),
                        Supplier = session.Query<Supplier>().FirstOrDefault(x =>
                            x.Code == supplierData.First().Code ||
                            x.Name == supplierData.First().Name
                        )
                    };

                    // products
                    productData.ForEach(x =>
                    {
                        var product = new Product();
                        product.Code = x.Code;
                        product.Name = x.Name;
                        product.Supplier = defaults.Supplier;
                        product.Category = session.Load<ProductCategory>(x.Category);

                        // inventory settings
                        // TODO: get this from excel
                        product.Inventory.TargetLevel = new Measure(_utils.RandomInteger(100, 200), UnitOfMeasure.Piece);
                        product.Inventory.ReorderLevel = new Measure(_utils.RandomInteger(50, 75), UnitOfMeasure.Piece);
                        product.Inventory.MinimumReorderQuantity = new Measure(_utils.RandomInteger(100, 150), UnitOfMeasure.Piece);

                        product.Inventory.IndividualBarcode = x.IndividualBarcode;
                        product.Inventory.PackagingBarcode = x.PackagingBarcode;
                        product.Inventory.PackagingSize = x.PiecePerPackage;
                        product.Inventory.UnitOfMeasure = unitOfMeasure.Individual;
                        product.Inventory.PackagingUnitOfMeasure = unitOfMeasure.Packaging;

                        // TODO: check for correct values
                        //product.Inventory.BasePrice = new Money(amount: 0M, currency: defaults.Currency);
                        //product.Inventory.BadStockPrice = new Money(amount: 0M, currency: defaults.Currency);
                        product.Inventory.BasePrice = new Money(amount: x.WholesalePerPiece, currency: defaults.Currency);
                        product.Inventory.BadStockPrice = new Money(amount: x.WholesalePerPiece, currency: defaults.Currency);
                        product.Inventory.WholesalePrice = new Money(amount: x.WholesalePerPiece, currency: defaults.Currency);
                        product.Inventory.RetailPrice = new Money(amount: x.RetailPricePerPiece, currency: defaults.Currency);
                        product.EnsureValidity();

                        session.Save(product);
                    });
                }

                transaction.Commit();
            }
        }
    }
}

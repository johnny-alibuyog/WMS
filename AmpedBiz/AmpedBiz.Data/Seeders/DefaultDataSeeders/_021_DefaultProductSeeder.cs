using AmpedBiz.Common.Configurations;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Inventories;
using AmpedBiz.Core.Services.Products;
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
            var filename = Path.Combine(DatabaseConfig.Instance.GetDefaultSeederDataAbsolutePath(), @"default_products.xlsx");

            if (!File.Exists(filename))
                return;
            
            var productData = new ExcelQueryFactory(filename).Worksheet()
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
                            Category = session.Load<ProductCategory>(x.Category)
                        });

                        product.Inventory.Accept(new InventoryUpdateVisitor()
                        {
                            // inventory settings
                            // TODO: get this from excel
                            TargetLevel = new Measure(_utils.RandomInteger(100, 200), UnitOfMeasure.Piece),
                            ReorderLevel = new Measure(_utils.RandomInteger(50, 75), UnitOfMeasure.Piece),
                            MinimumReorderQuantity = new Measure(_utils.RandomInteger(100, 150), UnitOfMeasure.Piece),

                            IndividualBarcode = x.IndividualBarcode,
                            PackagingBarcode = x.PackagingBarcode,
                            PackagingSize = x.PiecePerPackage,
                            UnitOfMeasure = unitOfMeasure.Individual,
                            PackagingUnitOfMeasure = unitOfMeasure.Packaging,

                            // TODO: check for correct values
                            //BasePrice = new Money(amount: 0M, currency: defaults.Currency),
                            //BadStockPrice = new Money(amount: 0M, currency: defaults.Currency),
                            BasePrice = new Money(amount: x.WholesalePerPiece, currency: defaults.Currency),
                            BadStockPrice = new Money(amount: x.WholesalePerPiece, currency: defaults.Currency),
                            WholesalePrice = new Money(amount: x.WholesalePerPiece, currency: defaults.Currency),
                            RetailPrice = new Money(amount: x.RetailPricePerPiece, currency: defaults.Currency),
                        });

                        //product.Inventory.TargetLevel = new Measure(_utils.RandomInteger(100, 200), UnitOfMeasure.Piece);
                        //product.Inventory.ReorderLevel = new Measure(_utils.RandomInteger(50, 75), UnitOfMeasure.Piece);
                        //product.Inventory.MinimumReorderQuantity = new Measure(_utils.RandomInteger(100, 150), UnitOfMeasure.Piece);

                        //product.Inventory.IndividualBarcode = x.IndividualBarcode;
                        //product.Inventory.PackagingBarcode = x.PackagingBarcode;
                        //product.Inventory.PackagingSize = x.PiecePerPackage;
                        //product.Inventory.UnitOfMeasure = unitOfMeasure.Individual;
                        //product.Inventory.PackagingUnitOfMeasure = unitOfMeasure.Packaging;

                        //// TODO: check for correct values
                        ////product.Inventory.BasePrice = new Money(amount: 0M, currency: defaults.Currency);
                        ////product.Inventory.BadStockPrice = new Money(amount: 0M, currency: defaults.Currency);
                        //product.Inventory.BasePrice = new Money(amount: x.WholesalePerPiece, currency: defaults.Currency);
                        //product.Inventory.BadStockPrice = new Money(amount: x.WholesalePerPiece, currency: defaults.Currency);
                        //product.Inventory.WholesalePrice = new Money(amount: x.WholesalePerPiece, currency: defaults.Currency);
                        //product.Inventory.RetailPrice = new Money(amount: x.RetailPricePerPiece, currency: defaults.Currency);
                        //product.EnsureValidity();

                        product.EnsureValidity();

                        session.Save(product);
                    });
                }

                transaction.Commit();
            }
        }
    }
}

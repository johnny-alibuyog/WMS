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
        private readonly ISessionFactory _sessionFactory;

        public _021_DefaultProductSeeder(ISessionFactory sessionFactory)
        {
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
                    PriceToDistributorPerPiece = Convert.ToDecimal(x["Price to Distributor Per Piece"].Cast<double>()),
                    PriceToDistributorPerPackage = Convert.ToDecimal(x["Price to Distributor Per Package"].Cast<double>()),
                    PriceToDownlinePerPiece = Convert.ToDecimal(x["Price to Downline Per Piece"].Cast<double>()),
                    PriceToDownlinePerPackage = Convert.ToDecimal(x["Price to Downline Per Package"].Cast<double>()),
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
                        product.Inventory.IndividualBarcode = x.IndividualBarcode;
                        product.Inventory.PackagingBarcode = x.PackagingBarcode;
                        product.Inventory.PackagingSize = x.PiecePerPackage;
                        product.Inventory.UnitOfMeasure = unitOfMeasure.Individual;
                        product.Inventory.PackagingUnitOfMeasure = unitOfMeasure.Packaging;
                        product.Inventory.BasePrice = new Money(amount: 0M, currency: defaults.Currency);
                        product.Inventory.RetailPrice = new Money(amount: x.PriceToDownlinePerPiece, currency: defaults.Currency);
                        product.Inventory.WholesalePrice = new Money(amount: x.PriceToDistributorPerPiece, currency: defaults.Currency);
                        product.Inventory.BadStockPrice = new Money(amount: 0M, currency: defaults.Currency);

                        session.Save(product);
                    });
                }

                transaction.Commit();
            }
        }
    }
}

using AmpedBiz.Common.Configurations;
using AmpedBiz.Core.Products;
using AmpedBiz.Data.Context;
using LinqToExcel;
using NHibernate;
using NHibernate.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DefaultDataSeeders
{
	public class _014_ProductCategorySeeder : IDefaultDataSeeder
    {
        private readonly IContextProvider _contextProvider;
        private readonly ISessionFactory _sessionFactory;

        public _014_ProductCategorySeeder(IContextProvider contextProvider, ISessionFactory sessionFactory)
        {
            this._contextProvider = contextProvider;
            this._sessionFactory = sessionFactory;
        }

        public bool IsSourceExternalFile => false;

        public void Seed()
        {
            var context = this._contextProvider.Build();

            using (var session = _sessionFactory.RetrieveSharedSession(context))
            using (var transaction = session.BeginTransaction())
            {
                var entities = session.Query<ProductCategory>().Cacheable().ToList();

                var entitiesToImport = this.GetProductCategoryFromProductFile(context) ?? ProductCategory.All;

                foreach (var item in entitiesToImport)
                {
                    if (!entities.Contains(item))
                    {
                        item.EnsureValidity();
                        session.Save(item);
                    }
                }

                transaction.Commit();
                _sessionFactory.ReleaseSharedSession();
            }
        }

        private IReadOnlyCollection<ProductCategory> GetProductCategoryFromProductFile(IContext context)
        {
            var categories = (List<ProductCategory>)null;

            var filename = Path.Combine(DatabaseConfig.Instance.Seeder.ExternalFilesAbsolutePath, context.TenantId, @"products.xlsx");

            if (File.Exists(filename))
            {
                categories = new ExcelQueryFactory(filename)
                    .Worksheet()
                    .ExtractRawProducts()
                    .ExtractProductCategories()
                    .ToList();
            }

            return categories;
        }
    }
}

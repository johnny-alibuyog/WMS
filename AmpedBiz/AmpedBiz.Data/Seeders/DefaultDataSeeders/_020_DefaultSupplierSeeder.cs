using AmpedBiz.Common.Configurations;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data.Context;
using LinqToExcel;
using NHibernate;
using NHibernate.Linq;
using System.IO;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DefaultDataSeeders
{
    public class _020_DefaultSupplierSeeder : IDefaultDataSeeder
    {
        private readonly IContextProvider _contextProvider;
        private readonly ISessionFactory _sessionFactory;

        public _020_DefaultSupplierSeeder(IContextProvider contextProvider, ISessionFactory sessionFactory)
        {
            this._contextProvider = contextProvider;
            this._sessionFactory = sessionFactory;
        }

        public bool IsSourceExternalFile => true;

        public void Seed()
        {
            var context = this._contextProvider.Build();

            var filename = Path.Combine(DatabaseConfig.Instance.Seeder.ExternalFilesAbsolutePath, context.TenantId, @"default_suppliers.xlsx");

            if (!File.Exists(filename))
                return; //throw new FileNotFoundException($"File {filename} not found", filename);

            var excel = new ExcelQueryFactory(filename);
            var data = excel.Worksheet()
                .Select(x => new Supplier()
                {
                    Code = x["Supplier Id"],
                    Name = x["Supplier Name"],
                    ContactPerson = x["Contact Person"],
                })
                .ToList();

            using (var session = this._sessionFactory.RetrieveSharedSession(context))
            using (var transaction = session.BeginTransaction())
            {
                session.SetBatchSize(100);

                var exists = session.Query<Customer>().Any();

                if (!exists)
                {
                    data.ForEach(supplier =>
                    {
                        supplier.EnsureValidity();
                        session.Save(supplier);
                    });
                }

                transaction.Commit();
                _sessionFactory.ReleaseSharedSession();
            }
        }
    }
}

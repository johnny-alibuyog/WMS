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
    public class _030_DefaultSupplierSeeder : IDefaultDataSeeder
    {
        private readonly IContext _context;
        private readonly ISessionFactory _sessionFactory;

        public _030_DefaultSupplierSeeder(DefaultContext context, ISessionFactory sessionFactory)
        {
            this._context = context;
            this._sessionFactory = sessionFactory;
        }

        public void Seed()
        {
            var filename = Path.Combine(DatabaseConfig.Instance.GetDefaultSeederDataAbsolutePath(), "default_suppliers.xlsx");

            if (!File.Exists(filename))
                return; //throw new FileNotFoundException($"File {filename} not found", filename);

                var excel = new ExcelQueryFactory(filename);
            var data = excel.Worksheet()
                .Select(x => new Supplier()
                {
                    Code = x["Supplier Id"],
                    Name = x["Supplier Name"],
                })
                .ToList();

            using (var session = this._sessionFactory.RetrieveSharedSession(_context))
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
            }
        }
    }
}

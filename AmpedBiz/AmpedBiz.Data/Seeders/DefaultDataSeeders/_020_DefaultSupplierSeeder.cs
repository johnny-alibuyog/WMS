using AmpedBiz.Core.Entities;
using LinqToExcel;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Data.Seeders.DefaultDataSeeders
{
    public class _020_DefaultSupplierSeeder : IDefaultDataSeeder
    {
        private readonly ISessionFactory _sessionFactory;

        public _020_DefaultSupplierSeeder(ISessionFactory sessionFactory)
        {
            this._sessionFactory = sessionFactory;
        }

        public void Seed()
        {
            var filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\Default\default_supliers.xlsx");

            if (!File.Exists(filename))
                return;

            var excel = new ExcelQueryFactory(filename);
            var data = excel.Worksheet()
                .Select(x => new Supplier()
                {
                    Code = x["Supplier ID"],
                    Name = x["Supplier Name"],
                })
                .ToList();

            using (var session = this._sessionFactory.OpenSession())
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

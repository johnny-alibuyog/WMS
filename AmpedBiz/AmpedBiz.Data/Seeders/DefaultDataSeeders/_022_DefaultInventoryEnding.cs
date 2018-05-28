using AmpedBiz.Common.Configurations;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data.Context;
using LinqToExcel;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.IO;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DefaultDataSeeders
{
    public class _022_DefaultInventoryEnding : IDefaultDataSeeder
    {
        private readonly IContextProvider _contextProvider;
        private readonly ISessionFactory _sessionFactory;

        public _022_DefaultInventoryEnding(IContextProvider contextProvider, ISessionFactory sessionFactory)
        {
            this._contextProvider = contextProvider;
            this._sessionFactory = sessionFactory;
        }

        public bool IsSourceExternalFile => true;

        public void Seed()
        {
            var context = this._contextProvider.Build();

            var filename = Path.Combine(DatabaseConfig.Instance.Seeder.ExternalFilesAbsolutePath, context.TenantId, @"default_ending_inventory.xlsx");

            if (!File.Exists(filename))
                return;

            var endingInventoryData = new ExcelQueryFactory(filename).Worksheet()
                .Select(x => new
                {
                    Code = x["PRODUCT CODE"].ToString(),
                    Name = x["DESCRIPTION"].ToString(),
                    Quantity = x["QTY STORE"].ToString(),
                })
                .ToList();

            using (var session = this._sessionFactory.RetrieveSharedSession(context))
            using (var transaction = session.BeginTransaction())
            {
                session.SetBatchSize(10);

                var batches = endingInventoryData.Batch(10);

                batches.ForEach(batch =>
                {

                    var inventory = (Inventory)null;
                    var product = (Product)null;

                    var conditions = batch.Aggregate(Restrictions.Disjunction(), (next, item) =>
                    {
                        next.Add(Restrictions.Disjunction()
                            .Add(Restrictions.On(() => product.Code).IsLike(item.Code, MatchMode.Exact))
                            .Add(Restrictions.On(() => product.Name).IsLike(item.Name, MatchMode.Exact))
                        );

                        return next;
                    });

                    var inventories = session
                        .QueryOver(() => inventory)
                        .JoinAlias(() => inventory.Product, () => product)
                        .Where(conditions)
                        .List();

                    Console.WriteLine(inventories);
                });

                transaction.Commit();
                _sessionFactory.ReleaseSharedSession();
            }
        }
    }
}

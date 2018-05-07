using AmpedBiz.Core.Entities;
using AmpedBiz.Data.Context;
using NHibernate;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DummyDataSeeders
{
    public class _001_UnitOfMeasureSeeder : IDummyDataSeeder
    {
        private readonly IContextProvider _contextProvider;
        private readonly ISessionFactory _sessionFactory;

        public _001_UnitOfMeasureSeeder(IContextProvider contextProvider, ISessionFactory sessionFactory)
        {
            this._contextProvider = contextProvider;
            this._sessionFactory = sessionFactory;
        }

        public bool IsSourceExternalFile => false;

        public void Seed()
        {
            var data = new List<UnitOfMeasure>();

            for (int i = 0; i < 5; i++)
            {
                data.Add(new UnitOfMeasure($"UOM{i}", $"Unit Of Measure {i}"));
            }

            var context = this._contextProvider.Build();

            using (var session = _sessionFactory.RetrieveSharedSession(context))
            using (var transaction = session.BeginTransaction())
            {
                //session.SetBatchSize(100);

                var entities = session.Query<UnitOfMeasure>().Cacheable().ToList();
                if (entities.Count == 0)
                {
                    foreach (var item in data)
                    {
                        item.EnsureValidity();
                        session.Save(item);
                    }
                }

                transaction.Commit();
            }
        }
    }
}

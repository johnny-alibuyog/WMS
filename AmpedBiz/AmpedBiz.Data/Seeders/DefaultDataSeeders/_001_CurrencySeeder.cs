using AmpedBiz.Core.Entities;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DefaultDataSeeders
{
    public class _001_CurrencySeeder : IDefaultDataSeeder
    {
        private readonly ISessionFactory _sessionFactory;

        public _001_CurrencySeeder(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void Seed()
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var entity = session.Query<Currency>().Cacheable().ToList();
                if (entity.Count == 0)
                {
                    foreach (var item in Currency.All)
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

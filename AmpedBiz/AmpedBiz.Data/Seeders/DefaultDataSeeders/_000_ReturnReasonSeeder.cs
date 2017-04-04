using AmpedBiz.Core.Entities;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DefaultDataSeeders
{
    public class _000_ReturnReasonSeeder : IDefaultDataSeeder
    {
        private readonly ISessionFactory _sessionFactory;

        public _000_ReturnReasonSeeder(ISessionFactory sessionFactory)
        {
            this._sessionFactory = sessionFactory;
        }

        public void Seed()
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var entity = session.Query<ReturnReason>().Cacheable().ToList();
                if (entity.Count == 0)
                {
                    foreach (var item in ReturnReason.All)
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

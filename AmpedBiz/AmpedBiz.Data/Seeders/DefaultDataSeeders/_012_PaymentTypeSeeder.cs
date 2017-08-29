using AmpedBiz.Core.Entities;
using AmpedBiz.Data.Context;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DefaultDataSeeders
{
    public class _012_PaymentTypeSeeder : IDefaultDataSeeder
    {
        private readonly IContext _context;
        private readonly ISessionFactory _sessionFactory;

        public _012_PaymentTypeSeeder(DefaultContext context, ISessionFactory sessionFactory)
        {
            this._context = context;
            this._sessionFactory = sessionFactory;
        }

        public void Seed()
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var entities = session.Query<PaymentType>().Cacheable().ToList();

                foreach(var item in PaymentType.All)
                {
                    if (!entities.Contains(item))
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

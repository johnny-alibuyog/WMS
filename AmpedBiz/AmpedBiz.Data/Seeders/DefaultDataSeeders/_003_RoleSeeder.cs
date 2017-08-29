using AmpedBiz.Core.Entities;
using AmpedBiz.Data.Context;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DefaultDataSeeders
{
    public class _003_RoleSeeder : IDefaultDataSeeder
    {
        private readonly IContext _context;
        private readonly ISessionFactory _sessionFactory;

        public _003_RoleSeeder(DefaultContext context, ISessionFactory sessionFactory)
        {
            this._context = context;
            this._sessionFactory = sessionFactory;
        }

        public void Seed()
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var entities = session.Query<Role>().Cacheable().ToList();

                foreach (var item in Role.All)
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

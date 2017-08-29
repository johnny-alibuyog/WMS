using AmpedBiz.Core.Entities;
using AmpedBiz.Data.Context;
using NHibernate;

namespace AmpedBiz.Data.Seeders.DefaultDataSeeders
{
    public class _001_DefaultTenantSeeder : IDefaultDataSeeder
    {
        private readonly IContext _context;
        private readonly ISessionFactory _sessionFactory;

        public _001_DefaultTenantSeeder(DefaultContext context, ISessionFactory sessionFactory)
        {
            this._context = context;
            this._sessionFactory = sessionFactory;
        }

        public void Seed()
        {
            using (var session = _sessionFactory.RetrieveSharedSession())
            using (var transaction = session.BeginTransaction())
            {
                var entity = session.Get<Tenant>(Tenant.Default.Id);
                if (entity == null)
                {
                    entity = Tenant.Default;
                    entity.EnsureValidity();
                    session.Save(entity);
                }

                transaction.Commit();
                _sessionFactory.ReleaseSharedSession();
            }
        }
    }
}

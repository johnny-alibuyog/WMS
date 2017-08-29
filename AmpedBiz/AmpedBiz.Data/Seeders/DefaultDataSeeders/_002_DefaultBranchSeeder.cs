using AmpedBiz.Core.Entities;
using AmpedBiz.Data.Context;
using NHibernate;

namespace AmpedBiz.Data.Seeders.DefaultDataSeeders
{
    public class _002_DefaultBranchSeeder : IDefaultDataSeeder
    {
        private readonly IContext _context;
        private readonly ISessionFactory _sessionFactory;

        public _002_DefaultBranchSeeder(DefaultContext context, ISessionFactory sessionFactory)
        {
            this._context = context;
            this._sessionFactory = sessionFactory;
        }

        public void Seed()
        {
            using (var session = _sessionFactory.RetrieveSharedSession(_context))
            using (var transaction = session.BeginTransaction())
            {
                var entity = session.Get<Branch>(Branch.Default.Id);
                if (entity == null)
                {
                    entity = Branch.Default;
                    entity.EnsureValidity();
                    session.Save(entity, entity.Id);
                }

                transaction.Commit();
                _sessionFactory.ReleaseSharedSession();
            }
        }
    }
}

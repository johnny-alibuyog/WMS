using AmpedBiz.Core.Entities;
using NHibernate;

namespace AmpedBiz.Data.Seeders.DefaultDataSeeders
{
    public class _011_DefaultBranchSeeder : IDefaultDataSeeder
    {
        private readonly ISessionFactory _sessionFactory;

        public _011_DefaultBranchSeeder(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void Seed()
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var entity = session.Get<Branch>(Branch.SupperBranch.Id);
                if (entity == null)
                {
                    entity = Branch.SupperBranch;
                    session.Save(entity, entity.Id);
                }

                transaction.Commit();
            }
        }
    }
}

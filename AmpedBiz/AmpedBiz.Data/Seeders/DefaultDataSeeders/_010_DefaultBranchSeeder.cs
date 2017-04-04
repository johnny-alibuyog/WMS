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
                var entity = session.Get<Branch>(Branch.SuperBranch.Id);
                if (entity == null)
                {
                    entity = Branch.SuperBranch;
                    entity.EnsureValidity();
                    session.Save(entity, entity.Id);
                }

                transaction.Commit();
            }
        }
    }
}

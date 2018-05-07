using AmpedBiz.Core.Entities;
using AmpedBiz.Data.Context;
using NHibernate;

namespace AmpedBiz.Data.Seeders.DefaultDataSeeders
{
    public class _002_DefaultBranchSeeder : IDefaultDataSeeder
    {
        private readonly IContextProvider _contextProvider;
        private readonly ISessionFactory _sessionFactory;

        public _002_DefaultBranchSeeder(IContextProvider contextProvider, ISessionFactory sessionFactory)
        {
            this._contextProvider = contextProvider;
            this._sessionFactory = sessionFactory;
        }

        public bool IsSourceExternalFile => false;

        public void Seed()
        {
            var context = this._contextProvider.Build();

            using (var session = _sessionFactory.RetrieveSharedSession(context))
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

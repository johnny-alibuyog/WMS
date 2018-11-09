using AmpedBiz.Core.Returns;
using AmpedBiz.Data.Context;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DefaultDataSeeders
{
	public class _010_ReturnReasonSeeder : IDefaultDataSeeder
    {
        private readonly IContextProvider _contextProvider;
        private readonly ISessionFactory _sessionFactory;

        public _010_ReturnReasonSeeder(IContextProvider contextProvider, ISessionFactory sessionFactory)
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
                _sessionFactory.ReleaseSharedSession();
            }
        }
    }
}

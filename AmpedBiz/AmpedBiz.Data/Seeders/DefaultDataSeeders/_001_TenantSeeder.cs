using AmpedBiz.Core.Entities;
using AmpedBiz.Data.Context;
using AmpedBiz.Data.Seeders.DataProviders;
using LinqToExcel;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DefaultDataSeeders
{
    public class _001_TenantSeeder : IDefaultDataSeeder
    {
        private readonly IContextProvider _contextProvider;
        private readonly ISessionFactory _sessionFactory;

        public _001_TenantSeeder(IContextProvider contextProvider, ISessionFactory sessionFactory)
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
                var seed = Tenant.Default; //TenantData.Get(context, session);

                if (!session.Query<Tenant>().Any(x => x.Id == seed.Id))
                {
                    seed.EnsureValidity();
                    session.Save(seed, seed.Id); //session.Save(seed);
                }

                transaction.Commit();
                _sessionFactory.ReleaseSharedSession();
            }
        }
    }

    internal class TenantData : DataProvider<Tenant>
    {
        public static Tenant Get(IContext context, ISession session) => new TenantData(context, session).Get().FirstOrDefault();

        public TenantData(IContext context, ISession session) : base("@tenant.xlxs", context, session) { }

        public override Tenant Map(Row row)
        {
            return new Tenant(
                id: row[nameof(Tenant.Id)],
                name: row[nameof(Tenant.Name)],
                description: row[nameof(Tenant.Description)]
            );
        }
    }
}

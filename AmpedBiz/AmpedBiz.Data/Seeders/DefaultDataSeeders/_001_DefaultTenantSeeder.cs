using AmpedBiz.Core.Entities;
using AmpedBiz.Data.Context;
using AmpedBiz.Data.Seeders.DataProviders;
using LinqToExcel;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DefaultDataSeeders
{
    public class _001_DefaultTenantSeeder : IDefaultDataSeeder
    {
        private readonly IContextProvider _contextProvider;
        private readonly ISessionFactory _sessionFactory;

        public _001_DefaultTenantSeeder(IContextProvider contextProvider, ISessionFactory sessionFactory)
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
                var dataProvider = new ExcelDataProvider(context, session);
                var entity = dataProvider.Import(@"tenant.xlsx", DataMapper.Map).FirstOrDefault();

                if (!session.Query<Tenant>().Any(x => x.Id == entity.Id))
                {
                    entity.EnsureValidity();
                    session.Save(entity);
                }

                transaction.Commit();
                _sessionFactory.ReleaseSharedSession();
            }
        }

        public class DataMapper
        {
            public static Tenant Map(Row row)
            {
                return new Tenant(
                    id: row[nameof(Tenant.Id)],
                    name: row[nameof(Tenant.Name)],
                    description: row[nameof(Tenant.Description)]
                );
            }
        }
    }
}

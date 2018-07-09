using AmpedBiz.Core.Entities;
using AmpedBiz.Data.Context;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DefaultDataSeeders
{
    public class _004_DefaultSettingSeeder : IDefaultDataSeeder
    {
        private readonly IContextProvider _contextProvider;
        private readonly ISessionFactory _sessionFactory;

        public _004_DefaultSettingSeeder(IContextProvider contextProvider, ISessionFactory sessionFactory)
        {
            _contextProvider = contextProvider;
            _sessionFactory = sessionFactory;
        }

        public bool IsSourceExternalFile => false;

        public void Seed()
        {
            var context = this._contextProvider.Build();

            using (var session = _sessionFactory.RetrieveSharedSession(context))
            using (var transaction = session.BeginTransaction())
            {
                var settings = session.Query<Setting>().Cacheable().ToList();

                var settingTypes = settings.Select(x => x.GetType()).ToList();

                if (!settingTypes.Contains(typeof(Setting<InvoiceReportSetting>)))
                    session.Save(Setting<InvoiceReportSetting>.Default());

                if (!settingTypes.Contains(typeof(Setting<UserSetting>)))
                    session.Save(Setting<UserSetting>.Default());

                if (!settingTypes.Contains(typeof(Setting<CurrencySetting>)))
                    session.Save(Setting<CurrencySetting>.Default());

                transaction.Commit();
                _sessionFactory.ReleaseSharedSession();
            }
        }
    }
}

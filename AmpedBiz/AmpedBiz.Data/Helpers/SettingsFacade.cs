using AmpedBiz.Core.Common;
using AmpedBiz.Core.Settings;
using AmpedBiz.Core.Settings.Services;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Data.Helpers
{
	public class SettingsFacade : ISettingsFacade
    {
        private readonly ISession _session;

        private readonly Lazy<IEnumerable<Setting>> _settings;

        public UserSetting Users => this.Get<UserSetting>();

        public InvoiceReportSetting InvoiceReports => this.Get<InvoiceReportSetting>();

        public CurrencySetting Currencies => this.Get<CurrencySetting>();

        public Currency DefaultCurrency => this._session.Load<Currency>(this.Get<CurrencySetting>().DefaultCurrencyId);

        private T Get<T>() where T : SettingType
        {
            return this._settings.Value.OfType<Setting<T>>().FirstOrDefault()?.Value ?? Activator.CreateInstance<T>();
        }

        public SettingsFacade(ISession session)
        {
            this._session = session;
            this._settings = new Lazy<IEnumerable<Setting>>(() => this._session.Query<Setting>());
        }
    }
}

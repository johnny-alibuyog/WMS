using AmpedBiz.Data.Context;
using NHibernate;
using System;

namespace AmpedBiz.Data.Inteceptors
{
    public class TenancyInterceptor : EmptyInterceptor
    {
        private ISession _session;

        private Func<IContext> GetContext { get; }

        public TenancyInterceptor(Func<IContext> getContext)
        {
            this.GetContext = getContext;
        }

        public override void SetSession(ISession session)
        {
            this._session = session;
        }
    }
}

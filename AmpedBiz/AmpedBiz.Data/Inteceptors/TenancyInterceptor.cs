using AmpedBiz.Core;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data.Context;
using NHibernate;
using NHibernate.Type;
using System;
using System.Linq;

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
            base.SetSession(session);
        }

        public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            var context = this.GetContext();
            var instance = entity as IHaveTenant;

            if (context.HasTenant() && instance != null)
            {
                if (instance.Tenant == null)
                {
                    var index = Array.IndexOf(propertyNames, nameof(IHaveTenant.Tenant));
                    state[index] = instance.Tenant = this._session.Load<Tenant>(context.TenantId);
                }
                else if (instance.Tenant.Id != context.TenantId)
                {
                    throw new InvalidOperationException("Ilegal data access.");
                }
            }

            return base.OnSave(entity, id, state, propertyNames, types);
        }

        public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, IType[] types)
        {
            var context = this.GetContext();
            var instance = entity as IHaveTenant;

            if (context.HasTenant() && instance != null)
            {
                if (instance.Tenant == null)
                {
                    var index = Array.IndexOf(propertyNames, nameof(IHaveTenant.Tenant));
                    currentState[index] = instance.Tenant = this._session.Load<Tenant>(context.TenantId);
                }
                else if (instance.Tenant.Id != context.TenantId)
                {
                    throw new InvalidOperationException("Ilegal data access.");
                }
            }

            return base.OnFlushDirty(entity, id, currentState, previousState, propertyNames, types);
        }

        public override void OnDelete(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            var context = this.GetContext();
            var instance = entity as IHaveTenant;

            if (context.HasTenant() && instance != null && instance.Tenant?.Id == context.TenantId)
            {
                throw new InvalidOperationException("Ilegal data access.");
            }


            base.OnDelete(entity, id, state, propertyNames, types);
        }

    }
}

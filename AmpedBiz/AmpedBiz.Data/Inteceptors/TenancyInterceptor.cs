using AmpedBiz.Core;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data.Context;
using AmpedBiz.Data.EntityDefinitions;
using NHibernate;
using NHibernate.Type;
using System;

namespace AmpedBiz.Data.Inteceptors
{
    public class TenancyInterceptor : EmptyInterceptor
    {
        private ISession _session;

        private bool IsTenantFilterEnabled => !string.IsNullOrWhiteSpace(GetContext()?.TenantId); //this._session.GetEnabledFilter(TenantDefinition.Filter.FilterName) != null;

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

        public override bool OnLoad(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            var context = this.GetContext();
            var instance = entity as IHasTenant;

            if (this.IsTenantFilterEnabled && instance != null)
            {
                var index = Array.IndexOf(propertyNames, nameof(IHasTenant.Tenant));
                var tenant = state[index] as Tenant;
                if (tenant != null && tenant.Id != context.TenantId)
                {
                    throw new InvalidOperationException("Ilegal data access.");
                }
            }

            return base.OnLoad(entity, id, state, propertyNames, types);
        }

        public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            var context = this.GetContext();
            var instance = entity as IHasTenant;

            if (this.IsTenantFilterEnabled && instance != null)
            {
                if (instance.Tenant == null)
                {
                    var index = Array.IndexOf(propertyNames, nameof(IHasTenant.Tenant));
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
            var instance = entity as IHasTenant;

            if (this.IsTenantFilterEnabled && instance != null)
            {
                if (instance.Tenant == null)
                {
                    var index = Array.IndexOf(propertyNames, nameof(IHasTenant.Tenant));
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
            var instance = entity as IHasTenant;

            if (this.IsTenantFilterEnabled && instance != null && instance.Tenant != null && instance.Tenant.Id != context.TenantId)
            {
                throw new InvalidOperationException("Ilegal data access.");
            }

            base.OnDelete(entity, id, state, propertyNames, types);
        }

    }
}

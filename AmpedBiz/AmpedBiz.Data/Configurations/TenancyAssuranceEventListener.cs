using AmpedBiz.Core;
using AmpedBiz.Data.EntityDefinitions;
using NHibernate.Event;
using NHibernate.Persister.Entity;
using System;

namespace AmpedBiz.Data.Configurations
{
    public class TenancyAssuranceEventListener : 
        IPostLoadEventListener, 
        IPreUpdateEventListener, 
        IPreInsertEventListener, 
        IPreDeleteEventListener
    {
        public void OnPostLoad(PostLoadEvent @event)
        {
            if (@event.Entity is IHaveTenant)
            {
                var tenantId = @event.Entity.GetTenantId();
                var currentTenantId = @event.Session.GetCurrentTenantId();
                if (currentTenantId != tenantId)
                    throw new InvalidOperationException("Ilegal data access.");
            }
        }

        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            if (@event.Entity is IHaveTenant)
            {
                var state = @event.OldState ?? @event.State;
                var tenantId = state.GetTenantId(@event.Persister);
                var currentTenantId = @event.Session.GetCurrentTenantId();
                if (currentTenantId != tenantId)
                    throw new InvalidOperationException("Ilegal data access.");
            }

            return false;
        }

        public bool OnPreInsert(PreInsertEvent @event)
        {
            if (@event.Entity is IHaveTenant)
            {
                var tenantId = @event.Entity.GetTenantId();
                var currentTenantId = @event.Session.GetCurrentTenantId();
                if (currentTenantId != tenantId)
                    throw new InvalidOperationException("Ilegal data access.");
            }

            return false;
        }

        public bool OnPreDelete(PreDeleteEvent @event)
        {
            if (@event.Entity is IHaveTenant)
            {
                var tenantId = @event.Entity.GetTenantId();
                var currentTenantId = @event.Session.GetCurrentTenantId();
                if (currentTenantId != tenantId)
                    throw new InvalidOperationException("Ilegal data access.");
            }

            return false;
        }
    }

    internal static class TenancyAssuranceEventListenerExtention
    {
        public static Guid GetTenantId(this object[] state, IEntityPersister persister)
        {
            return (Guid)state.GetValue(Array.IndexOf(persister.PropertyNames, "TenantId"));
        }

        public static Guid GetTenantId(this object source)
        {
            return ((IHaveTenant)source).Tenant.Id;
        }

        public static Guid GetCurrentTenantId(this IEventSource source)
        {
            return (Guid)source.GetFilterParameterValue(TenantDefinition.Filter.ParameterName);
        }
    }
}

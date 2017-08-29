using AmpedBiz.Core;
using AmpedBiz.Core.Entities;
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
            if (@event.Entity is IHaveTenant && @event.Session.TenancyFilterEnabled())
            {
                var tenantId = @event.Entity.GetTenantId();
                var currentTenantId = @event.Session.GetCurrentTenantId();
                
                if (!string.IsNullOrWhiteSpace(tenantId) && currentTenantId != tenantId)
                {
                    throw new InvalidOperationException("Ilegal data access.");
                }
            }
        }

        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            if (@event.Entity is IHaveTenant && @event.Session.TenancyFilterEnabled())
            {
                var state = @event.OldState ?? @event.State;
                var tenantId = state.GetTenantId(@event.Persister);
                var currentTenantId = @event.Session.GetCurrentTenantId();

                if (string.IsNullOrWhiteSpace(tenantId))
                {
                    ((IHaveTenant)@event.Entity).Tenant = @event.Session.Load<Tenant>(currentTenantId);
                }

                else if (currentTenantId != tenantId)
                {
                    throw new InvalidOperationException("Ilegal data access.");
                }
            }

            return false;
        }

        public bool OnPreInsert(PreInsertEvent @event)
        {
            if (@event.Entity is IHaveTenant && @event.Session.TenancyFilterEnabled())
            {
                var tenantId = @event.Entity.GetTenantId();
                var currentTenantId = @event.Session.GetCurrentTenantId();

                if (string.IsNullOrWhiteSpace(tenantId))
                {
                    ((IHaveTenant)@event.Entity).Tenant = @event.Session.Load<Tenant>(currentTenantId);
                }
                else if (currentTenantId != tenantId)
                {
                    throw new InvalidOperationException("Ilegal data access.");
                }
            }

            return false;
        }

        public bool OnPreDelete(PreDeleteEvent @event)
        {
            if (@event.Entity is IHaveTenant && @event.Session.TenancyFilterEnabled())
            {
                var tenantId = @event.Entity.GetTenantId();
                var currentTenantId = @event.Session.GetCurrentTenantId();

                if (!string.IsNullOrWhiteSpace(tenantId) && currentTenantId != tenantId)
                {
                    throw new InvalidOperationException("Ilegal data access.");
                }
            }

            return false;
        }
    }

    internal static class TenancyAssuranceEventListenerExtention
    {
        public static bool TenancyFilterEnabled(this IEventSource session)
        {
            return session.EnabledFilters.ContainsKey(TenantDefinition.Filter.FilterName);
        }

        public static string GetTenantId(this object[] state, IEntityPersister persister)
        {
            return (string)state.GetValue(Array.IndexOf(persister.PropertyNames, "TenantId"));
        }

        public static string GetTenantId(this object source)
        {
            return ((IHaveTenant)source).Tenant?.Id;
        }

        public static string GetCurrentTenantId(this IEventSource source)
        {
            return (string)source.GetFilterParameterValue($"{TenantDefinition.Filter.FilterName}.{TenantDefinition.Filter.ParameterName}");
        }
    }
}

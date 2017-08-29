﻿using NHibernate.Cfg;
using NHibernate.Event;

namespace AmpedBiz.Data.Configurations
{
    internal static class EventListenerConfiguration
    {
        public static void Configure(Configuration configuration)
        {
            configuration.AppendListeners(ListenerType.PreInsert, new IPreInsertEventListener[] { new ValidationEventListener(), new TenancyAssuranceEventListener(), });
            configuration.AppendListeners(ListenerType.PreUpdate, new IPreUpdateEventListener[] { new ValidationEventListener(), new TenancyAssuranceEventListener(), });
            configuration.AppendListeners(ListenerType.PreDelete, new IPreDeleteEventListener[] { new TenancyAssuranceEventListener(), });
            configuration.AppendListeners(ListenerType.PreCollectionRecreate, new IPreCollectionRecreateEventListener[] { new ValidationEventListener(), });
            configuration.AppendListeners(ListenerType.PreCollectionUpdate, new IPreCollectionUpdateEventListener[] { new ValidationEventListener(), });
            configuration.AppendListeners(ListenerType.PreCollectionRemove, new IPreCollectionRemoveEventListener[] { new ValidationEventListener(), });
            configuration.AppendListeners(ListenerType.PostLoad, new IPostLoadEventListener[] { new TenancyAssuranceEventListener(), });
            configuration.AppendListeners(ListenerType.PostInsert, new IPostInsertEventListener[] { new AuditEventListener(), });
            configuration.AppendListeners(ListenerType.PostUpdate, new IPostUpdateEventListener[] { new AuditEventListener(), });
            configuration.AppendListeners(ListenerType.PostDelete, new IPostDeleteEventListener[] { new AuditEventListener(), });
        }
    }
}

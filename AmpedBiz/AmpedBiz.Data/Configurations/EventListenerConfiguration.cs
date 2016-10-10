using NHibernate.Cfg;
using NHibernate.Event;

namespace AmpedBiz.Data.Configurations
{
    internal static class EventListenerConfiguration
    {
        public static void Configure(this Configuration configuration)
        {
            // Validation Interceptors
            configuration.AppendListeners(ListenerType.PreInsert, new IPreInsertEventListener[] { new ValidationEventListener(), });
            configuration.AppendListeners(ListenerType.PreUpdate, new IPreUpdateEventListener[] { new ValidationEventListener(), });
            configuration.AppendListeners(ListenerType.PreCollectionRecreate, new IPreCollectionRecreateEventListener[] { new ValidationEventListener(), });
            configuration.AppendListeners(ListenerType.PreCollectionUpdate, new IPreCollectionUpdateEventListener[] { new ValidationEventListener(), });
            configuration.AppendListeners(ListenerType.PreCollectionRemove, new IPreCollectionRemoveEventListener[] { new ValidationEventListener(), });

            // Audit Interceptors
            configuration.AppendListeners(ListenerType.PostInsert, new IPostInsertEventListener[] { new AuditEventListener(), });
            configuration.AppendListeners(ListenerType.PostUpdate, new IPostUpdateEventListener[] { new AuditEventListener(), });
            configuration.AppendListeners(ListenerType.PostDelete, new IPostDeleteEventListener[] { new AuditEventListener(), });
        }
    }
}

using NHibernate.Cfg;
using NHibernate.Event;

namespace AmpedBiz.Data.Configurations
{
    internal static class EventListenerConfiguration
    {
        public static void Configure(this Configuration configuration)
        {
            configuration.AppendListeners(ListenerType.PreInsert, new IPreInsertEventListener[] { new ValidationEventListener(), });
            configuration.AppendListeners(ListenerType.PreUpdate, new IPreUpdateEventListener[] { new ValidationEventListener(), });

            //configuration.AppendListeners(ListenerType.PostInsert, new IPostInsertEventListener[] { new AuditEventListener(), });
            //configuration.AppendListeners(ListenerType.PostUpdate, new IPostUpdateEventListener[] { new AuditEventListener(), });
            //configuration.AppendListeners(ListenerType.PostDelete, new IPostDeleteEventListener[] { new AuditEventListener(), });
        }
    }

}

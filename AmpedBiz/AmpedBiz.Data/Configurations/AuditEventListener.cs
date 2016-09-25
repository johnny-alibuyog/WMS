using System;
using AmpedBiz.Core;
using AmpedBiz.Core.Entities;
using NHibernate;
using NHibernate.Event;

namespace AmpedBiz.Data.Configurations
{
    internal class AuditEventListener : IPostInsertEventListener, IPostUpdateEventListener, IPostDeleteEventListener
    {
        public void OnPostDelete(PostDeleteEvent @event)
        {
            // TODO: 
        }

        public void OnPostInsert(PostInsertEvent @event)
        {
            var auditProvider = SessionFactoryProvider.AuditProvider;
            var session = @event.Session.GetSession(EntityMode.Poco);

            var entity = @event.Entity as IAuditable;
            if (entity == null)
                return;

            var userId = auditProvider.GetCurrentUserId();
            if (userId == null)
                return;

            entity.CreatedBy = session.Load<User>(userId);
            entity.CreatedOn = DateTime.Now;
        }

        public void OnPostUpdate(PostUpdateEvent @event)
        {
            var auditProvider = SessionFactoryProvider.AuditProvider;
            var session = @event.Session.GetSession(EntityMode.Poco);

            var entity = @event.Entity as IAuditable;
            if (entity == null)
                return;

            var currentUser = auditProvider.GetCurrentUserId();
            if (currentUser == null)
                return;

            entity.ModifiedBy = session.Load<User>(currentUser);
            entity.ModifiedOn = DateTime.Now;
        }
    }
    
    /*
    internal class AuditEventListener : IPostInsertEventListener, IPostUpdateEventListener, IPostDeleteEventListener
    {
        private string ParseName(object entity)
        {
            if (entity == null)
                return "Entity Not Initialized";

            return entity.GetType().ToString().Split('.').Last();
        }

        public void OnPostInsert(PostInsertEvent @event)
        {
            if (!(@event.Entity is IAuditable))
                return;

            var session = @event.Session.GetSession(EntityMode.Poco);

            var audit = new Audit2()
            {
                UserName = Thread.CurrentPrincipal.Identity.Name,
                EntityId = @event.Id.ToString(),
                EntityName = ParseName(@event.Entity),
                Operation = "Insert",
            };

            session.Save(audit);
            session.Flush();
        }

        public void OnPostUpdate(PostUpdateEvent @event)
        {
            //www.robertgray.net.au/posts/2011/6/auditing-with-nhibernate#.VYuRmPmqpuA

            if (!(@event.Entity is IAuditable))
                return;

            var session = @event.Session.GetSession(EntityMode.Poco);

            for (var index = 0; index < @event.State.Length; index++)
            {
                var newState= @event.State[index];
                var oldState = @event.OldState[index];

                if (newState != oldState)
                {
                    //var baseType = newState.GetType().BaseType;
                    //var isValueObject = baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(ValueObjectBase<>);

                    if (newState.GetType().IsValueType) // primitive type
                    {
                        var audit = new Audit2()
                        {
                            UserName = Thread.CurrentPrincipal.Identity.Name,
                            EntityId = @event.Id.ToString(),
                            EntityName = ParseName(@event.Entity),
                            FieldName = @event.Persister.PropertyNames[index],
                            OldValue = oldState != null ? oldState.ToString() : null,
                            NewValue = newState != null ? newState.ToString() : null,
                            Operation = "Update"
                        };

                        session.Save(audit);
                    }
                    else // complex type
                    {
                        var type = newState.GetType();
                        var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                        // iterate through each property of the complex type
                        foreach (var property in properties)
                        {
                            var newValue = property.GetValue(newState);
                            var oldValue = property.GetValue(oldState);

                            var audit = new Audit2()
                            {
                                UserName = Thread.CurrentPrincipal.Identity.Name,
                                EntityId = @event.Id.ToString(),
                                EntityName = ParseName(@event.Entity),
                                FieldName = @event.Persister.PropertyNames[index] + "." + property.Name,
                                OldValue = oldValue != null ? oldValue.ToString() : null,
                                NewValue = newValue != null ? newValue.ToString() : null,
                                Operation = "Update"
                            };

                            session.Save(audit);
                        }
                    }
                }
            }

            session.Flush();
        }

        public void OnPostDelete(PostDeleteEvent @event)
        {
            if (!(@event.Entity is IAuditable))
                return;

            var session = @event.Session.GetSession(EntityMode.Poco);

            var audit = new Audit2()
            {
                UserName = Thread.CurrentPrincipal.Identity.Name,
                EntityId = @event.Id.ToString(),
                EntityName = ParseName(@event.Entity),
                Operation = "Delete",
            };

            session.Save(audit);
            session.Flush();
        }
    }
    */


    /*
    internal class AuditEventListener : IPreInsertEventListener, IPreUpdateEventListener, IPreDeleteEventListener
    {
        #region Routine Helpers

        private void Set(IEntityPersister persister, object[] state, string propertyName, object value)
        {
            var index = Array.IndexOf(persister.PropertyNames, propertyName);
            if (index == -1)
                return;

            state[index] = value;
        }

        private IAuditProvider GetAuditProvider(AbstractPreDatabaseOperationEvent @event)
        {
            var resolver = SessionProvider.AuditProvider;
            if (resolver == null)
                return null;

            var info = @event.Entity
                .GetType()
                .GetProperties()
                .Where(x => x.PropertyType == typeof(Audit))
                .Select(x => new
                {
                    PropertyInfo = x,
                    Value = x.GetValue(@event.Entity, null) as Audit
                })
                .FirstOrDefault();

            if (info == null)
                return null;

            resolver.PropertyInfo = info.PropertyInfo;
            resolver.CurrentAudit = info.Value;

            return resolver;
        }

        #endregion

        public bool OnPreInsert(PreInsertEvent @event)
        {
            var auditResolver = GetAuditProvider(@event);
            if (auditResolver == null)
                return false;

            var newAudit = auditResolver.CreateNew();

            Set(@event.Persister, @event.State, auditResolver.PropertyInfo.Name, newAudit);

            auditResolver.PropertyInfo.SetValue(@event.Entity, newAudit, null);

            return false;

            return false;
        }

        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            var auditResolver = GetAuditProvider(@event);
            if (auditResolver == null)
                return false;

            var updatedAudit = auditResolver.CreateUpdate();

            Set(@event.Persister, @event.State, auditResolver.PropertyInfo.Name, updatedAudit);

            auditResolver.PropertyInfo.SetValue(@event.Entity, updatedAudit, null);

            return false;
        }

        public bool OnPreDelete(PreDeleteEvent @event)
        {
            throw new NotImplementedException();
        }
    }
    */
}

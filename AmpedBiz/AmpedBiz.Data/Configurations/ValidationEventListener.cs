using System;
using System.Linq;
using AmpedBiz.Common.Exceptions;
using NHibernate.Event;

namespace AmpedBiz.Data.Configurations
{
    internal class ValidationEventListener : IPreInsertEventListener, IPreUpdateEventListener, IPreCollectionRemoveEventListener, IPreCollectionUpdateEventListener, IPreCollectionRecreateEventListener
    {
        private void PerformValidation(object entity)
        {
            var validator = SessionProvider.Validator;
            var invalidValues = validator.Validate(entity);
            if (invalidValues.Count() > 0)
            {
                var invalidFields = invalidValues
                    .Select(x => new InvalidField()
                    {
                        Entity = x.EntityType.Name,
                        Property = x.PropertyName,
                        Message = x.Message,
                        Value = x.Value,
                    })
                    .ToArray();

                throw new ValidationExceptionBuilder().Build(invalidFields);
            }
        }

        public bool OnPreInsert(PreInsertEvent @event)
        {
            PerformValidation(@event.Entity);
            return false;
        }

        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            PerformValidation(@event.Entity);
            return false;
        }

        public void OnPreRemoveCollection(PreCollectionRemoveEvent @event)
        {
            PerformValidation(@event.AffectedOwnerOrNull);
        }

        public void OnPreUpdateCollection(PreCollectionUpdateEvent @event)
        {
            PerformValidation(@event.AffectedOwnerOrNull);
        }

        public void OnPreRecreateCollection(PreCollectionRecreateEvent @event)
        {
            PerformValidation(@event.AffectedOwnerOrNull);
        }
    }

}

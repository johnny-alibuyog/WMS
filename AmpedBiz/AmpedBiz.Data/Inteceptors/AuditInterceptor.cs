using AmpedBiz.Core;
using AmpedBiz.Core.SharedKernel;
using AmpedBiz.Core.Users;
using AmpedBiz.Data.Context;
using NHibernate;
using NHibernate.Type;
using System;
using System.Linq;

namespace AmpedBiz.Data.Inteceptors
{
	public class AuditInterceptor : EmptyInterceptor
    {
        private ISession _session;

        private Func<IContext> GetContext { get; }

        public AuditInterceptor(Func<IContext> getContext)
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
            var auditable = entity as IAuditable;
            if (auditable != null)
            {
                auditable.CreatedBy = this.Set(nameof(auditable.CreatedBy), this._session.Get<User>(this.GetContext().UserId), state, propertyNames);
                auditable.CreatedOn = this.Set(nameof(auditable.CreatedOn), DateTime.Now, state, propertyNames);
            }

            return base.OnSave(entity, id, state, propertyNames, types);
        }

        public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, IType[] types)
        {
            var auditable = entity as IAuditable;
            if (auditable != null)
            {
                auditable.ModifiedBy = this.Set(nameof(auditable.ModifiedBy), this._session.Get<User>(this.GetContext().UserId), currentState, propertyNames);
                auditable.ModifiedOn = this.Set(nameof(auditable.ModifiedOn), DateTime.Now, currentState, propertyNames);
            }

            return base.OnFlushDirty(entity, id, currentState, previousState, propertyNames, types);
        }

        private TValue Set<TValue>(string propertyName, TValue propertyValue, object[] state, string[] propertyNames)
        {
            var property = propertyNames
                .Select((x, i) => new { Name = x, Index = i })
                .FirstOrDefault(x => x.Name == propertyName);

            state[property.Index] = propertyValue;

            return propertyValue;
        }

    }
}

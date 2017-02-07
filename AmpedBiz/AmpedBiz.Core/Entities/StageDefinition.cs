using AmpedBiz.Common.Extentions;
using System.Linq;

namespace AmpedBiz.Core.Entities
{
    public class StageDefenition<TStatus, TAggregate>
    {
        public virtual TStatus[] AllowedTransitions { get; private set; }

        public virtual TAggregate[] AllowedModifications { get; private set; }

        public StageDefenition() { }

        public StageDefenition(TStatus[] allowedTransitions, TAggregate[] allowedModifications)
        {
            this.AllowedTransitions = allowedTransitions;
            this.AllowedModifications = allowedModifications;
        }

        public StageDefenition(string[] allowedTransitions, string[] allowedModifications)
        {
            this.AllowedTransitions = allowedTransitions
                .Select(x => x.As<TStatus>())
                .ToArray();

            this.AllowedModifications = allowedModifications
                .Select(x => x.As<TAggregate>())
                .ToArray();
            ;
        }

        public bool IsTransitionAllowedTo(TStatus status)
        {
            return this.AllowedTransitions.Contains(status);
        }

        public bool IsModificationAllowedTo(TAggregate aggregate)
        {
            return this.AllowedModifications.Contains(aggregate);
        }

        public bool IsModificationAllowed()
        {
            return this.AllowedModifications.Any();
        }
    }
}

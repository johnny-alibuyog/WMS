namespace AmpedBiz.Service.Dto
{
    public class StageDefenition<TStatus, TAggregate>
    {
        public virtual TStatus[] AllowedTransitions { get; set; }

        public virtual TAggregate[] AllowedModifications { get; set; }
    }
}
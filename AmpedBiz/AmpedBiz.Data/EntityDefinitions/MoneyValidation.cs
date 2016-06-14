using AmpedBiz.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class MoneyValidation : ValidationDef<Money>
    {
        public MoneyValidation()
        {
            Define(x => x.Amount);

            Define(x => x.Currency)
                .IsValid();
        }
    }
}

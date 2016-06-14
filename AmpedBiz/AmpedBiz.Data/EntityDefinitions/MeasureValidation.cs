using AmpedBiz.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class MeasureValidation : ValidationDef<Measure>
    {
        public MeasureValidation()
        {
            Define(x => x.Value);

            Define(x => x.Unit)
                .IsValid();
        }
    }
}

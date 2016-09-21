using AmpedBiz.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class ReturnValidation : ValidationDef<Return>
    {
        public ReturnValidation()
        {
            Define(x => x.Id);

            Define(x => x.Branch)
                .NotNullable();

            Define(x => x.Customer)
                .NotNullable();

            Define(x => x.ReturnedBy)
                .NotNullable();

            Define(x => x.ReturnedOn);

            Define(x => x.Reason)
                .NotNullable();

            Define(x => x.Remarks)
                .MaxLength(500);

            Define(x => x.Items)
                .NotNullableAndNotEmpty()
                .And.HasValidElements();
        }
    }
}

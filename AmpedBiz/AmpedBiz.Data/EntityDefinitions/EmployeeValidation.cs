using AmpedBiz.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class EmployeeValidation : ValidationDef<Employee>
    {
        public EmployeeValidation()
        {
            Define(x => x.Id);

            Define(x => x.Contact);

            Define(x => x.EmployeeType);

            Define(x => x.User)
                .NotNullable()
                .And
                .IsValid();
        }
    }
}
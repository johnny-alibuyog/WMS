using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmpedBiz.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class EmployeeTypeValidation : ValidationDef<EmployeeType>
    {
        public EmployeeTypeValidation()
        {
            Define(x => x.Id)
                .NotNullableAndNotEmpty()
                .And.MaxLength(30);

            Define(x => x.Name)
                .NotNullableAndNotEmpty()
                .And.MaxLength(150);
        }
    }
}

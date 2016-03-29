﻿using AmpedBiz.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class CustomerValidation : ValidationDef<Customer>
    {
        public CustomerValidation()
        {
            Define(x => x.Id)
                .NotNullableAndNotEmpty();

            Define(x => x.Name)
                .NotNullableAndNotEmpty()
                .And.MaxLength(255);
        }
    }
}
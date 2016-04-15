﻿using AmpedBiz.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class CurrencyValidation : ValidationDef<Currency>
    {
        public CurrencyValidation()
        {
            Define(x => x.Id)
               .NotNullableAndNotEmpty()
               .And.MaxLength(30);

            Define(x => x.Symbol)
                .NotNullableAndNotEmpty()
                .And.MaxLength(150);

            Define(x => x.Name)
                .NotNullableAndNotEmpty()
                .And.MaxLength(150);
        }
    }
}

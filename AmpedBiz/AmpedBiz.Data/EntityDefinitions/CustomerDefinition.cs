﻿using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class CustomerDefinition
    {
        public class Mapping : ClassMap<Customer>
        {
            public Mapping()
            {
                Id(x => x.Id)
                    .GeneratedBy.GuidComb();

                Map(x => x.Code);

                Map(x => x.Name);

                References(x => x.Pricing);

                Component(x => x.Contact);

                Map(x => x.IsActive);

                Component(x => x.CreditLimit,
                    MoneyDefinition.Mapping.Map("CreditLimit_", nameof(Customer)));

                Component(x => x.OfficeAddress,
                    AddressDefenition.Mapping.Map("Office_"));

                Component(x => x.BillingAddress,
                    AddressDefenition.Mapping.Map("Billing_"));
            }
        }

        public class Validation : ValidationDef<Customer>
        {
            public Validation()
            {
                Define(x => x.Id);

                Define(x => x.Code)
                    .MaxLength(255);

                Define(x => x.Name)
                    .NotNullableAndNotEmpty()
                    .And.MaxLength(255);

                Define(x => x.Pricing)
                    .IsValid();

                Define(x => x.Contact)
                    .IsValid();

                Define(x => x.IsActive);

                Define(x => x.CreditLimit)
                    .IsValid();

                Define(x => x.OfficeAddress)
                    .IsValid();

                Define(x => x.BillingAddress)
                    .IsValid();
            }
        }
    }
}
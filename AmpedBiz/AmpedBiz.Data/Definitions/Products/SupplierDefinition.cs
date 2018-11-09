﻿using AmpedBiz.Core.Products;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.Definitions.Products
{
	public class SupplierDefinition
	{
		public class Mapping : ClassMap<Supplier>
		{
			public Mapping()
			{
				Id(x => x.Id)
					.GeneratedBy.GuidComb();

				Map(x => x.Code);

				Map(x => x.Name);

				Map(x => x.ContactPerson);

				Component(x => x.Address);

				Component(x => x.Contact);

				HasMany(x => x.Products)
					.Cascade.AllDeleteOrphan()
					.Not.KeyNullable()
					.Not.KeyUpdate()
					.Inverse()
					.AsBag();

				//ApplyFilter<TenantDefinition.Filter>();
			}
		}

		public class Validation : ValidationDef<Supplier>
		{
			public Validation()
			{
				Define(x => x.Id);

				Define(x => x.Code)
					.MaxLength(150);

				Define(x => x.Name)
					.NotNullableAndNotEmpty()
					.And.MaxLength(150);

				Define(x => x.ContactPerson)
					.MaxLength(150);

				Define(x => x.Address)
					.IsValid();

				Define(x => x.Contact)
					.IsValid();

				Define(x => x.Products)
					.HasValidElements();
			}
		}
	}
}

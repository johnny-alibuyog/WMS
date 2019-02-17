using AmpedBiz.Core.Products;
using AmpedBiz.Data.Definitions.Common;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.Definitions.Products
{
	public class ProductCategoryDefinition
	{
		public class Mapping : ClassMap<ProductCategory>
		{
			public Mapping()
			{
				Id(x => x.Id)
					.GeneratedBy.Assigned();

				References(x => x.Tenant);

				Map(x => x.Name);

				ApplyFilter<TenantDefinition.Filter>();
			}
		}

		public class Vaildation : ValidationDef<ProductCategory>
		{
			public Vaildation()
			{
				Define(x => x.Id)
					.NotNullableAndNotEmpty()
					.And.MaxLength(150);

				Define(x => x.Tenant)
					.IsValid();

				Define(x => x.Name)
					.NotNullableAndNotEmpty()
					.And.MaxLength(150);
			}
		}
	}
}

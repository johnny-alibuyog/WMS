using AmpedBiz.Core.Settings;
using AmpedBiz.Data.Definitions.Common;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.Definitions.Settings
{
	public class SettingDefinition
	{
		public class Mapping : ClassMap<Setting>
		{
			public Mapping()
			{

				Id(x => x.Id)
					.GeneratedBy.GuidComb();

				References(x => x.Tenant);

				DiscriminateSubClassesOnColumn("Type");

				ApplyFilter<TenantDefinition.Filter>();
			}
		}

		public class Validation : ValidationDef<Setting>
		{
			public Validation()
			{
				Define(x => x.Id);

				Define(x => x.Tenant);
			}
		}
	}
}

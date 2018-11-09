using AmpedBiz.Core.Settings;
using AmpedBiz.Data.CustomTypes;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.Definitions.Settings
{
	public class UserSettingDefinition
	{
		public class Mapping : SubclassMap<Setting<UserSetting>>
		{
			public Mapping()
			{
				Map(x => x.Value)
					.CustomType<JsonType<UserSetting>>();

				DiscriminatorValue(nameof(UserSetting));
			}
		}

		public class Validation : ValidationDef<Setting<UserSetting>>
		{
			public Validation()
			{
				Define(x => x.Value);
			}
		}
	}
}

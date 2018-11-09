using AmpedBiz.Core.Settings;
using AmpedBiz.Data.CustomTypes;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.Definitions.Settings
{
	public class InvoiceReportSettingDefinition
	{
		public class Mapping : SubclassMap<Setting<InvoiceReportSetting>>
		{
			public Mapping()
			{
				Map(x => x.Value)
					.CustomType<JsonType<InvoiceReportSetting>>();

				DiscriminatorValue(nameof(InvoiceReportSetting));
			}
		}

		public class Validation : ValidationDef<Setting<InvoiceReportSetting>>
		{
			public Validation()
			{
				Define(x => x.Value);
			}
		}
	}
}

using AmpedBiz.Core.Products;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;
using System;

namespace AmpedBiz.Data.Definitions.Products
{
	public class MeasureDefinition
	{
		public class Mapping : ComponentMap<Measure>
		{
			public Mapping()
			{
				Map(x => x.Value);

				References(x => x.Unit);
			}

			internal static Action<ComponentPart<Measure>> Map(string prefix = "", string parent = "")
			{
				return mapping =>
				{
					mapping.Map(x => x.Value, $"{prefix}Value");

					mapping.References(x => x.Unit, $"{prefix}UnitId")
						.ForeignKey($"FK_{parent}_{prefix}Unit");
				};
			}
		}

		public class Validation : ValidationDef<Measure>
		{
			public Validation()
			{
				Define(x => x.Value);

				Define(x => x.Unit)
					.IsValid();

				this.ValidateInstance.By((instance, context) =>
				{
					var valid = true;

					if (instance != null && instance.Unit == null)
					{
						context.AddInvalid<Measure, UnitOfMeasure>(
							message: $"Unit should not be null.",
							property: x => x.Unit
						);
						valid = false;
					}

					return valid;
				});
			}
		}
	}
}

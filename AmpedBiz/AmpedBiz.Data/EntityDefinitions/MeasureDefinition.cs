using System;
using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
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
            }
        }
    }
}

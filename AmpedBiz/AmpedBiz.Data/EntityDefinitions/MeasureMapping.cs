using System;
using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class MeasureMapping : ComponentMap<Measure>
    {
        public MeasureMapping()
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
}

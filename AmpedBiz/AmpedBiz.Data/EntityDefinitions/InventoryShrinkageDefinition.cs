using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class InventoryShrinkageDefinition
    {
        public class Mapping : ClassMap<InventoryShrinkage>
        {
            public Mapping()
            {
                Id(x => x.Id)
                    .GeneratedBy.GuidComb();

                References(x => x.Inventory);

                References(x => x.ShrinkedBy);

                Map(x => x.ShrinkedOn);

                References(x => x.Reason);

                Map(x => x.Remarks);

                Component(x => x.Shrinkage,
                    MeasureDefinition.Mapping.Map("Shrinkage", nameof(InventoryShrinkage)));
            }
        }

        public class Validation : ValidationDef<InventoryShrinkage>
        {
            public Validation()
            {
                Define(x => x.Id);

                Define(x => x.Inventory)
                    .NotNullable()
                    .And.IsValid();

                Define(x => x.ShrinkedBy)
                    .NotNullable()
                    .And.IsValid();

                Define(x => x.ShrinkedOn);

                Define(x => x.Reason)
                    .NotNullable()
                    .And.IsValid();

                Define(x => x.Remarks)
                    .MaxLength(750);

                Define(x => x.Shrinkage)
                    .NotNullable()
                    .And.IsValid();
            }
        }
    }
}

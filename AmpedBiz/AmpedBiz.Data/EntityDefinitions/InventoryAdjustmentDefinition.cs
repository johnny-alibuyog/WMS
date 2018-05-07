using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class InventoryAdjustmentDefinition
    {
        public class Mapping : ClassMap<InventoryAdjustment>
        {
            public Mapping()
            {
                Id(x => x.Id)
                    .GeneratedBy.GuidComb();

                References(x => x.Inventory);

                References(x => x.AdjustedBy);

                Map(x => x.AdjustedOn);

                Map(x => x.Reason);

                Map(x => x.AdjustmentType);

                Component(x => x.Adjustment, 
                    MeasureDefinition.Mapping.Map("Adjustment_", nameof(InventoryAdjustment)));
            }
        }

        public class Validation : ValidationDef<InventoryAdjustment>
        {
            public Validation()
            {
                Define(x => x.Id);

                Define(x => x.Inventory)
                    .NotNullable()
                    .And.IsValid();

                Define(x => x.AdjustedBy)
                    .NotNullable()
                    .And.IsValid();

                Define(x => x.AdjustedOn);

                Define(x => x.Reason)
                    .NotNullableAndNotEmpty()
                    .And.MaxLength(750);

                Define(x => x.AdjustmentType);

                Define(x => x.Adjustment)
                    .NotNullable()
                    .And.IsValid();
            }
        }
    }
}

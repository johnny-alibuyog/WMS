using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class StockDefinition
    {
        public class Mapping : ClassMap<Stock>
        {
            public Mapping()
            {
                Id(x => x.Id)
                    .GeneratedBy.GuidComb();

                Map(x => x.CreatedOn);

                Map(x => x.ModifiedOn);

                References(x => x.CreatedBy);

                References(x => x.ModifiedBy);

                References(x => x.Inventory);

                Component(x => x.Quantity,
                    MeasureDefinition.Mapping.Map("Quantity_", nameof(Stock)));

                Map(x => x.ExpiresOn);

                Map(x => x.Bad);

                DiscriminateSubClassesOnColumn("Movement")
                    .Length(20);
            }
        }

        public class Validation : ValidationDef<Stock>
        {
            public Validation()
            {
                Define(x => x.Id);

                Define(x => x.CreatedOn);

                Define(x => x.ModifiedOn);

                Define(x => x.CreatedBy)
                    .NotNullable()
                    .And.IsValid();

                Define(x => x.ModifiedBy);

                Define(x => x.Inventory)
                    .NotNullable()
                    .And.IsValid();

                Define(x => x.Quantity)
                    .NotNullable()
                    .And.IsValid();

                Define(x => x.ExpiresOn);

                Define(x => x.Bad);
            }
        }

    }

    public class ReceivedStockDefinition
    {
        public class Mapping : SubclassMap<ReceivedStock>
        {
            public Mapping()
            {
                DiscriminatorValue("Received");
            }
        }

        public class Validation : ValidationDef<ReceivedStock> { }
    }

    public class ReleasedStockDefinition
    {
        public class Mapping : SubclassMap<ReleasedStock>
        {
            public Mapping()
            {
                DiscriminatorValue("Released");
            }
        }

        public class Validation : ValidationDef<ReleasedStock> { }
    }

    public class ShrinkedStockDefinition
    {

        public class Mapping : SubclassMap<ShrinkedStock>
        {
            public Mapping()
            {
                DiscriminatorValue("Shrinked");

                //References(x => x.Cause);

                //Map(x => x.Remarks);
            }
        }

        public class Validation : ValidationDef<ShrinkedStock>
        {
            public Validation()
            {
                Define(x => x.Cause)
                    .NotNullable()
                    .And.IsValid();

                Define(x => x.Remarks)
                    .MaxLength(700);
            }
        }
    }
}

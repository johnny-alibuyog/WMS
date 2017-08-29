using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class ReturnItemDefinition
    {
        public class Mapping : ClassMap<ReturnItem>
        {
            public Mapping()
            {
                Id(x => x.Id)
                    .GeneratedBy.GuidComb();

                References(x => x.Product);

                References(x => x.Return);

                References(x => x.ReturnReason);

                Component(x => x.Quantity,
                    MeasureDefinition.Mapping.Map("Quantity_", nameof(ReturnItem)));

                Component(x => x.Standard,
                    MeasureDefinition.Mapping.Map("Standard_", nameof(ReturnItem)));

                Component(x => x.QuantityStandardEquivalent,
                    MeasureDefinition.Mapping.Map("QuantityStandardEquivalent_", nameof(ReturnItem)));

                Component(x => x.UnitPrice,
                    MoneyDefinition.Mapping.Map("UnitPrice_", nameof(ReturnItem)));

                Component(x => x.TotalPrice,
                    MoneyDefinition.Mapping.Map("TotalPrice_", nameof(ReturnItem)));
            }
        }

        public class Validation : ValidationDef<ReturnItem>
        {
            public Validation()
            {
                Define(x => x.Id);

                Define(x => x.Product)
                    .NotNullable();

                Define(x => x.Return)
                    .NotNullable();

                Define(x => x.ReturnReason)
                    .NotNullable();

                Define(x => x.Quantity)
                    .IsValid();

                Define(x => x.Standard)
                    .IsValid();

                Define(x => x.QuantityStandardEquivalent)
                    .IsValid();

                Define(x => x.UnitPrice)
                    .IsValid();

                Define(x => x.TotalPrice)
                    .IsValid();
            }
        }
    }
}
